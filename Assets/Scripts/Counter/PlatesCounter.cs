using System;
using UnityEngine;

public class PlatesCounter : BaseCounter {


    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectsSO plateKitchenObjectSO;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSwapnedAmount;
    private int platesSwappedAmountMax = 4;
    private void Update() {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax) {
            spawnPlateTimer = 0f;

            if (platesSwapnedAmount < platesSwappedAmountMax) {
                platesSwapnedAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }





    public override void Interact(Player player) {
        if (!player.HastKitchenObject()) {
            //Player is empty hand
            if (platesSwapnedAmount > 0) {
                //There is at least one plate here

                platesSwapnedAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }

        }
    }
}
