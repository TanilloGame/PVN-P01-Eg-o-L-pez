using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Serialization;
using UnityEngine.U2D;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour

{
    
    public PlayerHealth playerHealth;
    public int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerHealth.TakeDamage(damage);

        
        
        }

    }
}
