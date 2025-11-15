using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
public class EnemyBattle : MonoBehaviour
{
    public EnemyData enemyData;
    public float currentHP;
    public UnityEvent OnTakeDamage;
    public UnityEvent OnDeath;
    public UnityEvent<float, float> OnHPChanged;
    public RPSChoice currentChoice;
    private EnemyAnimation anim;
    private bool isDead = false;
    private Animator animator;
    [HideInInspector] public EnemySpawner spawner;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        anim = GetComponent<EnemyAnimation>();
    }

    public void InitializeEnemy(EnemyData data)
    {
        enemyData = Instantiate(data);
        currentHP = enemyData.baseHP;

        // Update UI
        UIHandler.Instance.UpdateEnemyHPUI(currentHP, enemyData.baseHP);
        UIHandler.Instance.UpdateEnemyName(enemyData.enemyName);

        // Subscribe to UI update
        OnHPChanged.AddListener(UIHandler.Instance.UpdateEnemyHPUI);
        OnHPChanged?.Invoke(currentHP, enemyData.baseHP);
    }
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, enemyData.baseHP);

        OnTakeDamage?.Invoke();
        OnHPChanged?.Invoke(currentHP, enemyData.baseHP);

        if (currentHP <= 0)
        {
            isDead = true;
            anim.PlayDeath();
            OnDeath?.Invoke();

            if (spawner != null)
                spawner.OnEnemyDefeated(this);

            StartCoroutine(DestroyAfterDeath());
        }
        else
        {
            anim.PlayHit();
        }
    }
    public float GetDamage()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.attackSound);
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
