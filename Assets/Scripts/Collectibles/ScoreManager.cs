using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour, IDataPersistence
{
    public static ScoreManager instance;
    
    public TextMeshProUGUI scoreText;
    

    public static int score;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Update()
    {
        scoreText.text = score.ToString();
        
    }

    public void ChangeScore(int CoinValue)
    {
     score += CoinValue;
     
     scoreText.text = "X " + score.ToString();
    }

    public void LoadData(GameData data)
    {
        ScoreManager.score = data.scoreData;
    }

    public void SaveData(ref GameData data)
    {
        data.scoreData = ScoreManager.score;
    }
}
