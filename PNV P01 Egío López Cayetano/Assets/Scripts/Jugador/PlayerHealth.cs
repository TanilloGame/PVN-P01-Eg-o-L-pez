using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour

{

    [SerializeField] private Image playerLife1;
    [SerializeField] private Image playerLife2;
    [SerializeField] private Image playerLife3;
    public int health;
    public int maxHealth = 3;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int amount) 
    { 
        health -= amount;
        if (health <= 0)
        Destroy(gameObject);
        switch (health) 
        {
            case 2: 
                Destroy(playerLife1);
                break;
            case 1:
                Destroy(playerLife2);
                break;
            case 0:
                Destroy(playerLife3);
                break;


        }
        



    }
}

