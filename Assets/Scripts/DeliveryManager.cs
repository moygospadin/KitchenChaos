using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {


    public static DeliveryManager Instance {
        get; private set;
    }



    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;

    public event EventHandler OnRecipeSpawn;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;


    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;
    private int successfulRecipesAmount;
    private void Awake() {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update() {
        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer <= 0f) {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeMax > waitingRecipeSOList.Count) {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];

                waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawn?.Invoke(this, EventArgs.Empty);
            }

        }
    }


    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {



        for (int i = 0; i < waitingRecipeSOList.Count; i++) {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];



            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectsSOList().Count) {
                // Has the same number of ingredients
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectsSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList) {
                    //Cycling through all ingredients in recipe
                    bool ingredientFound = false;

                    foreach (KitchenObjectsSO planeKitchenObjectSO in plateKitchenObject.GetKitchenObjectsSOList()) {
                        //Cycling through all ingredients in the plane

                        if (planeKitchenObjectSO == recipeKitchenObjectSO) {
                            //Ingredient matches!
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound) {
                        // This recipe ingridient was not found on the Plate
                        plateContentsMatchesRecipe = false;
                    }
                }
                if (plateContentsMatchesRecipe) {
                    //Player delivered the correct recipe!

                    successfulRecipesAmount++;
                    waitingRecipeSOList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        //No matches found
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);

    }


    public List<RecipeSO> GetWaitingRecipeSOList() {

        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipesAmount() {

        return successfulRecipesAmount;
    }
}
