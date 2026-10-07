using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 7f;

    [Header("Jump")]
    [SerializeField] float jumpVelocity = 13f;
    [SerializeField] float coyoteTime = 0.12f;
    [SerializeField] float jumpBufferTime = 0.12f;
    [SerializeField] float fallGravityMultiplier = 1.8f;
    [SerializeField] float lowJumpGravityMultiplier = 2f;

    [Header("Ground check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.15f;
    [SerializeField] LayerMask groundLayer;

    Rigidbody2D rb;
    PlayerControls controls;
    Vector2 moveInput;
    float coyoteCounter;
    float jumpBufferCounter;
    bool jumpHeld;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;
        controls.Player.Jump.performed += OnJumpPressed;
        controls.Player.Jump.canceled += OnJumpReleased;
    }

    void OnDisable()
    {
        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnMove;
        controls.Player.Jump.performed -= OnJumpPressed;
        controls.Player.Jump.canceled -= OnJumpReleased;
        controls.Player.Disable();
    }

    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void OnJumpPressed(InputAction.CallbackContext context)
    {
        jumpBufferCounter = jumpBufferTime;
        jumpHeld = true;
    }

    void OnJumpReleased(InputAction.CallbackContext context)
    {
        jumpHeld = false;

        // Releasing jump while rising cuts the jump short.
        if (rb.linearVelocity.y > 0f)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
    }

    void Update()
    {
        bool grounded = Physics2D.OverlapCircle(
            groundCheck.position, groundCheckRadius, groundLayer);

        coyoteCounter = grounded ? coyoteTime : coyoteCounter - Time.deltaTime;
        jumpBufferCounter -= Time.deltaTime;

        if (jumpBufferCounter > 0f && coyoteCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
            jumpBufferCounter = 0f;
            coyoteCounter = 0f;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        if (rb.linearVelocity.y < 0f)
        {
            rb.gravityScale = fallGravityMultiplier;
        }
        else if (rb.linearVelocity.y > 0f && !jumpHeld)
        {
            rb.gravityScale = lowJumpGravityMultiplier;
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
