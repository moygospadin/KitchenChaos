using System;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent {


    [SerializeField] private Transform counterTopPoint;





    private KitchenObject kitchenObject;

    public virtual void Interact(Player player) {
        Debug.LogError("Base counter.Interact()");
    }
    public virtual void InteractAlternative(Player player) {
        Debug.LogError("Base counter.InteractAlternative()");
    }

    
    public Transform GetKitchenObjectFollowTransform() {

        return counterTopPoint;
    }



    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
    }


    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    public void ClearKitchenObject() {
        this.kitchenObject = null;
    }


    public bool HastKitchenObject() {
        return (kitchenObject != null);
    }
}
