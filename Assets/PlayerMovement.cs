using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    public float highJumpForceMultiplier = 1.5f;
    public float groundCheckDistance = 0.1f;
    public float coyoteTime = 0.1f;
    public LayerMask groundLayer;
    private bool isGrounded = false;
    private bool isJumping = false;
    private float coyoteTimer = 0f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Horizontal movement
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // Jumping
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || coyoteTimer > 0f)
            {
                isJumping = true;
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                coyoteTimer = 0f;
            }
        }
        else if (Input.GetButtonUp("Jump") && isJumping)
        {
            isJumping = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        // Ground check
        isGrounded = Physics2D.BoxCast(transform.position, new Vector2(0.9f, 0.1f), 0f, Vector2.down, groundCheckDistance);

        // Coyote time
        if (isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;

        // Apply additional force for high jump if the jump button is held
        if (isJumping && rb.velocity.y > 0f)
        {
            rb.AddForce(new Vector2(0f, jumpForce * highJumpForceMultiplier * Time.deltaTime));
        }
    }
}
