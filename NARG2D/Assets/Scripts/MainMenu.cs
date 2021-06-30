using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class MainMenu : MonoBehaviour
{
    public void sceneStart(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        AnalyticsResult sceneLoaded = Analytics.CustomEvent("SceneLoaded " + sceneName);
        Debug.Log("Analytics result " + sceneLoaded);
    }

    public void quitGame()
    {
        Debug.Log("Quit the Game!");
        Application.Quit();
    }
}
