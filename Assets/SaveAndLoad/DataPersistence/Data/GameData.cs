using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{
    [Header("Currency")] 
    public int scoreData;
    
    public Vector3 playerPosition;
    
    public string sceneName;
    
    public GameData()
    {
        this.playerPosition = Vector3.zero;
        
        this.scoreData = 0;
        
        this.sceneName = string.Empty;
    }
    
}
