using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Data/Enemy Database")]
public class EnemyDatabase : ScriptableObject
{
    public EnemyData[] enemyList;

    public EnemyData GetEnemyByName(string name)
    {
        return enemyList.FirstOrDefault(e => e.enemyName == name);
    }
}
