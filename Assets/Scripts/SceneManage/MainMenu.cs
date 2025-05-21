using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    FadeInOut fade;

    [Header("Egg Layers")] 
    public GameObject crack1;
    public GameObject crack2;
    public GameObject legL;
    public GameObject legR;
    public int layerCounter = 0;
    
    [Header("New Game Shenanigans")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private AudioSource crackSound;
    [SerializeField] private AudioSource funnySound;
    [SerializeField] private AudioSource jebemIgora;
    [Header("Menu Buttons")]
    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button quackButton;
    
    [Header("Options Buttons")]
    [SerializeField] private Button backButton;
    
    [Header("Misc Objects")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private AudioSource sneezeSound;


    private void Start()
    {
        fade = FindObjectOfType<FadeInOut>();
        
        if (!DataPersistenceManager.instance.hasGameData())
        {
            continueGameButton.interactable = false;
            continueGameButton.gameObject.SetActive(false);
        }
        else
        {
            continueGameButton.interactable = true;
        }
    }
    public void onNewGameClicked()
    {
        StartCoroutine(NewGame());
    }

    public void onContinueClicked()
    {
        StartCoroutine(ContinueGame());
    }

    public void Options()
    {
        newGameButton.gameObject.SetActive(false);
        continueGameButton.gameObject.SetActive(false);
        optionsButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        quackButton.gameObject.SetActive(false);
    }

    public void OptionsBack()
    {
        newGameButton.gameObject.SetActive(true);
        continueGameButton.gameObject.SetActive(true);
        optionsButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        quackButton.gameObject.SetActive(true);
        optionsPanel.SetActive(false);
    }
    
    public void QuitGame ()
    {
        StartCoroutine(Quit());
    }

    public void Quack()
    {
        sneezeSound.Play();
    }
    
    IEnumerator NewGame()
    {
        if (layerCounter == 0)
        {   
            crackSound.Play();
            crack1.SetActive(true);
            layerCounter++;
        }
        else if (layerCounter == 1)
        {
            crackSound.Play();
            crack1.SetActive(false);
            crack2.SetActive(true);
            layerCounter++;
        }
        else if (layerCounter == 2)
        {
            jebemIgora.Stop();
            crackSound.Play();
            legL.SetActive(true);
            legR.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            funnySound.Play();
            fade.FadeIn();
            yield return new WaitForSeconds(4.5f);
            DataPersistenceManager.instance.NewGame();
            SceneManager.LoadSceneAsync("MainWorld");
        }
        
    }
    
    IEnumerator ContinueGame()
    {
        layerCounter = 2;
        if (layerCounter == 2)
        {
            jebemIgora.Stop();
            funnySound.Play();
            fade.FadeIn();
            crack2.SetActive(true);
            legL.SetActive(true);
            legR.SetActive(true);
            yield return new WaitForSeconds(4.5f);
            SceneManager.LoadSceneAsync("MainWorld");
        }
        
    }
    
    IEnumerator Quit()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(3f);
        Application.Quit();
    }
    

}