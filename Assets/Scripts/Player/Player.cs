using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;

    private float inputX;
    private float inputY;
    private Vector2 movementInput;
    private Animator[] animators;
    private bool isMoving;
    private bool inputDisable;

    #region Event Functions

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
    }

    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.MoveToPosition += OnMoveToPosition;
        EventHandler.MouseClickedEvent += OnMouseClickedEvent;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.MoveToPosition -= OnMoveToPosition;
        EventHandler.MouseClickedEvent -= OnMouseClickedEvent;
    }

    private void Update()
    {
        if (inputDisable == false)
        {
            PlayerInput();
        }
        else
        {
            isMoving = false;
        }

        SwitchAnimation();
    }

    private void FixedUpdate()
    {
        if (inputDisable == false)
        {
            Movement();
        }
    }

    #endregion

    private void PlayerInput()
    {
        // if (inputY == 0)
        inputX = Input.GetAxisRaw("Horizontal");
        // if (inputX == 0)
        inputY = Input.GetAxisRaw("Vertical");

        if (inputX != 0 && inputY != 0)
        {
            inputX *= 0.6f;
            inputY *= 0.6f;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            inputX *= 0.5f;
            inputY *= 0.5f;
        }

        movementInput = new Vector2(inputX, inputY);
        isMoving = movementInput != Vector2.zero;
    }

    private void Movement()
    {
        rb.MovePosition(rb.position + movementInput * (speed * Time.deltaTime));
    }

    private void SwitchAnimation()
    {
        foreach (var anim in animators)
        {
            anim.SetBool("IsMoving", isMoving);

            if (isMoving)
            {
                anim.SetFloat("InputX", inputX);
                anim.SetFloat("InputY", inputY);
            }
        }
    }

    #region EventHandler Functions

    private void OnBeforeSceneUnloadEvent()
    {
        inputDisable = true;
    }

    private void OnAfterSceneLoadedEvent()
    {
        inputDisable = false;
    }

    private void OnMoveToPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }

    private void OnMouseClickedEvent(Vector3 pos, ItemDetails itemDetails)
    {
        // TODO: Player animation
        EventHandler.CallExecuteAfterAnimationEvent(pos, itemDetails);
    }

    #endregion
}