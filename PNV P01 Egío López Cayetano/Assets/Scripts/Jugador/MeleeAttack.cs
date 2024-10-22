using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public float attackRange = 1.5f; // El rango del ataque cuerpo a cuerpo
    public LayerMask enemyLayers; // Las capas que detectan a los enemigos
    public int attackDamage = 25; // Da�o que inflige el ataque
    public float knockbackForce = 10f; // Fuerza del empuje al golpear a un enemigo

    
    public Transform attackPoint;
    
    public ParticleSystem scratchAttack;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Bot�n izquierdo del rat�n para atacar
        {
            
            Attack();
        }
    }

    void Attack()
    {

        scratchAttack.Play();
        // Detecta los enemigos en el rango de ataque
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Aplica da�o y empuje a cada enemigo golpeado
        foreach (Collider2D enemy in hitEnemies)
        {
            // Calcula la direcci�n del empuje
            Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;

            // Aplica da�o y empuje al enemigo
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage, knockbackDirection);
        }
        
    }

    // Visualizaci�n del rango de ataque en la vista de escena
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
