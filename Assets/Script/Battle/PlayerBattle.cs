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

    private void Start()
    {
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
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, playerData.maxHP);

        OnTakeDamage?.Invoke();
        OnHPChanged?.Invoke(currentHP, playerData.maxHP);

        if (currentHP <= 0)
        {
            OnDeath?.Invoke();
            Debug.Log($"player has been defeated!");
        }
    }

    public float GetDamage()
    {
        return playerData.baseDamage;
    }
}
