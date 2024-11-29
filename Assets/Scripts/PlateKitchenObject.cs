
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredietnAdded;
    public class OnIngredientAddedEventArgs : EventArgs {
        public KitchenObjectsSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectsSO> validKitchenObjectsList;
    private List<KitchenObjectsSO> kitchenObjectSOList;


    private void Awake() {
        kitchenObjectSOList = new List<KitchenObjectsSO>();
    }
    public bool TryAddIngredient(KitchenObjectsSO kitchenObjectsSO) {
        if (!validKitchenObjectsList.Contains(kitchenObjectsSO)) {
            //Not a valid ingridient
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectsSO)) {
            return false;
        }
        else {
            kitchenObjectSOList.Add(kitchenObjectsSO);
            OnIngredietnAdded?.Invoke(this, new OnIngredientAddedEventArgs {
                kitchenObjectSO = kitchenObjectsSO
            });


            return true;
        }

    }



    public List<KitchenObjectsSO> GetKitchenObjectsSOList() {


        return kitchenObjectSOList;
    }
}
