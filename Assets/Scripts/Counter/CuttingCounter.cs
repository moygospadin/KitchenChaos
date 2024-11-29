using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress {
    [SerializeField] private CuttingReciepeSO[] cuttingReciepeSOArray;

    private int cuttingProgress;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler OnCut;


    public override void Interact(Player player) {

        if (!HastKitchenObject()) {

            if (player.HastKitchenObject()) {


                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {

                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                    CuttingReciepeSO cuttingReciepeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = (float)cuttingProgress / cuttingReciepeSO.cuttingProgressMax
                    });
                }


            }
            else {

                //Player not carrying anything
            }

        }
        else {
            if (player.HastKitchenObject()) {
                //Player is carrying anything
            }
            else {
                //Player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }


    }



    public override void InteractAlternative(Player player) {

        if (HastKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            KitchenObjectsSO inputKitchenObjectSo = GetKitchenObject().GetKitchenObjectSO();
            cuttingProgress++;


            OnCut?.Invoke(this, EventArgs.Empty);
            CuttingReciepeSO cuttingReciepeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSo);

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = (float)cuttingProgress / cuttingReciepeSO.cuttingProgressMax
            });

            if (cuttingProgress >= cuttingReciepeSO.cuttingProgressMax) {

                KitchenObjectsSO outputKitchenObjectSO = GetOutputForInput(inputKitchenObjectSo);

                GetKitchenObject().DestroySelf();



                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }


        }
    }


    private KitchenObjectsSO GetOutputForInput(KitchenObjectsSO inputKitchenObjectSo) {

        CuttingReciepeSO cuttingReciepeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSo);

        if (cuttingReciepeSO != null) {
            return cuttingReciepeSO.output;
        }
        return null;
    }

    private bool HasRecipeWithInput(KitchenObjectsSO inputKitchenObjectSo) {
        CuttingReciepeSO cuttingReciepeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSo);



        if (cuttingReciepeSO != null) {
            return true;
        }
        return false;
    }




    private CuttingReciepeSO GetCuttingRecipeSOWithInput(KitchenObjectsSO kitchenObjectSO) {
        foreach (CuttingReciepeSO cuttingReciepeSO in cuttingReciepeSOArray) {

            if (cuttingReciepeSO.input == kitchenObjectSO) {
                return cuttingReciepeSO;
            }
        }
        return null;

    }
}
