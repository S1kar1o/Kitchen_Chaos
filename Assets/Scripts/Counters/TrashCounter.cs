using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAneObjectTrash;
    public override void Interact(PlayerController player)
    {
        if (player.HasKitchenObject())
        {
           KitchenObject.DestroyKitchenObject(player.GetKitchenObject());
            InteractLogicServerRpc();
        }
    }
    new public static void ResetStaticData()
    {
        OnAneObjectTrash = null;
    }
    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }
    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        OnAneObjectTrash?.Invoke(this, EventArgs.Empty);
    }
}
