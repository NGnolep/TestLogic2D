using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButtonChoice : MonoBehaviour
{
    public RPSChoice choice;
    public PlayerBattle player;
    public RPSManager battleManager;

    public void OnClickChoice()
    {
        player.currentChoice = choice;
        battleManager.ResolveBattle();
    }
}
