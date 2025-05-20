using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudio : MonoBehaviour
{
    
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Awake()
    {
        audioSource.ignoreListenerPause = true;
    }
}
