using System;
using System.Linq;
using events;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static event events.EventLocation ObjectFound; 
    public static event events.Event ObjectLost;
    public static event events.EventClueArray InventoryButtonPressed;
    
    public float moveSpeed = 5f;
    public bool canMove = true;
    public float pickDistance;

    private Vector2 _lastDirection;
    private RaycastHit2D? _lastPickableObject;
    private RaycastHit2D? _lastPnj;
    private Rigidbody2D rigidbody;
    private Animator animator;
    private Vector2 movement;
    private DialogClueManager _dialogClueManager;
    private ClueInventoryManager _clueInventoryManager;
    
    private readonly KeyCode inventoryKey = KeyCode.C;
    private readonly KeyCode actionKey = KeyCode.Return;

    // Start is called before the first frame update
    void Start()
    {
        ClueManager.StartDialog += startDialog;
        _dialogClueManager = GetComponent<DialogClueManager>();
        _clueInventoryManager = GetComponent<ClueInventoryManager>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void startDialog(string[] sentences, Action endcallback)
    {
        if (!_dialogClueManager.dialogIsRunning)
        {
            _dialogClueManager.LaunchDialog(endcallback, sentences);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(actionKey) && _dialogClueManager.dialogIsRunning)
        {
            _dialogClueManager.NextLine();
        }

        var result = Physics2D.RaycastAll(transform.position, _lastDirection, pickDistance).ToList();
        RaycastHit2D? pickableBuffer = null;
        RaycastHit2D? pnjInteractableBuffer = null;

        if (result.Count > 1)
        {
            var firstResult = result.GetRange(0, result.Count - 1).First();
            if (firstResult.collider.CompareTag("Interactable"))
            {
                pickableBuffer = firstResult;
            }

            if (firstResult.collider.CompareTag("InteractablePnj"))
            {
                pnjInteractableBuffer = firstResult;
            }
        }

        if (!pickableBuffer.HasValue && _lastPickableObject.HasValue)
        {
            ObjectLost?.Invoke();
        }
        if (pickableBuffer.HasValue && !_lastPickableObject.HasValue)
        {
            ObjectFound?.Invoke(pickableBuffer.Value.collider.transform.position);
        }
        _lastPickableObject = pickableBuffer;
        _lastPnj = pnjInteractableBuffer;
        

        if (!_dialogClueManager.dialogIsRunning)
        {
            if (Input.GetKeyDown(actionKey) && _lastPickableObject.HasValue)
            {
                _lastPickableObject.Value.collider.gameObject.SendMessage("Interact");
            }

            if (Input.GetKeyDown(actionKey) && _lastPnj.HasValue)
            {
                _lastPnj.Value.collider.gameObject.SendMessage("Interact");
            } 
        
            if (Input.GetKeyDown(inventoryKey))
            {
                InventoryButtonPressed?.Invoke(_clueInventoryManager.clues.ToArray());
                switchCanMove();
            }    
        }
        
        if(canMove)
        {
            // Inputs
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            animator.SetFloat("Speed", movement.magnitude);
            if (Vector2.zero != movement)
            {
                _lastDirection = movement;
                animator.SetFloat("moveX", movement.x);
                animator.SetFloat("moveY", movement.y);    
            }
        }
    }

    private void switchCanMove()
    {
        setCanMove(!getcanMove());
    }

    private void setCanMove(bool newCanMove)
    {
        canMove = newCanMove;
        if (!newCanMove)
        {
            movement = Vector2.zero;
            animator.SetFloat("Speed", movement.magnitude);
        }
    }

    private bool getcanMove()
    {
        return canMove;
    }

    private void FixedUpdate() {
        // Movements
        rigidbody.MovePosition(rigidbody.position + (movement.normalized) * moveSpeed * Time.fixedDeltaTime);
    }

    void setMove(bool active) => canMove = active;
}
