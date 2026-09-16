using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float checkSize = 0.02f;
    [SerializeField] int raycastAmount = 5;
    [SerializeField] bool useRaycastVariant = true;
    [SerializeField] bool drawDebugRays = true;
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
        if (useRaycastVariant)
        {
            UpdateJumpStates_RaycastVariant();
        }
        else
        {
            UpdateJumpStates_ColliderVariant();
        }
        HandleVerticalMovement();
        HandleHorizontalMovement();
    }

    // Can be extended to be used for wall jumping
    void UpdateJumpStates_ColliderVariant()
    {
        if (!Mathf.Approximately(body.linearVelocityY, 0f))
        {
            isGrounded = false;
            return;
        }

        Vector2 hitboxPosition = transform.position;
        hitboxPosition += hitbox.offset;
        Vector2 hitboxSize = hitbox.size;

        // 0.01f needed to avoid border collission with player hitbox
        hitboxPosition.y -= hitboxSize.y * (0.5f + checkSize / 2) + 0.01f;
        hitboxSize.y = hitboxSize.y * checkSize;
        isGrounded = Physics2D.OverlapBox(hitboxPosition, hitboxSize, 0f) != null;

        if (drawDebugRays)
        {
            hitboxPosition.y += checkSize / 2;
            hitboxPosition.x -= hitboxSize.x / 2;
            Debug.DrawRay(hitboxPosition, Vector2.down, Color.red);
            hitboxPosition.x += hitboxSize.x;
            Debug.DrawRay(hitboxPosition, Vector2.down, Color.red);
        }
    }

    void UpdateJumpStates_RaycastVariant()
    {
        if (!Mathf.Approximately(body.linearVelocityY, 0f))
        {
            isGrounded = false;
            return;
        }

        Vector2 hitboxPosition = transform.position;
        hitboxPosition += hitbox.offset;
        Vector2 hitboxSize = hitbox.size;

        // 0.01f needed to avoid border collission with player hitbox
        hitboxPosition.y -= hitboxSize.y * 0.5f + 0.01f;
        hitboxPosition.x -= hitboxSize.x / 2;

        float delta = hitboxSize.x / (raycastAmount - 1);
        for (int i = 0; i < raycastAmount; i++)
        {
            isGrounded = Physics2D.Raycast(hitboxPosition, Vector2.down, checkSize);
            if (drawDebugRays)
            {
                Debug.DrawRay(hitboxPosition, Vector2.down, Color.red);
            }
            if (isGrounded) break;
            hitboxPosition.x += delta;
        }
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

            // apply extra gravity when falling for better game feel
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
