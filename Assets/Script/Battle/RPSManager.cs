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
        SetButtonsActive(true, false); // enable and undim
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
