using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private IkitchenObjectParent kitchenObjectParent;
    private FollowTransform followTransform;
    protected virtual void Awake()
    {
        followTransform = GetComponent<FollowTransform>();
    }
    public KitchenObjectSO GetkitchenObjectSO()
    {
        return kitchenObjectSO;
    }
    public void SetKitchenObjectParent(IkitchenObjectParent kitchenObjectParent)
    {
        SetKitchenObjectParentServerRpc(kitchenObjectParent.GetNetworkObject());
    }
    [ServerRpc (RequireOwnership =false)]
    private void SetKitchenObjectParentServerRpc(NetworkObjectReference kictchenObjectReference)
    {
        SetKitchenObjectParentClientRpc(kictchenObjectReference);
    }
    [ClientRpc]
    private void SetKitchenObjectParentClientRpc(NetworkObjectReference kictchenObjectReference)
    {
        kictchenObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IkitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IkitchenObjectParent>();

        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.CleanKitchenObject();
        }
        this.kitchenObjectParent = kitchenObjectParent;
        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.Log("error");
        }
        kitchenObjectParent.SetKitchenObject(this);
        followTransform.SetTransform(kitchenObjectParent.GetKitchenObjectFollowTransforms());
    }

    public IkitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }
    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if(this is PlateKitchenObject)
        {
            plateKitchenObject=this as PlateKitchenObject;
            return true;
        }
        plateKitchenObject=null;
        return false;

    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    public void CleanKitchenObjectOnParent()
    {
        kitchenObjectParent.CleanKitchenObject();

    }
    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO,IkitchenObjectParent ikitchenObjectParent)
    {
        KitchenGameMultiplayer.Instance.SpawnKitchenObject( kitchenObjectSO,  ikitchenObjectParent);
    }
    public static void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        KitchenGameMultiplayer.Instance.DestroyKitchenObject(kitchenObject);

    }
}
