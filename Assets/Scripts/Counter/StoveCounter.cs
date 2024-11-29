using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress {

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public class OnStateChangedEventArgs : EventArgs {
        public State state;

    }
    public enum State {
        Idle,
        Frying,
        Fried,
        Burned
    }


    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private FryingRecipeSO fryingRecipeSO;
    private float fryingTimer;
    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;

    private void Start() {
        state = State.Idle;
    }

    private void Update() {

        if (HastKitchenObject()) {

            switch (state) {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax,
                    });
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax) {

                        //Fried
                        fryingTimer = 0f;
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);


                        state = State.Fried;
                        burningTimer = 0f;

                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;
                    OnProgressChanged.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax,
                    });
                    if (burningTimer > burningRecipeSO.burningTimerMax) {

                        // Burned

                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        state = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                        OnProgressChanged.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0f,
                        });
                    }
                    break;
                case State.Burned:
                    break;
            }

        }
    }

    public override void Interact(Player player) {

        if (!HastKitchenObject()) {

            if (player.HastKitchenObject()) {


                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {

                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    state = State.Frying;
                    fryingTimer = 0f;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    OnProgressChanged.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax,
                    });
                }
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

                        state = State.Idle;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                        OnProgressChanged.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0f,
                        });
                    }
                }

            }
            else {
                //Player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                OnProgressChanged.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = 0f,
                });
            }
        }
    }



    private KitchenObjectsSO GetOutputForInput(KitchenObjectsSO inputKitchenObjectSo) {

        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSo);

        if (fryingRecipeSO != null) {
            return fryingRecipeSO.output;
        }
        return null;
    }

    private bool HasRecipeWithInput(KitchenObjectsSO inputKitchenObjectSo) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSo);
        if (fryingRecipeSO != null) {
            return true;
        }
        return false;
    }




    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectsSO kitchenObjectSO) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {

            if (fryingRecipeSO.input == kitchenObjectSO) {
                return fryingRecipeSO;
            }
        }
        return null;

    }



    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectsSO kitchenObjectSO) {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray) {

            if (burningRecipeSO.input == kitchenObjectSO) {
                return burningRecipeSO;
            }
        }
        return null;

    }
}
