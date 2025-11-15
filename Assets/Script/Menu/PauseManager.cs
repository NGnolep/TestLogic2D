using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void PauseButton()
    {
        TogglePause();
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f; 
    }

    public void ResumeButton()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void QuitButton()
    {
        Time.timeScale = 1f;
        if (FadeController.Instance != null)
            FadeController.Instance.FadeOutAndLoad("MainMenu");
        else
            SceneManager.LoadScene("MainMenu");
    }
}
