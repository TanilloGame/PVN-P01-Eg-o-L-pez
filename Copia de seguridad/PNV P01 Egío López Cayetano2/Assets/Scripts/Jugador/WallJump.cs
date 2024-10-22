using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    // Par�metros configurables
    public float wallSlideSpeed = 2f;              // Velocidad al deslizarse en la pared
    public float wallJumpForceX = 6f;              // Fuerza horizontal del WallJump
    public float wallJumpForceY = 12f;             // Fuerza vertical del WallJump
    public float jumpBufferTime = 0.2f;            // Margen de tiempo para permitir un salto responsivo
    public float wallCheckDistance = 0.5f;         // Distancia para detectar paredes
    public LayerMask wallLayer;                    // Capa asignada a las paredes

    // Referencias
    private Rigidbody2D rb;
    private bool isTouchingWall;                   // Si el personaje est� tocando una pared
    private bool isWallSliding;                    // Si el personaje est� deslizando por la pared
    private bool isJumpingOffWall;                 // Si el personaje est� haciendo WallJump
    private float jumpTimeCounter;                 // Temporizador de salto
    private bool isFacingRight = true;             // Direcci�n en la que mira el personaje

    // Para la detecci�n de paredes
    public Transform wallCheckPointLeft;           // Punto para detectar paredes a la izquierda
    public Transform wallCheckPointRight;          // Punto para detectar paredes a la derecha
    public float checkRadius = 0.2f;               // Radio de detecci�n de las paredes
    public ParticleSystem jumpingParticle;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckWall();                                // Verificar colisiones con paredes
        HandleWallSlide();                          // Deslizar por la pared si es necesario
        HandleWallJump();                           // Manejar el salto desde la pared
    }

    private void CheckWall()
    {
        // Detectar si estamos tocando una pared a la izquierda o derecha
        bool isTouchingWallLeft = Physics2D.OverlapCircle(wallCheckPointLeft.position, checkRadius, wallLayer);
        bool isTouchingWallRight = Physics2D.OverlapCircle(wallCheckPointRight.position, checkRadius, wallLayer);

        isTouchingWall = isTouchingWallLeft || isTouchingWallRight;

        if (isTouchingWallLeft && isFacingRight || isTouchingWallRight && !isFacingRight)
        {
            Flip(); // Si estamos tocando la pared, invierte la direcci�n si es necesario
        }
    }

    private void HandleWallSlide()
    {
        // Si tocamos la pared y no estamos en el suelo, comenzamos a deslizar
        if (isTouchingWall && !isGrounded() && rb.velocity.y < 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);  // Desliza lentamente hacia abajo
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void HandleWallJump()
    {
        if (isWallSliding && Input.GetButtonDown("Jump"))
        {
            isJumpingOffWall = true;
            

            // Aplica una fuerza horizontal en direcci�n opuesta a la pared
            Vector2 wallJumpDirection = isFacingRight ? new Vector2(-wallJumpForceX, wallJumpForceY) : new Vector2(wallJumpForceX, wallJumpForceY);
            rb.velocity = wallJumpDirection;

            jumpTimeCounter = jumpBufferTime;  // Inicia el temporizador del buffer de salto
            jumpingParticle.Play();
        }

        // Controla el temporizador del buffer de salto para mejorar la responsividad
        if (isJumpingOffWall)
        {
            jumpTimeCounter -= Time.deltaTime;
            if (jumpTimeCounter <= 0)
            {
                isJumpingOffWall = false;
            }
        }
    }

    private bool isGrounded()
    {
        // Implementa tu l�gica para verificar si el jugador est� en el suelo
        // Se podr�a usar un GroundCheck con raycast o collider en el personaje
        return Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnDrawGizmos()
    {
        // Visualizaci�n de las �reas de detecci�n de las paredes en la vista de la escena
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(wallCheckPointLeft.position, checkRadius);
        Gizmos.DrawWireSphere(wallCheckPointRight.position, checkRadius);
    }
}