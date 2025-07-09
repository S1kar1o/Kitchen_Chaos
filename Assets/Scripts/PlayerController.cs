using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PlayerController : MonoBehaviour, IkitchenObjectParent
{
    public event EventHandler OnPickedSomething;
    public static PlayerController Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;

    }
    private float moveSpeed = 7f;
    private float rotatingSpeed = 10;
    private bool isWalking = false;
    [SerializeField] InputSystem inputSystem;
    [SerializeField] LayerMask countersLayerMask;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private void Start()
    {
        inputSystem.OnInteractAction += InputSystem_OnInteractAction;
        inputSystem.OnInteractAlternateAction += InputSystem_OnInteractAlternateAction; ;
    }

    private void InputSystem_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying())  return;
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    public void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("more than 1 player instance");
        }
        Instance = this;
    }
    private void InputSystem_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    void Update()
    {
        HandleInteraction();
        HandleMovement();
    }
    private void HandleInteraction()
    {
        Vector2 currentPosition = inputSystem.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(currentPosition.x, 0, currentPosition.y);
        float interactDistance = 2f;

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raynCastHit, interactDistance, countersLayerMask))
        {
            if (raynCastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);

                }
            }
            else
            {
                SetSelectedCounter(null);

            }
        }
        else
        {
            SetSelectedCounter(null);

        }

    }
    private void HandleMovement()
    {


        Vector2 currentPosition = inputSystem.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(currentPosition.x, 0, currentPosition.y);
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = .2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x<-.5f||moveDir.x>+.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }
            }

        }
        if (canMove)
        {
            isWalking = moveDir != Vector3.zero;

            transform.position += moveDir * moveDistance;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotatingSpeed);
        }
        else
        {
            isWalking = false;
        }

    }
    public bool IsWalking()
    {
        return isWalking;
    }
    private void SetSelectedCounter(BaseCounter selecterCounter)
    {
        this.selectedCounter = selecterCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });
    }

    public Transform GetKitchenObjectFollowTransforms()
    {
        return kitchenObjectHoldPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
    }
    public KitchenObject GetKitchenObject() { return kitchenObject; }
    public void CleanKitchenObject()
    {
        kitchenObject = null;
    }
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
