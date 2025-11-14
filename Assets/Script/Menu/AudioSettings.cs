using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class AudioSettings : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        masterSlider.value = GetPrefOrDefault("Master", 1f);
        bgmSlider.value = GetPrefOrDefault("BGM", 1f);
        sfxSlider.value = GetPrefOrDefault("SFX", 1f);

        AudioManager.Instance.SetMasterVolume(masterSlider.value);
        AudioManager.Instance.SetBGMVolume(bgmSlider.value);
        AudioManager.Instance.SetSFXVolume(sfxSlider.value);

        masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(AudioManager.Instance.SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
    }

    private float GetPrefOrDefault(string key, float defaultValue)
    {
        if (PlayerPrefs.HasKey(key))
            return Mathf.Max(PlayerPrefs.GetFloat(key), 0.0001f);
        else
            return defaultValue;
    }
}
