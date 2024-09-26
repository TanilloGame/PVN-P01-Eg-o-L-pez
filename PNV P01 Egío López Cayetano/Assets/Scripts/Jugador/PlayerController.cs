using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;



public class PlayerController : MonoBehaviour

{

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float speed;

    [SerializeField] private float jumpForce;
    [SerializeField] private float fallForce;

    [SerializeField] private TextMeshProUGUI coinText;

    public int coinValue = 1;
    public int currentCoins;




    // Start is called before the first frame update
    void Start()
    {
        currentCoins = 0;
        coinText.text = currentCoins.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
            transform.Translate(speed * Time.deltaTime, 0, 0);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(speed * -Time.deltaTime, 0, 0);

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
        
        {
            Destroy(other.collider.gameObject);
            currentCoins++;
            coinText.text = currentCoins.ToString();
            
        }

        if (other.collider.CompareTag("Door"))

        {
            SceneManager.LoadScene("Level 2");
        }

    }




}



