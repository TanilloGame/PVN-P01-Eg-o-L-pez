using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Variables públicas ajustables desde el editor
    public float moveSpeed = 8f;
    public float jumpForce = 16f;
    public float dashSpeed = 15f;
    public float wallSlideSpeed = 2f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float dashDuration = 0.2f;
    public float coyoteTime = 0.2f;
    public int extraJumps = 1;

    // Variables privadas
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private bool isGrounded;
    private bool isWallSliding;
    private bool isDashing;
    private bool canDoubleJump;
    private bool isJumping;
    private int jumpCount;
    private float dashTimeLeft;
    private float lastGroundedTime;

    // Layers y Colliders
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public Transform groundCheck;
    public Transform wallCheck;
    public float groundCheckRadius = 0.2f;
    public float wallCheckDistance = 0.5f;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = extraJumps;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        GroundCheck();
        WallCheck();

        if (isDashing)
        {
            return; // Si está haciendo dash, no puede hacer otras acciones.
        }

        HandleMovement();
        HandleJump();
        HandleDash();
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        animator.SetBool("isJumping", !isGrounded && rb.velocity.y > 0);
        
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);


        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Mirando a la derecha
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Mirando a la izquierda
        }


        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
    }

    void HandleJump()
    {
        if (isGrounded)
        {
            jumpCount = extraJumps;
            canDoubleJump = true;
            isJumping = false;
            lastGroundedTime = Time.time;
        }

        // Salto principal
        if (Input.GetButtonDown("Jump") && (isGrounded || Time.time - lastGroundedTime <= coyoteTime))
        {
            Jump();
            isJumping = true;
        }
        // Doble salto
        else if (Input.GetButtonDown("Jump") && canDoubleJump && !isGrounded)
        {
            Jump();
            canDoubleJump = false;
        }

        // Wall Jump
        if (isWallSliding && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * moveSpeed, jumpForce);
            isWallSliding = false;
        }

        // Mejorar el control en el aire
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }

    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void WallCheck()
    {
        isWallSliding = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, wallLayer) && !isGrounded && rb.velocity.y < 0;
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    
}