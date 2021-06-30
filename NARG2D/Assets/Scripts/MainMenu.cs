using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class MainMenu : MonoBehaviour
{
    public void tempStart(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        AnalyticsResult analytics_sceneLoaded = Analytics.CustomEvent("Scene Loaded " + sceneName);
    }

    public void quitGame()
    {
        Debug.Log("Quit the Game!");
        Application.Quit();
    }
}
