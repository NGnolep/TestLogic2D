using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
public class EnemyBattle : MonoBehaviour
{
    public EnemyData enemyData;
    private float currentHP;
    public Slider hpSlider;
    public TMP_Text hpText;
    public UnityEvent OnTakeDamage;
    public UnityEvent OnDeath;
    public UnityEvent<float, float> OnHPChanged;
    public RPSChoice currentChoice;

    [HideInInspector] public EnemySpawner spawner;

    private void Start()
    { 
        if (enemyData != null)
        {
            InitializeEnemy(enemyData);
        }
        else
        {
            Debug.LogWarning("EnemyData not assigned to " + gameObject.name);
        }
    }

    public void InitializeEnemy(EnemyData data)
    {
        enemyData = data;
        currentHP = enemyData.baseHP;

        if (hpSlider != null)
        {
            hpSlider.maxValue = enemyData.baseHP;
            hpSlider.value = currentHP;
            Debug.Log("hpSlider assigned correctly");
        }
        else
        {
            Debug.LogWarning("hpSlider is NULL for " + gameObject.name);
        }
        if (hpText != null)
        {
            hpText.text = $"{currentHP}/{enemyData.baseHP}";
            Debug.Log("hpText assigned correctly");
        }
        else
        {
            Debug.LogWarning("hpText is NULL for " + gameObject.name);
        }
        OnHPChanged.AddListener(UpdateHPUI);
        OnHPChanged?.Invoke(currentHP, enemyData.baseHP);

        Debug.Log($"Initialized Enemy: {enemyData.enemyName} | Current HP: {currentHP}/{enemyData.baseHP}");
    }

    private void UpdateHPUI(float current, float max)
    {
        if (hpSlider != null)
            hpSlider.value = current;

        if (hpText != null)
            hpText.text = $"{current}/{max}";
    }
    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, enemyData.baseHP);

        if (hpSlider != null)
            hpSlider.value = currentHP;


        OnTakeDamage?.Invoke();
        OnHPChanged?.Invoke(currentHP, enemyData.baseHP);
        if (currentHP <= 0)
        {
            Debug.Log(enemyData.enemyName + " has been defeated!");
            OnDeath?.Invoke();

            // Inform spawner this enemy is gone
            if (spawner != null)
                spawner.OnEnemyDefeated(this);
        }
    }
    public float GetDamage()
    {
        return enemyData.baseDamage;
    }
}
