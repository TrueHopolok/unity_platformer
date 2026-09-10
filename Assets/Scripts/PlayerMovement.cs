using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    InputControls controls;
    InputAction jumpAction;
    InputAction moveAction;
    Rigidbody2D body;
    bool isGrounded;
    bool jumpJustPressed;
    bool jumpJustReleased;
    [SerializeField] float jumpForce = 15f;
    [SerializeField] float gravityForce = 25f;
    [SerializeField] float horizontalVelocity = 10f;

    void Awake()
    {
        controls = new InputControls();
        // can be skipped, since OnEnable() is also called right after Awake()
        // but calling it twice won't do any harm
        // controls.Enable();
        jumpAction = controls.Player.Jump;
        moveAction = controls.Player.Move;
        body = GetComponent<Rigidbody2D>();
        // disable natural gravity, since we will control it manually
        body.gravityScale = 0f;
    }

    void Update()
    {
        jumpJustPressed = jumpJustPressed || jumpAction.WasPressedThisFrame();
        jumpJustReleased = jumpJustReleased || jumpAction.WasReleasedThisFrame();
    }

    void FixedUpdate()
    {
        Debug.Log(body.linearVelocityY);
        HandleJumpCollissions();
        HandleVerticalMovement();
        HandleHorizontalMovement();
    }

    // Can be extended to be used for wall jumping
    void HandleJumpCollissions()
    {
        isGrounded = false; // todo
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
            // todo: move jump logic here
        }
        else
        {
            if (body.linearVelocityY > 0f && jumpJustReleased)
            {
                body.linearVelocityY *= 0.5f;
            }
            body.linearVelocityY -= gravityForce * Time.fixedDeltaTime;
            if (body.linearVelocityY < 0f)
            {
                body.linearVelocityY -= gravityForce * Time.fixedDeltaTime;
            }
        }

        jumpJustPressed = false;
        jumpJustReleased = false;
    }

    void HandleHorizontalMovement()
    {
        float direction = moveAction.ReadValue<Vector2>().x;
        body.linearVelocityX = direction * horizontalVelocity;
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
