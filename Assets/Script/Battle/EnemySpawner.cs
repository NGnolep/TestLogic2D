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

    private GameObject enemyPrefab;

    private EnemyBattle currentEnemy;
    
    public RPSManager battleManager;

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        EnemyData randomEnemy = enemyDatabase.GetRandomEnemy();
        if (randomEnemy == null)
        {
            Debug.LogWarning("No enemies in database!");
            yield break;
        }

        GameObject enemyObj = Instantiate(randomEnemy.enemyPrefab, spawnPoint.position, Quaternion.identity);
        currentEnemy = enemyObj.GetComponent<EnemyBattle>();

        currentEnemy.spawner = this;
        currentEnemy.enemyData = randomEnemy;

        Slider hpSlider = GameObject.Find("EnemyHP")?.GetComponent<Slider>();
        TMP_Text hpText = GameObject.Find("EnemyHPText")?.GetComponent<TMP_Text>();

        if (hpSlider != null && hpText != null)
        {
            currentEnemy.hpSlider = hpSlider;
            currentEnemy.hpText = hpText;

        }
        else
        {
            Debug.LogWarning("Enemy UI elements not found in scene!");
        }

        if (battleManager != null)
        {
            battleManager.enemy = currentEnemy;
            Debug.Log($"[EnemySpawner] Assigned new enemy ({randomEnemy.enemyName}) to BattleManager.");

            battleManager.SetButtonsActive(false, true);
        }

        if (battleManager != null)
        {
            battleManager.enemy = currentEnemy;
            Debug.Log($"[EnemySpawner] Assigned new enemy ({randomEnemy.enemyName}) to BattleManager.");
        }

        Debug.Log($"Spawned enemy: {randomEnemy.enemyName}");

        yield return StartCoroutine(MoveToPosition(enemyObj.transform, battlePoint.position));

        Debug.Log("Enemy reached battle point — wave start!");

        if (battleManager != null)
        {
            battleManager.EnableBattleRound();
        }
    }

    IEnumerator MoveToPosition(Transform enemy, Vector3 target)
    {
        while (Vector3.Distance(enemy.position, target) > 0.1f)
        {
            enemy.position = Vector3.MoveTowards(enemy.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnEnemyDefeated(EnemyBattle defeatedEnemy)
    {
        Debug.Log("Enemy defeated! Preparing next wave...");

        Destroy(defeatedEnemy.gameObject);

        StartCoroutine(NextWaveDelay(2f));
    }

    IEnumerator NextWaveDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(SpawnEnemyRoutine());
    }

}
