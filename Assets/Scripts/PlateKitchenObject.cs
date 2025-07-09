using System;
using System.Collections;
using System.Collections.Generic;
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
        kitchenObjectSOList.Add(ingredient);
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedArgs
        {
            KitchenObjectSO = ingredient
        }) ;
        return true;
    }
    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }
}
