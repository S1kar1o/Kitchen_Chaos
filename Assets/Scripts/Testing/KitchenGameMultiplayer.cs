using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenGameMultiplayer : NetworkBehaviour
{
    [SerializeField] KitchenObjectListSO kitchenObjectListSO;
    public static KitchenGameMultiplayer Instance {  get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public  void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IkitchenObjectParent ikitchenObjectParent)
    {
        SpawnKitchenObjectServerRpc(GetKitchenObjectSOIndex(kitchenObjectSO), ikitchenObjectParent.GetNetworkObject());
    }
    [ServerRpc(RequireOwnership = false)]
    private void SpawnKitchenObjectServerRpc(int kitchenObjectSOIndex, NetworkObjectReference KitchenObjectParentNetworkReference)
    {
        KitchenObjectSO kitchenObjectSO = GetKitchenObjectByIndex(kitchenObjectSOIndex);

        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        NetworkObject networkObj= kitchenObjectTransform.GetComponent<NetworkObject>();
        networkObj.Spawn(true);

        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        KitchenObjectParentNetworkReference.TryGet(out NetworkObject KitchenObjectParentNetworkObject);
        IkitchenObjectParent ikitchenObjectParent= KitchenObjectParentNetworkObject.GetComponent<IkitchenObjectParent>();
        kitchenObject.SetKitchenObjectParent(ikitchenObjectParent);
    }

    public int GetKitchenObjectSOIndex(KitchenObjectSO kitchenObject)
    {
        return kitchenObjectListSO.kitchenObjectSOList.IndexOf(kitchenObject);
    }
    public KitchenObjectSO GetKitchenObjectByIndex(int kitchenObjectSOIndex)
    {
        return kitchenObjectListSO.kitchenObjectSOList[kitchenObjectSOIndex];
    }
    public void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        DestroyKitchenObjectServerRpc(kitchenObject.NetworkObject);
    }
    [ServerRpc (RequireOwnership =false)]
    private void DestroyKitchenObjectServerRpc(NetworkObjectReference kitchenObjectNetworkObjectReference)
    {
        kitchenObjectNetworkObjectReference.TryGet(out NetworkObject kitchenObjectNetworkObject);
        KitchenObject kitchenObject= kitchenObjectNetworkObject.GetComponent<KitchenObject>();
        ClerKitchenObjectOnParentClientRpc(kitchenObjectNetworkObjectReference);
        kitchenObject.DestroySelf();
    }
    [ClientRpc]
    private void ClerKitchenObjectOnParentClientRpc(NetworkObjectReference kitchenObjectNetworkObjectReference)
    {
        kitchenObjectNetworkObjectReference.TryGet(out NetworkObject kitchenObjectNetworkObject);
        KitchenObject kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObject>();
        kitchenObject.CleanKitchenObjectOnParent();
    }
}
