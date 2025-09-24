using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    private float spawnPlateTimer;
    private int plateSpawnedAmount;
    private int plateSpawnedAmountMax = 4;
    private float spawnPlateTimerMax = 4f;
    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;
            if (GameManager.Instance.IsGamePlaying() && plateSpawnedAmount < plateSpawnedAmountMax)
            {
                SpawnPlatesServerRpc();
            }
        }
    }
    [ServerRpc]
    private void SpawnPlatesServerRpc()
    {
        SpawnPlatesClientRpc();
    }
    [ClientRpc]
    private void SpawnPlatesClientRpc()
    {
        plateSpawnedAmount++;
        OnPlateSpawned?.Invoke(this, EventArgs.Empty);
    }
    public override void Interact(PlayerController player)
    {
        if (!player.HasKitchenObject())
        {
            if (plateSpawnedAmount > 0)
            {
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                InteractLogicServerRpc();
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }
    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        plateSpawnedAmount--;
        OnPlateRemoved?.Invoke(this, EventArgs.Empty);
    }
}
