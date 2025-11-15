using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EnemySpawner : MonoBehaviour
{
    public EnemyDatabase enemyDatabase;
    public Transform spawnPoint;       
    public Transform battlePoint;
    public float moveSpeed = 3f;
    public RPSManager battleManager;
    private int currentWave = 0;

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        currentWave++;
        UIHandler.Instance.SetCurrentWave(currentWave);

        EnemyData data = enemyDatabase.GetRandomEnemy();
        if (data == null) yield break;

        GameObject enemyObj = Instantiate(data.enemyPrefab, spawnPoint.position, Quaternion.identity);
        EnemyBattle enemy = enemyObj.GetComponent<EnemyBattle>();
        enemy.spawner = this;
        enemy.InitializeEnemy(data);

        battleManager.enemy = enemy;

        // Move to battle point
        yield return StartCoroutine(MoveToPosition(enemyObj.transform, battlePoint.position));

        StartCoroutine(UIHandler.Instance.ShowStartPopup());

        battleManager.EnableBattleRound();
    }

    IEnumerator MoveToPosition(Transform enemy, Vector3 target)
    {
        EnemyAnimation anim = enemy.GetComponent<EnemyAnimation>();
        while (Vector3.Distance(enemy.position, target) > 0.1f)
        {
            enemy.position = Vector3.MoveTowards(enemy.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnEnemyDefeated(EnemyBattle defeatedEnemy)
    {
        UIHandler.Instance.AddScore(2000);
        UIHandler.Instance.UpdateTotalScore(UIHandler.Instance.currentScore);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.deathSound);
        StartCoroutine(NextWaveDelay(2f));
    }

    IEnumerator NextWaveDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(SpawnEnemyRoutine());
    }
    
}
