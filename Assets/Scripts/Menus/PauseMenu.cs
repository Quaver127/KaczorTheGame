using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour, IDataPersistence
{
    FadeInOut fade;
    
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject manaContainer;
    [SerializeField] private GameObject ScoreContainer;
    [SerializeField] private GameObject HpContainer;
    [SerializeField] private GameObject optionsMenu;
   // [SerializeField] private GameObject duckWalk;
    private bool isPaused = false;

    public void Start()
    {
        fade = FindObjectOfType<FadeInOut>();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                PauseGame();
            }
        }
        
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        manaContainer.SetActive(false);
        ScoreContainer.SetActive(false);
        HpContainer.SetActive(false);
        Time.timeScale = 0; 
        isPaused = true;
        Cursor.visible = true;
    }
    
    public void Resume()
    {
        pauseMenu.SetActive(false);
        manaContainer.SetActive(true);
        ScoreContainer.SetActive(true);
        HpContainer.SetActive(true);
        Time.timeScale = 1;
        isPaused = false;
        Cursor.visible = false;
    }

    public void Save()
    {
        
    }

    public void Options()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void OptionsBack()
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void Quit(int sceneIndex)
    {
        StartCoroutine(Quit());
    }

    public void LoadData(GameData data)
    {
        
    }

    public void SaveData(ref GameData data)
    {
        
    }
    
    IEnumerator Quit()
    {   
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        fade.FadeIn();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
