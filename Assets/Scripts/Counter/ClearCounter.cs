using UnityEngine;

public class ClearCounter : BaseCounter {




    public override void Interact(Player player) {

        if (!HastKitchenObject()) {

            if (player.HastKitchenObject()) {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else {
                //Player not carrying anything
            }
        }
        else {
            if (player.HastKitchenObject()) {
                //Player is carrying something

                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    //Player is holding a plate

                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {

                        GetKitchenObject().DestroySelf();
                    }

                }
                else {

                    //Player is not carrying Plate 
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
                        //Player is holding a plate

                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) {

                            player.GetKitchenObject().DestroySelf();
                        }

                    }
                }

            }
            else {
                //Player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
