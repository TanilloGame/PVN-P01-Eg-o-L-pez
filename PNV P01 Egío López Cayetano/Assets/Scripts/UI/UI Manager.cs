using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    [SerializeField] private Image playerLife1;
    [SerializeField] private Image playerLife2;
    [SerializeField] private Image playerLife3;
    public int health;
    public int maxHealth = 3;
    private static UIManager instance;
    [SerializeField] private TextMeshProUGUI coinText;

    public int coinValue = 1;
    public int currentCoins;
    // Start is called before the first frame update
    void Start()
    {
        currentCoins = PlayerPrefs.GetInt("coins");
        currentCoins = 0;
        coinText.text = currentCoins.ToString();
        
    }
    private void Awake()
    {
        instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public static void HealthUI(int amount)
    {
      
            
        switch (amount)
        {
            case 2:
                instance.playerLife1.gameObject.SetActive(false);
                break;
            case 1:
                instance.playerLife1.gameObject.SetActive(false);
                instance.playerLife2.gameObject.SetActive(false);
                break;
            case 0:
                instance.playerLife1.gameObject.SetActive(false);
                instance.playerLife3.gameObject.SetActive(false);
                instance.playerLife2.gameObject.SetActive(false);
                break;
        }

        }
    
    
}
