using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Data/Enemy Database")]
public class EnemyDatabase : ScriptableObject
{
    public List<EnemyData> enemyList;

    public EnemyData GetEnemyByName(string name)
    {
        return enemyList.FirstOrDefault(e => e.enemyName == name);
    }

    public EnemyData GetRandomEnemy()
    {
        if (enemyList.Count == 0) return null;
        return enemyList[Random.Range(0, enemyList.Count)];
    }
}
