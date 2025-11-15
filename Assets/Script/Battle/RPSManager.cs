using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RPSManager : MonoBehaviour
{
    public PlayerBattle player;
    public EnemyBattle enemy;
    public float buttonCooldown = 1.5f;
    private bool roundActive = false;
    private void Start()
    {
        UIHandler.Instance.SetButtonsActive(false, true);
    }

    public void EnableBattleRound()
    {
        roundActive = true;
        UIHandler.Instance.SetButtonsActive(true, false);
    }
    public void ResolveBattle()
    {
        if (!roundActive) return;
        if (player == null || enemy == null) return;

        roundActive = false;
        UIHandler.Instance.SetButtonsActive(false, true);

        enemy.currentChoice = (RPSChoice)Random.Range(0, 3);
        RPSChoice playerChoice = player.currentChoice;
        RPSChoice enemyChoice = enemy.currentChoice;

        UIHandler.Instance.playerChoiceImage.sprite = UIHandler.Instance.GetSpriteForChoice(playerChoice);
        UIHandler.Instance.enemyChoiceImage.sprite = UIHandler.Instance.GetSpriteForChoice(enemyChoice);

        UIHandler.Instance.IncrementChoiceCounter(playerChoice);
        
        bool playerWins = (playerChoice == RPSChoice.Rock && enemyChoice == RPSChoice.Scissors) ||
                          (playerChoice == RPSChoice.Paper && enemyChoice == RPSChoice.Rock) ||
                          (playerChoice == RPSChoice.Scissors && enemyChoice == RPSChoice.Paper);

        if (playerChoice != enemyChoice)
        {
            if (playerWins)
            {
                enemy.TakeDamage(player.GetDamage());
            // If enemy is defeated
                if (enemy.currentHP <= 0)
                {
                    UIHandler.Instance.IncrementEnemiesDefeated();
                }
            }
            else
                player.TakeDamage(enemy.GetDamage());
        }
        StartCoroutine(RoundCooldown());
    }

    private IEnumerator RoundCooldown()
    {
        yield return new WaitForSeconds(buttonCooldown);
        if (enemy != null && enemy.gameObject.activeSelf)
        {
            roundActive = true;
            UIHandler.Instance.SetButtonsActive(true, false);
        }
    }
}
