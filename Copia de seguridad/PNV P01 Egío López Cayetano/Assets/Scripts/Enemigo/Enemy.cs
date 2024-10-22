using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public float knockbackDuration = 0.2f;  
    public float knockbackSpeed = 5f;       
    private bool isKnockedBack = false;     
    private float knockbackTime;            
    private Vector2 knockbackDirection;     

    private Rigidbody2D rb;
    private Animator animator; 
    private bool isDead = false;
    public ParticleSystem enemyHit;
    public ParticleSystem attackParticles;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
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
        if (isDead) return; 

        currentHealth -= damage;

        // Iniciar empuje
        knockbackDirection = knockbackDir;
        isKnockedBack = true;
        knockbackTime = 0f;
        enemyHit.Play();
        attackParticles.Play();

        
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
            
            rb.velocity = knockbackDirection * knockbackSpeed * (1f - (knockbackTime / knockbackDuration));
        }
        else
        {
            
            isKnockedBack = false;
            rb.velocity = Vector2.zero;
        }
    }

    void Die()
    {
        
        isDead = true;

        
        animator.SetTrigger("Die");

        
        GetComponent<Collider2D>().enabled = false;

        
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        
        Destroy(this.gameObject, 1.5f); 
    }
}