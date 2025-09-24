using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler OnCut;

    public static event EventHandler OnAnyCat;

    [SerializeField] private CuttingRecepieSO[] cuttingRecipeSOArray;

    new public static void ResetStaticData()
    {
        OnAnyCat = null;
    }
    private int cuttingProgres;
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
                    InteractLogicPlaceObjectOnCounterServerRpc();
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
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }
    [ServerRpc (RequireOwnership =false)]
    private void InteractLogicPlaceObjectOnCounterServerRpc()
    {
        InteractLogicPlaceObjectOnCounterClientRpc();
    }
    [ClientRpc]
    private void InteractLogicPlaceObjectOnCounterClientRpc()
    {
        cuttingProgres = 0;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = 0f
        });
    }
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecepieSO cuttingRecepieSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecepieSO != null;
    }
    private CuttingRecepieSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecepieSO cuttingRecepieSO in cuttingRecipeSOArray)
        {
            if (cuttingRecepieSO.input == inputKitchenObjectSO)
            {
                return cuttingRecepieSO;
            }
        }
        return null;
    }
    [ServerRpc(RequireOwnership = false)]
    private void CutObjectServerRpc()
    {
        CutObjectClientRpc();
        TestingDoneCuttingServerRpc();
    }
    [ClientRpc]
    private void CutObjectClientRpc()
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetkitchenObjectSO()))
        {
            cuttingProgres++;
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCat?.Invoke(this, EventArgs.Empty);
            CuttingRecepieSO cuttingRecepieSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetkitchenObjectSO());


            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgres / cuttingRecepieSO.cuttingProgresMax
            });
        }
    }
    [ServerRpc(RequireOwnership =false)]
    private void TestingDoneCuttingServerRpc()
    {
        CuttingRecepieSO cuttingRecepieSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetkitchenObjectSO());

        if (cuttingProgres >= cuttingRecepieSO.cuttingProgresMax)
        {
            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetkitchenObjectSO());
            KitchenObject.DestroyKitchenObject(GetKitchenObject());
            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
        }

    }
    public override void InteractAlternate(PlayerController player)
    {
        CutObjectServerRpc();
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecepieSO cuttingRecepieSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecepieSO != null)
        {
            return cuttingRecepieSO.output;
        }
        else
        {
            return null;
        }
    }
}
