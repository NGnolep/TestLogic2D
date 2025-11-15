using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class FadeController : MonoBehaviour
{
    public static FadeController Instance;
    public Image fadeImage;
    public float fadeTime = 1f;

    private Coroutine currentFade;
    private void Awake()
    {
        string scene = SceneManager.GetActiveScene().name;

       if (scene == "MainMenu")
        {
            if (Instance != null && Instance != this)
            {
                try
                {
                    SceneManager.sceneLoaded -= Instance.OnSceneLoaded;
                }
                catch { /* ignore if already unsubscribed */ }

                Destroy(Instance.gameObject);
            }
            Instance = this;
        }
        else
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(false);
            fadeImage.raycastTarget = false;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (fadeImage.gameObject.activeSelf)
        {
            if (currentFade != null)
                StopCoroutine(currentFade);

            currentFade = StartCoroutine(FadeIn());
        }
    }
    public IEnumerator FadeIn()
    {
        float t = 1f;
        while (t > 0f)
        {
            t -= Time.unscaledDeltaTime / fadeTime;
            fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }

        fadeImage.gameObject.SetActive(false);
        fadeImage.raycastTarget = false;
        currentFade = null;
    }

    public void FadeOutAndLoad(string sceneName)
    {
        if (currentFade != null)
            StopCoroutine(currentFade);

        fadeImage.gameObject.SetActive(true);
        fadeImage.raycastTarget = true;
        currentFade = StartCoroutine(FadeOut(sceneName));
    }

    public IEnumerator FadeOut(string sceneName)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / fadeTime;
            fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}
