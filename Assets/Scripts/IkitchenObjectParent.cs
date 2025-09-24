using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IkitchenObjectParent 
{
    public Transform GetKitchenObjectFollowTransforms();
    public void SetKitchenObject(KitchenObject kitchenObject);
    public KitchenObject GetKitchenObject() ;
    public void CleanKitchenObject();
    public bool HasKitchenObject();
    public NetworkObject GetNetworkObject();
}
