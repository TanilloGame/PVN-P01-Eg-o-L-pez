using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;            
    private int currentHealth;           

    public float invulnerabilityDuration = 2f; 
    private bool isInvulnerable = false;  
    public float knockbackForce = 10f;          
    public Color damageColor = Color.red;       
    public float colorChangeDuration = 0.2f;    
    public float flashInterval = 0.1f;    
    private bool isKnockedback = false;

    public LayerMask enemyLayer;                
    public Animator animator;                  
    public string deathAnimationName = "Death"; 

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;      
    private Color originalColor;               

    private bool isDead = false;                
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
        
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(1, Vector2.left);
        }
    }

    
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
                
                Die();
            }
            else
            {
                
                if (!isKnockedback)
                {
                    rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                }

                
                StartCoroutine(InvulnerabilityFlash());
            }
        }
    }

    
    void ApplyKnockback(Vector2 direction)
    {
        rb.velocity = Vector2.zero; 
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }

    

    
    private IEnumerator ChangeColorOnDamage()
    {
        spriteRenderer.color = damageColor;  
        yield return new WaitForSeconds(colorChangeDuration);
        spriteRenderer.color = originalColor; 
    }
    private IEnumerator InvulnerabilityFlash()
    {
        isInvulnerable = true;
        float invulnerabilityTimer = 0f;

        
        while (invulnerabilityTimer < invulnerabilityDuration)
        {
            spriteRenderer.enabled = false; 
            yield return new WaitForSeconds(flashInterval); 
            spriteRenderer.enabled = true;  
            yield return new WaitForSeconds(flashInterval); 

            invulnerabilityTimer += flashInterval * 2; 
        }

        isInvulnerable = false;
    }

   
    void Die()
    {
        if (!isDead)
        {
            isDead = true; 
            Debug.Log("Jugador ha muerto");

            
            if (animator != null)
            {
                animator.SetTrigger(deathAnimationName); 
            }

            
            StartCoroutine(HandleDeathAnimation());
        }
    }

    
    private IEnumerator HandleDeathAnimation()
    {
        
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        
        Destroy(gameObject); 
    }
}