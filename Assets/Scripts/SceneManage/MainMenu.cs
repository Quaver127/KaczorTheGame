using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    FadeInOut fade;
    
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button OptionsButton;
    [SerializeField] private Button QuitButton;
    [SerializeField] private Button QuackButton;
    
    [Header("Options Buttons")]
    [SerializeField] private Button BackButton;
    
    [Header("Misc Objects")]
    [SerializeField] private GameObject eye;
    [SerializeField] private GameObject optionsPanel;


    private void Start()
    {
        fade = FindObjectOfType<FadeInOut>();
        
        if (!DataPersistenceManager.instance.hasGameData())
        {
            continueGameButton.interactable = false;
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
        OptionsButton.gameObject.SetActive(false);
        QuitButton.gameObject.SetActive(false);
        QuackButton.gameObject.SetActive(false);
    }

    public void OptionsBack()
    {
        newGameButton.gameObject.SetActive(true);
        continueGameButton.gameObject.SetActive(true);
        OptionsButton.gameObject.SetActive(true);
        QuitButton.gameObject.SetActive(true);
        QuackButton.gameObject.SetActive(true);
        optionsPanel.SetActive(false);
    }
    
    public void QuitGame ()
    {
        StartCoroutine(Quit());
    }
    
    IEnumerator NewGame()
    {   
        fade.FadeIn();
        eye.SetActive(true);
        yield return new WaitForSeconds(3f);
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadSceneAsync("TestWorld");
    }
    
    IEnumerator ContinueGame()
    {
        fade.FadeIn();
        eye.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync("TestWorld");
    }

    IEnumerator Quit()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(3f);
        Application.Quit();
    }

}