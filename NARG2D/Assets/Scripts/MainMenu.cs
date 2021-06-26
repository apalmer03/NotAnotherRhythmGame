using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void tempStart()
    {
        SceneManager.LoadScene("Scenes/Prototype");
    }

    public void quitGame()
    {
        Debug.Log("Quit the Game!");
        Application.Quit();
    }
}
