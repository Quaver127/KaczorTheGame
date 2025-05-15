using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Toggle screenToggle;
    public GameObject Background;
    public GameObject Checkmark;
    public AudioMixer mainMixer;
    
    public void setVolume(float volume)
    {
        mainMixer.SetFloat("volume", volume);
    }
    
    public void setFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        if (screenToggle.isOn)
        {
            Background.SetActive(true);
            Checkmark.SetActive(false);
        }
        else
        {
            Background.SetActive(false);
            Checkmark.SetActive(true);
        }
    }
    

    public void setQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    
    
}
