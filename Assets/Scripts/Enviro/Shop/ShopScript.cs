using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ShopScript : MonoBehaviour
{
    public GameObject pillar1;
    public GameObject pillar2;
    
    public AudioSource audioSource;
    public AudioClip audioClip;
    [SerializeField]  GameObject player;
    private bool playerInTrigger = false;
    public GameObject shopScreen;
    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.B))
        {
            shopScreen.SetActive(!shopScreen.activeSelf);
            if (shopScreen.activeSelf)
            {
                Cursor.visible = true;
                pillar1.SetActive(true);
                pillar2.SetActive(true);
            }
            else
            {
                Cursor.visible = false;
                pillar1.SetActive(false);
                pillar2.SetActive(false);
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

    public void BuyDash()
    {
        if (ScoreManager.score >= 1)
        {
            ScoreManager.score -= 1;
            CharacterController2D.dashUnlocked = true;
            audioSource.PlayOneShot(audioClip);
            
        }
    }

    public void BuyDamage()
    {
        
    }

    public void BuyRange()
    {
        
    }
}
