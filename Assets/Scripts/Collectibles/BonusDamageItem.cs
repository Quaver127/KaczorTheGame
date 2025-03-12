using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDamageItem : MonoBehaviour
{
    public CharacterController2D controller;
    public Attack attack;
    public GameObject damageUpObject;
    public GameObject bonusDesc;
    [SerializeField] GameObject player;
    private bool playerInTrigger = false;
    public bool isUpgradePicked = false;


    public void Start()
    {
        if (isUpgradePicked)
        {
            Destroy(damageUpObject);
        }
    }
    public void Update()
    {
        if (playerInTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                attack.dmgValue += 1f;
                StartCoroutine(ShowDescription());
                
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

    IEnumerator ShowDescription()
    {
        isUpgradePicked = true;
        Debug.Log("FunctionIsBeingCalled");
        controller.canMove = false;
        bonusDesc.SetActive(true);
        yield return new WaitForSeconds(5f);
        controller.canMove = true;
        bonusDesc.SetActive(false);
        Destroy(damageUpObject);
        
    }
}
