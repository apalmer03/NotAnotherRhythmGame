using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialGameProcess : MonoBehaviour
{
    public GameObject player;
    private Health playerHealth;
    public GameObject enemy;
    private Health enemyHealth;
    public AudioSource music;
    public GameObject gameOverUI;
    public GameObject levelCompUI;
    public GameObject NoteSystem;
    private bool isGameOver = false;
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
        music.Stop();
        NoteSystem.SetActive(false);
        Destroy(player.GetComponent<TutorialMainCharacterController>());
    }
    private void LevelComplete()
    {
        Time.timeScale = 0;
        levelCompUI.SetActive(true);
        music.Stop();
        NoteSystem.SetActive(false);
        Destroy(player.GetComponent<TutorialMainCharacterController>());

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
