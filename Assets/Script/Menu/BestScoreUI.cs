using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BestScoreUI : MonoBehaviour
{
    public TMP_Text bestScoreText; 

    private void Start()
    {
        ShowBestScore();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Z) && Input.GetKeyDown(KeyCode.X))
        {
            ResetBestScorePref();
        }
    }
    public void ShowBestScore()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0); 
        if (bestScoreText != null)
            bestScoreText.text = $"Best Score: {bestScore}";
    }

    private void ResetBestScorePref()
    {
        PlayerPrefs.DeleteKey("BestScore"); 
        PlayerPrefs.Save();
        Debug.Log("Best Score has been reset to 0!");
    }
}
