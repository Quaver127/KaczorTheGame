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
    
    [Header("Misc Objects")]
    [SerializeField] private GameObject eye;


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