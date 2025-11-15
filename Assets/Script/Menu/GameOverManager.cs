using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverManager : MonoBehaviour
{
    public void ReloadScene()
    {
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (FadeController.Instance != null)
            FadeController.Instance.FadeOutAndLoad(currentScene);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
    }
    public void GoToMainMenu()
    {
        if (FadeController.Instance != null)
            FadeController.Instance.FadeOutAndLoad("MainMenu");
        else
            SceneManager.LoadScene("MainMenu");
    }
}
