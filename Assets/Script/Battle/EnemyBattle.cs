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
    public TMP_Text mobName;
    public TMP_Text hpText;
    public UnityEvent OnTakeDamage;
    public UnityEvent OnDeath;
    public UnityEvent<float, float> OnHPChanged;
    public RPSChoice currentChoice;
    private EnemyAnimation anim;
    private bool isDead = false;
    private Animator animator;
    [HideInInspector] public EnemySpawner spawner;

    private void Start()
    { 
        isDead = false;
        animator = GetComponent<Animator>();
        anim = GetComponent<EnemyAnimation>();
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
        enemyData = Instantiate(data);
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
        if  (mobName != null)
        {
            mobName.text = $"{enemyData.enemyName}";
            Debug.Log("Enemy name assigned");
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
        if (isDead) return;
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, enemyData.baseHP);

        if (hpSlider != null)
            hpSlider.value = currentHP;
        OnTakeDamage?.Invoke();
        OnHPChanged?.Invoke(currentHP, enemyData.baseHP);
        if (currentHP <= 0)
        {
            isDead = true;
            Debug.Log(enemyData.enemyName + " has been defeated!");
            anim.PlayDeath();
            OnDeath?.Invoke();

            // Inform spawner this enemy is gone
            if (spawner != null)
                spawner.OnEnemyDefeated(this);
            
            StartCoroutine(DestroyAfterDeath());
        }
        anim.PlayHit();
    }
    public float GetDamage()
    {
        anim.PlayAttack();
        return enemyData.baseDamage;
    }
    private IEnumerator DestroyAfterDeath()
    {
        // Wait for animator to switch to the death state
        yield return new WaitForEndOfFrame();

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        float animLength = state.length;

        yield return new WaitForSeconds(animLength); // wait until last frame

        Destroy(gameObject);
    }
}
