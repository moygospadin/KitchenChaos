using UnityEngine;

public class ClearCounter : BaseCounter {




    public override void Interact(Player player) {

        if (!HastKitchenObject()) {

            if (player.HastKitchenObject()) {

                player.GetKitchenObject().SetKitchenObjectParent(this);
            } else {

                //Player not carrying anything
            }

        } else {
            if (player.HastKitchenObject()) {
                //Player is carrying anything
            } else {
                //Player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }


    }





}
