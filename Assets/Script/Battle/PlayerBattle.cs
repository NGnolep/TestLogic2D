using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerBattle : MonoBehaviour
{
    public PlayerData playerData;
    public float currentHP;
    public UnityEvent OnTakeDamage;
    public UnityEvent OnDeath;
    public UnityEvent<float, float> OnHPChanged;
    public RPSChoice currentChoice;
    private PlayerAnimation anim;
    private bool isDead = false;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        anim = GetComponent<PlayerAnimation>();
        currentHP = playerData.maxHP;

        OnHPChanged.AddListener(UIHandler.Instance.UpdatePlayerHPUI);
        OnHPChanged?.Invoke(currentHP, playerData.maxHP);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, playerData.maxHP);

        OnTakeDamage?.Invoke();
        OnHPChanged?.Invoke(currentHP, playerData.maxHP);

        if (currentHP <= 0)
        {
            isDead = true;
            anim.PlayDeath();
            OnDeath?.Invoke();
            AudioManager.Instance.PlaySFX(AudioManager.Instance.deathSound);
            UIHandler.Instance.ShowGameOverPanel();
            StartCoroutine(DestroyAfterDeath());
        }
        else
        {
            anim.PlayHit();
        }
    }
    public float GetDamage()
    {
        anim.PlayAttack();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.attackSound);
        return playerData.baseDamage;
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
