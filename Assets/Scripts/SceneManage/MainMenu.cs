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


    private void Start()
    {
        if (!DataPersistenceManager.instance.hasGameData())
        {
            continueGameButton.interactable = false;
        }
    }
    public void onNewGameClicked()
    {
        DataPersistenceManager.instance.NewGame();
        
        SceneManager.LoadSceneAsync("TestWorld");
    }

    public void onContinueClicked()
    {
        SceneManager.LoadSceneAsync("TestWorld");
    }
    public void QuitGame ()
    {
        Application.Quit();
    }

}