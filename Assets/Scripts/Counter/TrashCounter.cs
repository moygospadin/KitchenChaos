using System;
using UnityEngine;

public class TrashCounter : BaseCounter {

    public static event EventHandler OnanyObjectTrashed;
    new public static void ResetStaticData() {
        OnanyObjectTrashed = null;

    }
    public override void Interact(Player player) {
        if (player.HastKitchenObject()) {

            player.GetKitchenObject().DestroySelf();
            OnanyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
