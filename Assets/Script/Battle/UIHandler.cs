using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance;
    public Slider mobHpSlider;
    public TMP_Text mobName;
    public TMP_Text mobHpText;

    public Image playerChoiceImage;
    public Image enemyChoiceImage;
    public Sprite rockSprite;
    public Sprite paperSprite;
    public Sprite scissorSprite;
    public Button[] choiceButtons;

    public Slider playerHpSlider;
    public TMP_Text playerHpText;

    public TMP_Text roundText;
    public TMP_Text scoreText;

    public GameObject startPopup;         
    public TMP_Text startPopupText;        
    public float startPopupDuration = 1.5f;
    public int currentWave = 0;
    public int currentScore = 0;

    public TMP_Text enemiesDefeatedText;
    public TMP_Text rockCounterText;
    public TMP_Text paperCounterText;
    public TMP_Text scissorsCounterText;
    public TMP_Text totalScoreText;
    private int totalEnemiesDefeated = 0;
    private int rockCount = 0;
    private int paperCount = 0;
    private int scissorsCount = 0;

    public GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public Sprite GetSpriteForChoice(RPSChoice choice)
    {
        switch (choice)
        {
            case RPSChoice.Rock: return rockSprite;
            case RPSChoice.Paper: return paperSprite;
            case RPSChoice.Scissors: return scissorSprite;
        }
        return null;
    }
    public void SetButtonsActive(bool state, bool dim = false)
    {
        foreach (Button btn in choiceButtons)
        {
            if (btn == null) continue;

            btn.interactable = state;

            ColorBlock colors = btn.colors;
            if (dim)
            {
                colors.normalColor = new Color(1f, 1f, 1f, 0.5f);
                colors.highlightedColor = new Color(1f, 1f, 1f, 0.5f);
            }
            else
            {
                colors.normalColor = Color.white;
                colors.highlightedColor = Color.white;
            }
            btn.colors = colors;
        }
    }
    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreText();
        SaveBestScore();
    }

    public void SaveBestScore()
    {
        int best = PlayerPrefs.GetInt("BestScore", 0);

        if (currentScore > best)
        {
            PlayerPrefs.SetInt("BestScore", currentScore);
            PlayerPrefs.Save();
        }
    }
    public void UpdateRoundText()
    {
        if (roundText != null)
            roundText.text = $"Round: {currentWave}";
    }

    public void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {currentScore}";
    }

    public void UpdateTotalScore(int score)
    {
        if (totalScoreText != null)
            totalScoreText.text = $"Score: {score}";
    }
    public void SetCurrentWave(int wave)
    {
        currentWave = wave;
        UpdateRoundText();
    }
    public IEnumerator ShowStartPopup()
    {
        if (startPopup == null || startPopupText == null)
            yield break;

        startPopupText.text = $"START!";
        startPopup.SetActive(true);

        yield return new WaitForSeconds(startPopupDuration);

        startPopup.SetActive(false);
    }

    public void UpdatePlayerHPUI(float current, float max)
    {
        if (playerHpSlider != null)
            playerHpSlider.value = current;

        if (playerHpText != null)
            playerHpText.text = $"{current}/{max}";
    }

    public void UpdateEnemyHPUI(float current, float max)
    {
        if (mobHpSlider != null)
            mobHpSlider.value = current;

        if (mobHpText != null)
            mobHpText.text = $"{current}/{max}";
    }

    public void UpdateEnemyName(string name)
    {
        if (mobName != null)
            mobName.text = name;
    }
    public void IncrementChoiceCounter(RPSChoice choice)
    {
        switch (choice)
        {
            case RPSChoice.Rock:
                rockCount++;
                if (rockCounterText != null)
                    rockCounterText.text = $"Rock used: {rockCount}";
                break;
            case RPSChoice.Paper:
                paperCount++;
                if (paperCounterText != null)
                    paperCounterText.text = $"Paper used: {paperCount}";
                break;
            case RPSChoice.Scissors:
                scissorsCount++;
                if (scissorsCounterText != null)
                    scissorsCounterText.text = $"Scissors used: {scissorsCount}";
                break;
        }
    }
    public void IncrementEnemiesDefeated()
    {
        totalEnemiesDefeated++;
        if (enemiesDefeatedText != null)
            enemiesDefeatedText.text = $"Total Enemies Defeated: {totalEnemiesDefeated}";
    }

    public void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
}
