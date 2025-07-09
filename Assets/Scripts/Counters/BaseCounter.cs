using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IkitchenObjectParent
{
    [SerializeField] private Transform counterTopPooint;
    private KitchenObject kitchenObject;

    public static event EventHandler OnPlaceSomethingHere;
    public static void ResetStaticData()
    {
        OnPlaceSomethingHere = null;
    }
    public virtual void Interact(PlayerController player)
    {

    }
    public virtual void InteractAlternate(PlayerController player)
    {

    }
    public Transform GetKitchenObjectFollowTransforms()
    {
        return counterTopPooint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
            OnPlaceSomethingHere?.Invoke(this, EventArgs.Empty);
    }
    public KitchenObject GetKitchenObject() { return kitchenObject; }
    public void CleanKitchenObject()
    {
        kitchenObject = null;
    }
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
