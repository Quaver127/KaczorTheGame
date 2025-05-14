using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    
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
        Application.Quit();
    }
    
    
    IEnumerator NewGame()
    { 
        eye.SetActive(true);
        yield return new WaitForSeconds(0.33f);
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadSceneAsync("TestWorld");
    }
    
    IEnumerator ContinueGame()
    { 
        eye.SetActive(true);
        yield return new WaitForSeconds(0.33f);
        SceneManager.LoadSceneAsync("TestWorld");
    }

}