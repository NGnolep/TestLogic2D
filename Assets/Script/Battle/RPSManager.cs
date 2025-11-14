using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RPSManager : MonoBehaviour
{
    public PlayerBattle player;
    public EnemyBattle enemy;
    public Button[] choiceButtons;
    public float buttonCooldown = 1.5f;

    private bool roundActive = false;

    public Image playerChoiceImage;
    public Image enemyChoiceImage;

    public Sprite rockSprite;
    public Sprite paperSprite;
    public Sprite scissorSprite;
    private void Start()
    {
        SetButtonsActive(false, true);
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
    public void EnableBattleRound()
    {
        roundActive = true;
        SetButtonsActive(true, false); 
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
    public void ResolveBattle()
    {
        if (!roundActive) return;
        roundActive = false;
        SetButtonsActive(false, true);
        if (player == null || enemy == null)
        {
            Debug.LogWarning("Player or Enemy not assigned in RPSBattleManager!");
            return;
        }

        enemy.currentChoice = (RPSChoice)Random.Range(0, 3);

        RPSChoice playerChoice = player.currentChoice;
        RPSChoice enemyChoice = enemy.currentChoice;
        if (playerChoiceImage != null)
            playerChoiceImage.sprite = GetSpriteForChoice(playerChoice);

        if (enemyChoiceImage != null)
            enemyChoiceImage.sprite = GetSpriteForChoice(enemyChoice);
        Debug.Log($"Player chose {playerChoice}, Enemy chose {enemyChoice}");

        if (playerChoice == enemyChoice)
        {
            Debug.Log("Draw! No one takes damage.");
        }
        else
        {
            bool playerWins =
                (playerChoice == RPSChoice.Rock && enemyChoice == RPSChoice.Scissors) ||
                (playerChoice == RPSChoice.Paper && enemyChoice == RPSChoice.Rock) ||
                (playerChoice == RPSChoice.Scissors && enemyChoice == RPSChoice.Paper);

                if (playerWins)
                {
                    Debug.Log("Player wins this round!");
                    enemy.TakeDamage(player.GetDamage());
                }
                else
                {
                    Debug.Log("Enemy wins this round!");
                    player.TakeDamage(enemy.GetDamage());
                }
        }
        StartCoroutine(RoundCooldown());
    }

    private IEnumerator RoundCooldown()
    {
        yield return new WaitForSeconds(buttonCooldown);

        if (enemy != null && enemy.gameObject.activeSelf)
        {
            roundActive = true;
            SetButtonsActive(true, false);
        }
    }
}
