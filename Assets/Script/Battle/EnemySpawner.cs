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
    public TMP_Text roundText;
    public TMP_Text scoreText;

    public GameObject startPopup;         
    public TMP_Text startPopupText;        
    public float startPopupDuration = 1.5f;
    private int currentWave = 0;
    private int currentScore = 0;

    void Start()
    {
        UpdateRoundText();
        UpdateScoreText();
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
        currentWave++;
        UpdateRoundText();
        GameObject enemyObj = Instantiate(randomEnemy.enemyPrefab, spawnPoint.position, Quaternion.identity);
        currentEnemy = enemyObj.GetComponent<EnemyBattle>();

        currentEnemy.spawner = this;
        currentEnemy.enemyData = randomEnemy;

        Slider hpSlider = GameObject.Find("EnemyHP")?.GetComponent<Slider>();
        TMP_Text hpText = GameObject.Find("EnemyHPText")?.GetComponent<TMP_Text>();
        TMP_Text nameText = GameObject.Find("EnemyNameText")?.GetComponent<TMP_Text>();

        if (hpSlider != null && hpText != null)
        {
            currentEnemy.hpSlider = hpSlider;
            currentEnemy.hpText = hpText;
            currentEnemy.mobName = nameText;
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
        StartCoroutine(ShowStartPopup());

        if (battleManager != null)
        {
            battleManager.EnableBattleRound();
        }
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
        Debug.Log("Enemy defeated! Preparing next wave...");
        AddScore(2000);
        StartCoroutine(NextWaveDelay(2f));
    }

    IEnumerator NextWaveDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(SpawnEnemyRoutine());
    }
    
     private void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreText();
        SaveBestScore();
    }

    private void SaveBestScore()
    {
        int best = PlayerPrefs.GetInt("BestScore", 0);

        if (currentScore > best)
        {
            PlayerPrefs.SetInt("BestScore", currentScore);
            PlayerPrefs.Save();
        }
    }
    private void UpdateRoundText()
    {
        if (roundText != null)
            roundText.text = $"Round: {currentWave}";
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {currentScore}";
    }

    private IEnumerator ShowStartPopup()
    {
        if (startPopup == null || startPopupText == null)
            yield break;

        startPopupText.text = $"START!";
        startPopup.SetActive(true);

        yield return new WaitForSeconds(startPopupDuration);

        startPopup.SetActive(false);
    }
}
