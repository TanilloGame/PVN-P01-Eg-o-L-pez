using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public float moveSpeed = 8f;
    public float jumpForce = 16f;
    public float dashSpeed = 15f;
   
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float dashDuration = 0.2f;
    public float coyoteTime = 0.2f;
    public int extraJumps = 1;
    public SpriteRenderer spriteRenderer;
    private bool facingRight = true;
    public Transform[] childObjects;
    private bool isFacingRight = true;            

    
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isDashing;
    private bool canDoubleJump;
    private bool isJumping;
    private int jumpCount;
    private float dashTimeLeft;
    private float lastGroundedTime;

    
   
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public ParticleSystem dashing;
    public ParticleSystem jumpingParticle;
    public ParticleSystem airTrailParticles;


    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = extraJumps;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        GroundCheck();
        

        if (isDashing)
        {
            return; 
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


        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
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



       
        if (Input.GetButtonDown("Jump") && (isGrounded || Time.time - lastGroundedTime <= coyoteTime))
        {
            Jump();
            isJumping = true;
            jumpingParticle.Play();
        }
        
        else if (Input.GetButtonDown("Jump") && canDoubleJump && !isGrounded)
        {
            Jump();
            canDoubleJump = false;
        }

        

        
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        if (!isGrounded)
        {
            if (!airTrailParticles.isPlaying)
            {
                airTrailParticles.Play();  
            }
        }
        else
        {
            if (airTrailParticles.isPlaying)
            {
                airTrailParticles.Stop();  
        }

    }

    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash());
            dashing.Play();
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        float originalGravity = rb.gravityScale;  
        rb.gravityScale = 0f;  

        
        rb.velocity = new Vector2(facingRight ? dashSpeed : -dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration); 

        rb.gravityScale = originalGravity;
        isDashing = false;
    }

    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }



    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        jumpingParticle.Play();
    }

    void Flip()
    {
        facingRight = !facingRight;  

        
        spriteRenderer.flipX = !spriteRenderer.flipX;

        
        foreach (Transform child in childObjects)
        {
            if (child.GetComponent<ParticleSystem>() == null) 
            {
                child.localScale = new Vector3(-child.localScale.x, child.localScale.y, child.localScale.z);
            }
            else 
            {
                var particleSystemRotation = child.localRotation.eulerAngles;
                particleSystemRotation.y = facingRight ? 0 : 180;  
                child.localRotation = Quaternion.Euler(particleSystemRotation);
            }
        }
    }
}