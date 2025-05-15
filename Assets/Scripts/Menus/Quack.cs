using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundPlayer : MonoBehaviour
{
    public Button button;              // Reference to the UI Button
    public AudioSource audioSource;    // AudioSource that will play the clip
    public AudioClip clickSound;       // Sound to play on click

    void Start()
    {
        if (button != null)
        {
            button.onClick.AddListener(PlayClickSound);
        }
    }

    void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}