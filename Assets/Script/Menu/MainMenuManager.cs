using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    public GameObject bestScorePanel;
    public GameObject optionsPanel;

    public string nextSceneName = "GameScene";  
    public float fadeWait = 1f;

    public void PlayGame()
    {
        StartCoroutine(FadeAndLoad());
    }

    private IEnumerator FadeAndLoad()
    {
        FadeController.Instance.FadeOut();

        yield return new WaitForSeconds(FadeController.Instance.fadeTime);

        SceneManager.LoadScene(nextSceneName);
    }
    public void OpenBestScore()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.clickSound);
        bestScorePanel.SetActive(true);
    }

    public void CloseBestScore()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.clickSound);
        bestScorePanel.SetActive(false);
    }
    public void OpenOptions()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.clickSound);
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.clickSound);
        optionsPanel.SetActive(false);
    }
    public void ExitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}
