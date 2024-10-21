using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public float knockbackDuration = 0.2f;  // Duración del empuje
    public float knockbackSpeed = 5f;       // Velocidad máxima del empuje
    private bool isKnockedBack = false;     // Si el enemigo está siendo empujado
    private float knockbackTime;            // Tiempo que lleva empujado
    private Vector2 knockbackDirection;     // Dirección del empuje

    private Rigidbody2D rb;
    private Animator animator; // Referencia al Animator del enemigo
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Obtener el Animator
    }

    void Update()
    {
        if (isKnockedBack && !isDead)
        {
            KnockbackUpdate();
        }
    }

    public void TakeDamage(int damage, Vector2 knockbackDir)
    {
        if (isDead) return; // Evita que el enemigo reciba más daño si ya está muerto

        currentHealth -= damage;

        // Iniciar empuje
        knockbackDirection = knockbackDir;
        isKnockedBack = true;
        knockbackTime = 0f;

        // Si el enemigo muere
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void KnockbackUpdate()
    {
        knockbackTime += Time.deltaTime;

        if (knockbackTime < knockbackDuration)
        {
            // Aplica la fuerza de empuje al enemigo
            rb.velocity = knockbackDirection * knockbackSpeed * (1f - (knockbackTime / knockbackDuration));
        }
        else
        {
            // Detener el empuje y restablecer velocidad
            isKnockedBack = false;
            rb.velocity = Vector2.zero;
        }
    }

    void Die()
    {
        // Marca al enemigo como muerto
        isDead = true;

        // Reproduce la animación de muerte
        animator.SetTrigger("Die");

        // Deshabilita el collider del enemigo para que no interactúe más con el jugador
        GetComponent<Collider2D>().enabled = false;

        // Detener cualquier movimiento adicional
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        // Destruir al enemigo después de un tiempo (esperando que la animación termine)
        Destroy(gameObject, 1.5f); // Ajusta este tiempo para que coincida con la duración de la animación
    }
}