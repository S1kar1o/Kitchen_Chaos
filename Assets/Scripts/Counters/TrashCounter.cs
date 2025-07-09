using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAneObjectTrash;
    public override void Interact(PlayerController player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            OnAneObjectTrash?.Invoke(this, EventArgs.Empty);
        }
    }
    new public static void ResetStaticData()
    {
        OnAneObjectTrash = null;
    }
}
