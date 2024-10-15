using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;             // Velocidad de movimiento horizontal
    public float jumpForce = 14f;             // Fuerza inicial del salto
    public float wallJumpForce = 12f;         // Fuerza del salto en la pared
    public float maxJumpTime = 0.35f;         // Duraci�n m�xima del salto mientras se mantiene presionado
    public float fallMultiplier = 2.5f;       // Aumenta la velocidad de ca�da
    public float lowJumpMultiplier = 2f;      // Aumenta la velocidad de ca�da cuando se suelta el bot�n de salto
    public int maxJumps = 2;                  // N�mero m�ximo de saltos permitidos (doble salto)

    [Header("Ground Check")]
    public Transform groundCheck;             // Referencia al objeto para detectar el suelo
    public float groundCheckRadius = 0.2f;    // Radio del Ground Check para detecci�n precisa
    public LayerMask groundLayer;             // Capa para detectar qu� es "suelo"
    private bool isGrounded;                  // �Est� el personaje en el suelo?

    [Header("Wall Check")]
    public Transform wallCheckLeft;           // Para verificar si el personaje est� tocando una pared a la izquierda
    public Transform wallCheckRight;          // Para verificar si el personaje est� tocando una pared a la derecha
    public float wallCheckRadius = 0.1f;      // Radio de verificaci�n de la pared
    public LayerMask wallLayer;               // Capa para detectar qu� es "pared"
    private bool isTouchingWallLeft;          // �Est� tocando una pared a la izquierda?
    private bool isTouchingWallRight;         // �Est� tocando una pared a la derecha?
    private bool isWallSliding;               // �Est� deslizando por la pared?
    public float wallSlideSpeed = 2f;         // Velocidad de deslizamiento en la pared

    private Rigidbody2D rb;
    private float moveInput;                  // Control de movimiento horizontal
    private float currentSpeed;
    private float jumpTimeCounter;            // Contador para controlar la duraci�n del salto
    private bool isJumping;                   // �Est� el jugador actualmente saltando?
    private int jumpCount;                    // Contador para el n�mero de saltos realizados

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = maxJumps;                 // Inicializar el n�mero de saltos disponibles
    }
     
    private void Update()
    {
        // Comprobar si el personaje est� en el suelo (Ground Check)
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Restablecer el contador de saltos al tocar el suelo
        if (isGrounded)
        {
            jumpCount = maxJumps;             // Resetear los saltos cuando el personaje toque el suelo
        }

        // Comprobar si est� tocando una pared a la izquierda o derecha
        isTouchingWallLeft = Physics2D.OverlapCircle(wallCheckLeft.position, wallCheckRadius, wallLayer);
        isTouchingWallRight = Physics2D.OverlapCircle(wallCheckRight.position, wallCheckRadius, wallLayer);

        // Comprobar si est� deslizando por la pared (solo si no est� en el suelo y toca una pared)
        if (!isGrounded && (isTouchingWallLeft || isTouchingWallRight) && rb.velocity.y < 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);  // Velocidad de deslizamiento
        }
        else
        {
            isWallSliding = false;
        }

        // Recoger la entrada horizontal (A para izquierda, D para derecha)
        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1f; // Mover a la izquierda
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1f; // Mover a la derecha
        }
        else
        {
            moveInput = 0f;
        }

        // Movimiento suave de aceleraci�n y desaceleraci�n
        if (moveInput != 0)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed * moveInput, 0.1f);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, 0.1f);
        }

        // Salto normal o desde la pared cuando se presiona Espacio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded || jumpCount > 0)
            {
                isJumping = true;
                jumpTimeCounter = maxJumpTime;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);  // Ejecutar el salto normal
                jumpCount--;  // Restar un salto disponible
            }
            else if (isWallSliding)
            {
                // Saltar desde la pared, invirtiendo la direcci�n horizontal
                if (isTouchingWallLeft)
                {
                    rb.velocity = new Vector2(wallJumpForce, jumpForce);  // Saltar a la derecha desde la pared izquierda
                }
                else if (isTouchingWallRight)
                {
                    rb.velocity = new Vector2(-wallJumpForce, jumpForce);  // Saltar a la izquierda desde la pared derecha
                }
            }
        }

        // Mantener el salto mientras se mantiene presionada la tecla y no se ha excedido el tiempo m�ximo de salto
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false; // Si se excede el tiempo de salto, detener el salto
            }
        }

        // Terminar el salto si se suelta la barra espaciadora
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

        // Aplicar la gravedad personalizada para ajustar la velocidad de ca�da
        if (rb.velocity.y < 0 && !isWallSliding)
        {
            // Ca�da r�pida
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            // Ca�da cuando se suelta el bot�n de salto (para peque�os saltos)
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // Ajustar la velocidad horizontal
        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualizaci�n del Ground Check y Wall Check en el editor
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        if (wallCheckLeft != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheckLeft.position, wallCheckRadius);
        }
        if (wallCheckRight != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheckRight.position, wallCheckRadius);
        }
    }
}