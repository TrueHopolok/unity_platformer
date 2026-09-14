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
        hitbox = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        jumpJustPressed = jumpJustPressed || jumpAction.WasPressedThisFrame();
        jumpJustReleased = jumpJustReleased || jumpAction.WasReleasedThisFrame();
    }

    void FixedUpdate()
    {
        UpdateJumpStates_ColliderVariant();
        // UpdateJumpStates_RaycastVariant();
        HandleVerticalMovement();
        HandleHorizontalMovement();
    }

    // Can be extended to be used for wall jumping
    void UpdateJumpStates_ColliderVariant()
    {
        const float checkSize = 0.02f;

        Vector2 hitboxPosition = transform.position;
        hitboxPosition += hitbox.offset;
        Vector2 hitboxSize = hitbox.size;

        // 0.01f needed to avoid border collission with player hitbox
        hitboxPosition.y -= hitboxSize.y * (0.5f + checkSize / 2) + 0.01f;
        hitboxSize.y = hitboxSize.y * checkSize;
        isGrounded = Mathf.Approximately(body.linearVelocityY, 0f) && Physics2D.OverlapBox(hitboxPosition, hitboxSize, 0f) != null;
    }

    void UpdateJumpStates_RaycastVariant()
    {
        // todo
    }

    void HandleVerticalMovement()
    {
        if (isGrounded)
        {
            body.linearVelocityY = 0f;
            if (jumpJustPressed)
            {
                body.linearVelocityY = jumpForce;
            }
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
