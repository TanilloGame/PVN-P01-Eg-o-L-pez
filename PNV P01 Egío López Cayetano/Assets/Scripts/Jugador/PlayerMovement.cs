using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float jumpForce = 14f;
    public float acceleration = 0.1f;
    public float deceleration = 0.1f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isGrounded;

    private Rigidbody2D rb;
    private float moveInput;
    private float currentSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Comprobar si el personaje está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Recoger la entrada horizontal (A/D o flechas)
        moveInput = Input.GetAxisRaw("Horizontal");

        // Movimiento suave de aceleración y desaceleración
        if (moveInput != 0)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed * moveInput, acceleration);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, deceleration);
        }

        // Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Ajustar la velocidad horizontal
        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualización del ground check en el editor
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}