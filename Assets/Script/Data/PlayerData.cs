using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Data/Player Data")]
public class PlayerData : ScriptableObject
{
    public float maxHP = 100f;
    public float baseDamage = 10f;
}
