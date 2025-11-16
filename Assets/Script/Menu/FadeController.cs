using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class FadeController : MonoBehaviour
{
    public static FadeController Instance;
    private Image fadeImage;
    public float fadeTime = 1f;
    private Coroutine currentFade;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        CreateFadeCanvas();
    }

    void CreateFadeCanvas()
    {
        GameObject canvasGO = new GameObject("FadeCanvas");
        canvasGO.transform.SetParent(transform);

        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999; // always on top

        CanvasGroup cg = canvasGO.AddComponent<CanvasGroup>();
        cg.interactable = false; // fade shouldn't block by default

        // Create fade image
        GameObject imgGO = new GameObject("FadeImage");
        imgGO.transform.SetParent(canvasGO.transform);
        fadeImage = imgGO.AddComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 1f);

        RectTransform rect = imgGO.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        fadeImage.raycastTarget = true;
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
        if (currentFade != null)
            StopCoroutine(currentFade);

        currentFade = StartCoroutine(FadeIn());
    }
    public IEnumerator FadeIn()
    {
        float t = 1f;

        fadeImage.gameObject.SetActive(true);
        fadeImage.raycastTarget = true;

        while (t > 0)
        {
            t -= Time.unscaledDeltaTime / fadeTime;
            fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.raycastTarget = false;
        currentFade = null;
    }

    public void FadeOutAndLoad(string scene)
    {
        if (currentFade != null)
            StopCoroutine(currentFade);

        currentFade = StartCoroutine(FadeOut(scene));
    }

    public IEnumerator FadeOut(string scene)
    {
        float t = 0f;

        fadeImage.gameObject.SetActive(true);
        fadeImage.raycastTarget = true;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / fadeTime;
            fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 1f);

        SceneManager.LoadScene(scene);
    }
}
