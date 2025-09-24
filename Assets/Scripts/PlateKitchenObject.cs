using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedArgs> OnIngredientAdded;
    public class OnIngredientAddedArgs : EventArgs
    {
        public KitchenObjectSO KitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    private List<KitchenObjectSO> kitchenObjectSOList;

    public bool TryAddIngredient(KitchenObjectSO ingredient)
    {
        if (!validKitchenObjectSOList.Contains(ingredient))
        {
            return false;
        }
        if (kitchenObjectSOList.Contains(ingredient))
        {
            return false;
        }
        TryAddIngredientServerRpc(
            KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(ingredient)
            );
        return true;
    }
    [ServerRpc (RequireOwnership = false)]
    private void TryAddIngredientServerRpc(int kitchenObjectSOIndex)
    {
        TryAddIngredientClientRpc(kitchenObjectSOIndex);
    }
    [ClientRpc]
    private void TryAddIngredientClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectByIndex(kitchenObjectSOIndex);
        kitchenObjectSOList.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedArgs
        {
            KitchenObjectSO = kitchenObjectSO
        });
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
    protected override void Awake()
    {
        base.Awake();
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }
}
