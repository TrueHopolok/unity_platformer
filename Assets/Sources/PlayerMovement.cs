using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float jumpBufferLength = 0.1f;
    [SerializeField] float gravityForce = 1f;
    [SerializeField] float jumpForce = 2f;
    CharacterController controller;
    InputControls controls;
    InputAction jumpAction;
    InputAction moveAction;
    float jumpBufferRemaining = 0f;
    Vector3 velocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        controls = new InputControls();
        jumpAction = controls.Player.Jump;
        moveAction = controls.Player.Move;
        velocity = new Vector3();
    }

    void Update()
    {
        if (jumpAction.WasPressedThisFrame())
        {
            jumpBufferRemaining = jumpBufferLength;
        }
        else
        {
            jumpBufferRemaining = Math.Clamp(jumpBufferRemaining - Time.deltaTime, 0, jumpBufferLength);
        }
    }

    void FixedUpdate()
    {
        HandleVerticalMovement();
        HandleHorizontalMovement();
        controller.Move(velocity);
    }

    void HandleVerticalMovement()
    {
        if (controller.isGrounded)
        {
            velocity.y = 0f;
            if (jumpBufferRemaining > 0f)
            {
                jumpBufferRemaining = 0f;

                // JUMP
                velocity.y = jumpForce;
            }
        }
        else
        {
            // FALL
            velocity.y -= gravityForce * Time.fixedDeltaTime;
        }
    }

    void HandleHorizontalMovement()
    {
        // todo
    }
}
