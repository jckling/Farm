using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;

    private float inputX;
    private float inputY;
    private Vector2 movementInput;

    #region Event Functions

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        Movement();
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

        movementInput = new Vector2(inputX, inputY);
    }

    private void Movement()
    {
        rb.MovePosition(rb.position + movementInput * (speed * Time.deltaTime));
    }
}