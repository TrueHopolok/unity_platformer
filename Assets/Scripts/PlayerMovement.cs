using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    InputControls controls;
    InputAction jumpAction;
    InputAction moveAction;
    Rigidbody2D body;
    BoxCollider2D hitbox;
    bool isGrounded;
    bool jumpJustPressed;
    bool jumpJustReleased;
    [SerializeField] float jumpForce = 8f;
    [SerializeField] float gravityForce = 5f;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] Vector2 groundCheckSize = new Vector2(0.8f, 0.1f);
    [SerializeField] LayerMask groundLayer;

    void Awake()
    {
        controls = new InputControls();
        controls.Enable();
        jumpAction = controls.Player.Jump;
        moveAction = controls.Player.Move;

        body = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        jumpJustPressed = jumpJustPressed || jumpAction.WasPressedThisFrame();
        jumpJustReleased = jumpJustReleased || jumpAction.WasReleasedThisFrame();
    }

    void FixedUpdate()
    {
        HandleJumpCollissions();
        HandleVerticalMovement();
        HandleHorizontalMovement();
    }

    // Can be extended to be used for wall jumping
    void HandleJumpCollissions()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
        // isGrounded = false; // todo
    }

    void HandleVerticalMovement()
    {
        if (jumpJustPressed)
        {
            body.linearVelocityY = jumpForce;
        }
        if (isGrounded)
        {
            body.linearVelocityY = 0f;
            // move jump logic here
        }
        else
        {
            if (body.linearVelocityY > 0f && jumpJustReleased)
            {
                body.linearVelocityY *= 0.5f;
            }
            body.linearVelocityY -= gravityForce * Time.fixedDeltaTime;
        }
        jumpJustPressed = false;
        jumpJustReleased = false;
    }

    void HandleHorizontalMovement()
    {
        // todo
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void OnDestroy()
    {
        controls.Dispose();
    }
}
