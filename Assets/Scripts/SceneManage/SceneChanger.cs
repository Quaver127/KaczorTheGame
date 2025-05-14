using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Tymski;
public class SceneChanger : MonoBehaviour
{
    FadeInOut fade;
    public SceneReference nextScene;

    private void Start()
    {
        
    }

    public IEnumerator _ChangeScene()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(nextScene);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(_ChangeScene());
        }
    }
}
