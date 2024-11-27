using System;
using UnityEngine;

public class Player : MonoBehaviour,IKitchenObjectParent {



    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {

        public BaseCounter selectedCounter;
    }


    [SerializeField] private float moveSpeed = 7f;

    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;


    private bool isWalking = false;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;


    private void Awake() {
        if (Instance != null) {
            Debug.LogError("SOMETHING GO WRONG, not single user");
        }
        Instance = this;
    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternativeAction += GameInput_OnInteractAlternativeAction;
    }

    private void GameInput_OnInteractAlternativeAction(object sender, EventArgs e) {
        if (selectedCounter != null) {
            selectedCounter.InteractAlternative(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    }

    private void Update() {

        HandleMovement();
        HandleInteractions();
    }


    public bool IsWalking() {
        return isWalking;
    }


    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float interactDistance = 2f;


        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }

        bool isHitting = Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask);

        if (isHitting) {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {


                if (baseCounter != selectedCounter) {
                    SetSelectedCounter(baseCounter);
                }

            } else {
                SetSelectedCounter(null);
            }
        } else {
            SetSelectedCounter(null);
        }

    }




    private void SetSelectedCounter(BaseCounter selectedCounter) {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = selectedCounter
        });
    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = Time.deltaTime * moveSpeed;
        float playerHeight = 2f;
        float playerRadius = .5f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);



        if (!canMove) {

            //Cannot move towards moveDir

            //Atemp only x movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove) {
                //Can move only on the x
                moveDir = moveDirX;
            } else {
                //Cannot move X

                //Atemp onlyz movement
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove) {
                    //Can move only on the z
                    moveDir = moveDirZ;

                }


            }
        }



        if (canMove) {
            transform.position += moveDir * moveDistance;
        }


        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }




    public Transform GetKitchenObjectFollowTransform() {

        return kitchenObjectHoldPoint;
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
