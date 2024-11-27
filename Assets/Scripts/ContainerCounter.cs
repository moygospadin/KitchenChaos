using System;
using UnityEngine;

public class ContainerCounter : BaseCounter {


    public event EventHandler OnPlayerGrabObject;

    [SerializeField] private KitchenObjectsSO kitchenObjectSO;





    public override void Interact(Player player) {
        if (!player.HastKitchenObject()) {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

            OnPlayerGrabObject?.Invoke(this, EventArgs.Empty);

        }
    }




}
