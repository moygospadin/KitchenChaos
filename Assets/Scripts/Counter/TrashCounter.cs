using System;
using UnityEngine;

public class TrashCounter : BaseCounter {

    public static event EventHandler OnanyObjectTrashed;

    public override void Interact(Player player) {
        if (player.HastKitchenObject()) {

            player.GetKitchenObject().DestroySelf();
            OnanyObjectTrashed?.Invoke(this,EventArgs.Empty);   
        }
    }
}
