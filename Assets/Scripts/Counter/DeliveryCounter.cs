using UnityEngine;

public class DeliveryCounter : BaseCounter {

    public override void Interact(Player player) {



        if (player.HastKitchenObject()) {

            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                //Only accept Plates
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                player.GetKitchenObject().DestroySelf();
            }
        }

    }
}
