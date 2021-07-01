using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class GameProcess : MonoBehaviour
{
    public GameObject player;
    private Health playerHealth;
    public GameObject enemy;
    private Health enemyHealth;
    public AudioSource music;
    public GameObject gameOverUI;
    public GameObject levelCompUI;
    public Text scoreUI;
    public GameObject NoteSystem;
    public GameObject scoreBackground;
    public GameObject scoreS;
    public GameObject scoreA;
    public GameObject scoreB;
    public GameObject scoreC;
    public GameObject scoreD;
    public GameObject scoreF;
    private bool isGameOver = false;
    public NoteSystem nSys; //TEST
    private int totScore = 0; //TEST

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = player.GetComponent<Health>();
        enemyHealth = enemy.GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    { 
        if (playerHealth.currHealth <= 0 && !isGameOver)
        {
            isGameOver = true;
            GameOver();
        }
        if (enemyHealth.currHealth <= 0 && !isGameOver)
        {
            isGameOver = true;
            LevelComplete();
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
        //scoreBackground.SetActive(true);
        music.Stop();
        NoteSystem.SetActive(false);
        Destroy(player.GetComponent<MainCharacterController>());
        AnalyticsResult analytics_gameover = Analytics.CustomEvent("Game Over");
        //totScore = nSys.GetTotal();

        //totScore = totScore - 200; //200 point penalty for losing
        //if (totScore < 0)
        //{
        //    totScore = 0;
        //}

        //scoreUI.text = "Total Score: " + totScore.ToString(); //Update if score was deducted
        //calculateLetter();
    }
    private void LevelComplete()
    {
        Time.timeScale = 0;
        levelCompUI.SetActive(true);
        scoreBackground.SetActive(true);
        music.Stop();
        NoteSystem.SetActive(false);
        Destroy(player.GetComponent<MainCharacterController>());
        totScore = nSys.GetTotal();
        calculateLetter();
        AnalyticsResult analytics_gameover = Analytics.CustomEvent("Level Complete");
    }

    public void calculateLetter()
    { //Temp score values for the demo, will adjust / add complexity later
        if (totScore >= 3400)
        {
            scoreS.SetActive(true);
        }
        else if (totScore >= 2600)
        {
            scoreA.SetActive(true);
        }
        else if (totScore >= 1900)
        {
            scoreB.SetActive(true);
        }
        else if (totScore >= 1200)
        {
            scoreC.SetActive(true);
        }
        else if (totScore >= 600)
        {
            scoreD.SetActive(true);
        }
        else
        {
            scoreF.SetActive(true);
        }
    }

    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Back()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
