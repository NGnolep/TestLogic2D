using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerBattle : MonoBehaviour
{
    public PlayerData playerData;
    private float currentHP;

    public Slider hpSlider;
    public TMP_Text hpText;

    public UnityEvent OnTakeDamage;
    public UnityEvent OnDeath;
    public UnityEvent<float, float> OnHPChanged;
    public RPSChoice currentChoice;
    private PlayerAnimation anim;
    private bool isDead = false;
    private Animator animator;
    private void Start()
    {
        isDead = false;
        animator = GetComponent<Animator>();
        anim = GetComponent<PlayerAnimation>();
        currentHP = playerData.maxHP;
        OnHPChanged.AddListener(UpdateHPUI);
        OnHPChanged?.Invoke(currentHP, playerData.maxHP);
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
        currentHP = Mathf.Clamp(currentHP, 0, playerData.maxHP);

        OnTakeDamage?.Invoke();
        OnHPChanged?.Invoke(currentHP, playerData.maxHP);

        if (currentHP <= 0)
        {
            isDead = true;
            anim.PlayDeath();
            OnDeath?.Invoke();
            Debug.Log($"player has been defeated!");
            StartCoroutine(DestroyAfterDeath());
        }
        anim.PlayHit();
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject); 
    }
    public float GetDamage()
    {
        anim.PlayAttack();
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
