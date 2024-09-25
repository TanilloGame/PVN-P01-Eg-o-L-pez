using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour

{
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float speed;

    [SerializeField] private float jumpForce;
    [SerializeField] private float fallForce;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
            transform.Translate(speed * Time.deltaTime, 0, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        if (rb.velocity.y < 0) 
        {
           rb.AddForce(Vector2.down * fallForce);
        }
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.collider.CompareTag("coin"))
            Destroy(other.collider.gameObject);
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
            Destroy(gameObject);
    }




}



