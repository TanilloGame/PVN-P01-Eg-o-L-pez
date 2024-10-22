using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;            // Puntos de salud m�ximos
    private int currentHealth;           // Salud actual

    public float invulnerabilityDuration = 2f; // Duraci�n de la invulnerabilidad tras recibir da�o
    private bool isInvulnerable = false;  // Estado de invulnerabilidad
    public float knockbackForce = 10f;          // Fuerza de empuje al recibir da�o
    public Color damageColor = Color.red;       // Color al recibir da�o
    public float colorChangeDuration = 0.2f;    // Duraci�n del cambio de color
    public float flashInterval = 0.1f;    // Intervalo entre flashes de parpadeo
    private bool isKnockedback = false;

    public LayerMask enemyLayer;                // Capa "Enemy"
    public Animator animator;                   // Referencia al Animator para animaciones
    public string deathAnimationName = "Death"; // Nombre de la animaci�n de muerte

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;      // Referencia para cambiar el color del sprite
    private Color originalColor;               // Almacena el color original del sprite

    private bool isDead = false;                // Para evitar m�ltiples llamadas a la muerte
    public ParticleSystem damageParticles;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        // Debug: Solo para reducir la salud manualmente (puedes eliminarlo despu�s de pruebas)
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(1, Vector2.left);
        }
    }

    // Detectar colisiones con objetos en la capa "Enemy"
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            
            if (!isInvulnerable && !isDead)
            {
                Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
                TakeDamage(1, knockbackDirection);
                damageParticles.Play();
            }
        }
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        if (!isInvulnerable)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                // Aqu� podr�as activar la animaci�n de muerte o el evento de muerte
                Die();
            }
            else
            {
                // Aplicar empuje si no est� siendo empujado
                if (!isKnockedback)
                {
                    rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                }

                // Iniciar la invulnerabilidad con parpadeo
                StartCoroutine(InvulnerabilityFlash());
            }
        }
    }

    // Aplicar el empuje
    void ApplyKnockback(Vector2 direction)
    {
        rb.velocity = Vector2.zero; // Detiene cualquier movimiento previo
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }

    

    // Cambia el color brevemente al recibir da�o
    private IEnumerator ChangeColorOnDamage()
    {
        spriteRenderer.color = damageColor;  // Cambia al color de da�o
        yield return new WaitForSeconds(colorChangeDuration);
        spriteRenderer.color = originalColor; // Vuelve al color original
    }
    private IEnumerator InvulnerabilityFlash()
    {
        isInvulnerable = true;
        float invulnerabilityTimer = 0f;

        // Parpadeo durante el tiempo de invulnerabilidad
        while (invulnerabilityTimer < invulnerabilityDuration)
        {
            spriteRenderer.enabled = false; // Hacer invisible
            yield return new WaitForSeconds(flashInterval); // Esperar un intervalo
            spriteRenderer.enabled = true;  // Hacer visible
            yield return new WaitForSeconds(flashInterval); // Esperar un intervalo

            invulnerabilityTimer += flashInterval * 2; // Aumentar el tiempo acumulado
        }

        isInvulnerable = false;
    }

    // L�gica de muerte
    void Die()
    {
        if (!isDead)
        {
            isDead = true; // Evitar m�ltiples llamadas a la muerte
            Debug.Log("Jugador ha muerto");

            // Iniciar la animaci�n de muerte
            if (animator != null)
            {
                animator.SetTrigger(deathAnimationName); // Activar la animaci�n de muerte
            }

            // Iniciar la coroutine para esperar a que la animaci�n termine
            StartCoroutine(HandleDeathAnimation());
        }
    }

    // Coroutine para manejar la animaci�n de muerte
    private IEnumerator HandleDeathAnimation()
    {
        // Esperar a que termine la animaci�n de muerte (asumiendo que la animaci�n tiene su duraci�n)
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Destruir o desactivar al jugador tras la animaci�n
        Destroy(gameObject); // O puedes desactivar el objeto: gameObject.SetActive(false);
    }
}