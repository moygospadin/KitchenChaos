using UnityEngine;

public class KitchenObject : MonoBehaviour {
    [SerializeField] private KitchenObjectsSO KitchenObjectsSO;





    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectsSO GetKitchenObjectSO() {
        return KitchenObjectsSO;
    }



    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent) {

        if (this.kitchenObjectParent != null) {

            this.kitchenObjectParent.ClearKitchenObject();
        }




        this.kitchenObjectParent = kitchenObjectParent;
        if (kitchenObjectParent.HastKitchenObject()) {

            Debug.LogError("kitchenObjectParent already has a KitchenObject");
        }
        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }


    public IKitchenObjectParent GetKitchenObjectParent() {
        return kitchenObjectParent;
    }


    public void DestroySelf() {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }


    public static KitchenObject SpawnKitchenObject(
        KitchenObjectsSO kitchenObjectsSO,
        IKitchenObjectParent kitchenObjectParent
        ) {

        Transform kithechObjectTransoform = Instantiate(kitchenObjectsSO.prefab);
        KitchenObject kitchenObject = kithechObjectTransoform.GetComponent<KitchenObject>();

        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
    }
}
