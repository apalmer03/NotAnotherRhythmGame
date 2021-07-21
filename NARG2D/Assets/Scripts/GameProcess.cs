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
    public NoteSystem nSys;
    private int totScore = 0;
    public GameObject pausePage;
    private bool gamePaused = false;
    private Animator playerAnim;
    private Animator enemyAnim;
    public GameObject startTutorial;
    public Text tutorialText;
    public GameObject centerRing;
    public GameObject outerRing;
    public GameObject controls;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = player.GetComponent<Health>();
        enemyHealth = enemy.GetComponent<Health>();
        playerAnim = player.GetComponent<Animator>();
        enemyAnim = enemy.GetComponent<Animator>();
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gamePaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void StartTutorial()
    {
        player.GetComponent<MainCharacterController>().paused = false;
        gamePaused = false;
        Time.timeScale = 1;
        music.Play();
        AudioListener.pause = false;
        startTutorial.gameObject.SetActive(false);
        tutorialText.text = "Light Attack - A\nHeavy Attack - S\nBlock: D\nEngage Ultimate: F\nControl Ultimate\nArrow Keys\nJump: Space";

        centerRing.gameObject.SetActive(true);
        outerRing.gameObject.SetActive(true);
        controls.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        player.GetComponent<MainCharacterController>().paused = false;
        gamePaused = false;
        Time.timeScale = 1;
        music.Play();
        AudioListener.pause = false;
        pausePage.gameObject.SetActive(false);
    }

    public void PauseGame()
    {
        player.GetComponent<MainCharacterController>().paused = true;
        gamePaused = true;
        Time.timeScale = 0f;
        music.Pause();
        AudioListener.pause = true;
        pausePage.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        Time.timeScale = 0;

        gameOverUI.SetActive(true);
        scoreBackground.SetActive(true);
        music.Stop();
        NoteSystem.SetActive(false);
        Destroy(player.GetComponent<MainCharacterController>());
        calculateLetter(false);
        AnalyticsResult analytics_gameover = Analytics.CustomEvent("Game Over");
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
        calculateLetter(true);
        AnalyticsResult analytics_gameover = Analytics.CustomEvent("Level Complete");
    }

    public void calculateLetter(bool successfulFinish)
    { //Temp score values for the demo, will adjust / add complexity later

        if (successfulFinish == false)
        {
            scoreF.SetActive(true);
        }
        else
        {
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
    }

    public void Retry()
    {
        ResumeGame();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Back()
    {
        ResumeGame();
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
