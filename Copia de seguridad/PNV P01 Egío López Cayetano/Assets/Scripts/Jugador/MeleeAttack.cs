using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public float attackRange = 1.5f; 
    public LayerMask enemyLayers; 
    public int attackDamage = 25; 
    public float knockbackForce = 10f; 
    public Transform attackPoint;
    public ParticleSystem scratchAttack;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            
            Attack();
        }
    }

    void Attack()
    {

        scratchAttack.Play();
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        
        foreach (Collider2D enemy in hitEnemies)
        {
            
            Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;

            
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage, knockbackDirection);
        }
        
    }

    
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
