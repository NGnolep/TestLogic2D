using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioClip mainMenuBGM;
    public AudioClip clickSound;
    public AudioClip deathSound;
    public AudioClip attackSound;
    public AudioMixer mixer;
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetMasterVolume(PlayerPrefs.GetFloat("Master", 1f));
        SetBGMVolume(PlayerPrefs.GetFloat("BGM", 1f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFX", 1f));  
    }
    void Start()
    {
        AudioManager.Instance.PlayBGM(mainMenuBGM);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;

        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    public void SetMasterVolume(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        mixer.SetFloat("Master", Mathf.Log10(value) * 20f);
        PlayerPrefs.SetFloat("Master", value);
    }

    public void SetBGMVolume(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        mixer.SetFloat("BGM", Mathf.Log10(value) * 20f);
        PlayerPrefs.SetFloat("BGM", value);
    }

    public void SetSFXVolume(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        mixer.SetFloat("SFX", Mathf.Log10(value) * 20f);
        PlayerPrefs.SetFloat("SFX", value);
    }
}
