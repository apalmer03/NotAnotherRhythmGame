using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LSMenu : MonoBehaviour
{
    public Animator transition1;
    public Animator transition2;
    public float transitionTime = 2.5f;

    public void PlayTutorial()
    {
        StartCoroutine(LoadLevel("Tutorial"));
    }

    public void PlayLevelOne()
    {
        StartCoroutine(LoadLevel("NewLevel"));
    }

    IEnumerator LoadLevel(string levelName)
    {
        transition1.SetTrigger("StartLevel");
        transition2.SetTrigger("StartLevel");
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);
    }
}
