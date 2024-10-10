using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.Tilemaps;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using UnityEditor;



public class PlayerController : MonoBehaviour

{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float speed;

    [SerializeField] private float jumpForce;
    [SerializeField] private float fallForce;

    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private float maxVelocityX;
    [SerializeField] private float maxVelocityY;
    public int coinValue = 1;
    public int currentCoins;

    [SerializeField] private Transform groundedRaycastLeftOrigin;
    [SerializeField] private Transform groundedRaycastRightOrigin;

    
    [SerializeField] private bool falling = false;
    


    // Start is called before the first frame update
    void Start()
    {
        currentCoins = PlayerPrefs.GetInt("coins");
        currentCoins = 0;
        coinText.text = currentCoins.ToString();


    }

    // Update is called once per frame
    void Update()
    {



       











        if (Input.GetKey(KeyCode.D))
        {

            transform.Translate(speed * Time.deltaTime, 0, 0);
            animator.SetBool("Walking", true);
            spriteRenderer.flipX = false;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(speed * -Time.deltaTime, 0, 0);
            animator.SetBool("Walking", true);
            spriteRenderer.flipX = true;




        }

        else
        {
            animator.SetBool("Walking", false);


        }

        //Salto

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            rb.velocity = Vector2.up * jumpForce;


            
            if (rb.velocity.y < 0)
            {

                rb.AddForce(Vector2.down * fallForce);
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxVelocityX, maxVelocityX), Mathf.Clamp(rb.velocity.y, -maxVelocityY, maxVelocityY));
               
            }
            
            




        

        }
        
        

    }


    private void OnCollisionEnter2D(Collision2D other)

    {
        if (other.collider.CompareTag("coin"))

        {
            Destroy(other.collider.gameObject);
            currentCoins++;
            coinText.text = currentCoins.ToString();

        }

        if (other.collider.CompareTag("Door"))

        {
            PlayerPrefs.SetInt("coins", coinValue);
            SceneManager.LoadScene("Level 2");
        }

    }

    

    

}



