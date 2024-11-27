using UnityEngine;

public class CuttingCounter : BaseCounter {
    [SerializeField] private CuttingReciepeSO[] cuttingReciepeSOArray;
    public override void Interact(Player player) {

        if (!HastKitchenObject()) {

            if (player.HastKitchenObject()) {


                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }

              
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



    public override void InteractAlternative(Player player) {

        if (HastKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {

            KitchenObjectsSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

            GetKitchenObject().DestroySelf();



            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);

        }
    }


    private KitchenObjectsSO GetOutputForInput(KitchenObjectsSO inputKitchenObjectSo) {
        foreach (CuttingReciepeSO cuttingReciepeSO in cuttingReciepeSOArray) {

            if (cuttingReciepeSO.input == inputKitchenObjectSo) {
                return cuttingReciepeSO.output;
            }
        }
        return null;
    }

    private bool HasRecipeWithInput(KitchenObjectsSO inputKitchenObjectSo) {
        foreach (CuttingReciepeSO cuttingReciepeSO in cuttingReciepeSOArray) {

            if (cuttingReciepeSO.input == inputKitchenObjectSo) {
                return true;
            }
        }
        return false;
    }
}
