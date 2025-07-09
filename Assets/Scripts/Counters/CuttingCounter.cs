using System;
using System.Collections;
using System.Collections.Generic;
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
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgres = 0;
                    CuttingRecepieSO cuttingRecepieSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetkitchenObjectSO());


                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgres / cuttingRecepieSO.cuttingProgresMax
                    });
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
                        GetKitchenObject().DestroySelf();
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
    public override void InteractAlternate(PlayerController player)
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
            if (cuttingProgres >= cuttingRecepieSO.cuttingProgresMax)
            {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetkitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }

        }
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
