using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour, IDataPersistence
{
    FadeInOut fade;
    [Header("On/Off Objects")]
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] private GameObject manaContainer;
    [SerializeField] private GameObject ScoreContainer;
    [SerializeField] private GameObject HpContainer;
    [SerializeField] private GameObject optionsMenu;
    
    [Header("Silly Little Stuff")]
    public AudioSource audioSource;    // AudioSource that will play the clip
    public AudioClip ZaWarudo;
    public AudioClip TimeFlowsAgain;
   // [SerializeField] private GameObject duckWalk;
    public bool isPaused = false;

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
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(ZaWarudo);
        pauseMenu.SetActive(true);
        manaContainer.SetActive(false);
        ScoreContainer.SetActive(false);
        HpContainer.SetActive(false);
        isPaused = true;
        StartCoroutine(LerpTimeScale(Time.timeScale, 0f, 2f)); 
        Cursor.visible = true;
    }
    
    public void Resume()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(TimeFlowsAgain);
        pauseMenu.SetActive(false);
        manaContainer.SetActive(true);
        ScoreContainer.SetActive(true);
        HpContainer.SetActive(true);
        isPaused = false;
        StartCoroutine(LerpTimeScale(Time.timeScale, 1f, 1.9f));
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
    
     IEnumerator LerpTimeScale(float start, float end, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Time.timeScale = Mathf.Lerp(start, end, elapsed / duration);
            elapsed += Time.unscaledDeltaTime; 
            yield return null;
        }

        Time.timeScale = end; 
    }
}
