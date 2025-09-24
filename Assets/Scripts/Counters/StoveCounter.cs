using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    [SerializeField] FryingRecepieSO[] fryingRecepieSOArray;
    [SerializeField] BurningRecepieSO[] burningRecepieSOArray;
    private NetworkVariable<float> fryingTimer = new NetworkVariable<float>(0f);
    private NetworkVariable <float> burningTimer= new NetworkVariable<float>(0f);
    private FryingRecepieSO fryingRecipeSO;
    private BurningRecepieSO burningRecepieSO;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }
    private NetworkVariable< State> state= new NetworkVariable<State>(State.Idle);
   
    public override void OnNetworkSpawn()
    {
        fryingTimer.OnValueChanged += FryingTimer_OnValueChanged;
        burningTimer.OnValueChanged += BurningTimer_OnValueChanged;
        state.OnValueChanged += State_OnValueChanged;
    }
    private void FryingTimer_OnValueChanged(float previousValue, float newValue)
    {
        float fryingTimerMax = fryingRecipeSO != null ? fryingRecipeSO.fryingProgresMax : 1f;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = fryingTimer.Value / fryingTimerMax
        });
    }
    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = state.Value
        });
        if(state.Value==State.Idle||state.Value==State.Burned)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = 0f
            });
        }
    } 
    private void BurningTimer_OnValueChanged(float previousValue, float newValue)
    {
        float burningTimerMax = burningRecepieSO != null ? burningRecepieSO.burningProgresMax : 1f;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = burningTimer.Value / burningTimerMax
        });
    }
    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        if (HasKitchenObject())
        {
            switch (state.Value)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer.Value += Time.deltaTime;



                    if (fryingTimer.Value >= fryingRecipeSO.fryingProgresMax)
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        state.Value = State.Fried;
                        burningTimer.Value = 0f;
                        fryingTimer.Value = 0f;
                        SetBurningRecipeSOClientRpc(
                            KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(GetKitchenObject().GetkitchenObjectSO())
                            );
                       

                    }
                    break;
                case State.Fried:
                    burningTimer.Value += Time.deltaTime;
                  
                    if (burningTimer.Value > burningRecepieSO.burningProgresMax)
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                        KitchenObject.SpawnKitchenObject(burningRecepieSO.output, this);
                        state.Value = State.Burned;
                       
                    }
                    break;
                case State.Burned:
                    break;
            }
        }

    }
    public override void Interact(PlayerController player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetkitchenObjectSO()))
                {
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    kitchenObject.SetKitchenObjectParent(this);

                    InteractLogicPlaceObjectOnCounterServerRpc(
                        KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObject.GetkitchenObjectSO()));


                }
            }

        }
        else
        {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetkitchenObjectSO()))
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                        state.Value = State.Idle;

                        SetStateToIdleServerRpc();

                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                SetStateToIdleServerRpc();
            }
        }

    }
    [ServerRpc (RequireOwnership = false)]
    private void SetStateToIdleServerRpc()
    {
        state.Value = State.Idle;

    }
    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPlaceObjectOnCounterServerRpc(int kitchenObjectSOIndex)
    {
        SetFryingRecipeSOClientRpc(kitchenObjectSOIndex);
        fryingTimer.Value = 0f;
    }
    [ClientRpc]
    private void SetFryingRecipeSOClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectByIndex(kitchenObjectSOIndex);
        fryingRecipeSO = GetFryingRecipeSOWithInput(kitchenObjectSO);

        state.Value = State.Frying;

       
    }
    [ClientRpc]
    private void SetBurningRecipeSOClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectByIndex(kitchenObjectSOIndex);
        burningRecepieSO = GetBurningRecipeSOWithInput(kitchenObjectSO);

        state.Value = State.Fried;
    }
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecepieSO fryingRecepieSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecepieSO != null;
    }
    private FryingRecepieSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecepieSO fryingRecepieSO in fryingRecepieSOArray)
        {
            if (fryingRecepieSO.input == inputKitchenObjectSO)
            {
                return fryingRecepieSO;
            }
        }
        return null;
    }
    private BurningRecepieSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecepieSO burningRecepieSO in burningRecepieSOArray)
        {
            if (burningRecepieSO.input == inputKitchenObjectSO)
            {
                return burningRecepieSO;
            }
        }
        return null;
    }
    public bool IsFried()
    {
        return state.Value == State.Fried;
    }
}
