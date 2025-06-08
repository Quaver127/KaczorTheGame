using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{
    [Header("Currency")] 
    public int scoreData;
    
    public GameData()
    {
        this.scoreData = 0;
    }
    
}
