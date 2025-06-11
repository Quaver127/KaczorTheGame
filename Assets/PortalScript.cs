using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour
{
    public GameObject video;
    public GameObject score;
    public GameObject health;
    public GameObject mana;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            video.SetActive(true);
            score.SetActive(false);
            health.SetActive(false);
            mana.SetActive(false);
            StartCoroutine(CloseVideo());
        }
    }


    IEnumerator CloseVideo()
    {
        yield return new WaitForSeconds(9f);
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
