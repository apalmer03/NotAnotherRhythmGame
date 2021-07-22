using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialInstructionController : MonoBehaviour
{
	public Text instructionDisplay;
	public string[] instructionsArray;
	public int currInstructionIndex;
	public bool tutorialCompleted = false;

    public GameObject player;
    private bool gamePaused = false;
    public AudioSource music;

    public GameObject centerRing;
    public GameObject outerRing;

    public Text totalScore;
    public Text multiplier;
    public Text combo;

    public GameObject controls;


    // Start is called before the first frame update
    void Start()
    {
    	instructionsArray = new string[] {"Welcome to our Tutorial Level! Let us show you how to play our game!\nPress 'N' to continue.", 
    	"You control the character on the left (\"Hero\"). The enemy is the character on the right (\"Boss\"). Your goal is to defeat the boss. Next, we'll go over how gameplay works.\nPress 'N' to continue.", 
    	"Your Hero can jump, attack, and block. Perform actions on the beat (click key when beat circle perfectly overlaps center circle) to get a combo multiplier and increased score. We'll go over UI next.\nPress 'N' to continue.", 
    	"Respective health bars are above each character. The center timer indicates how much time is remaining in the current level (remaining length of song). Next, we'll go over basic controls.\nPress 'N' to continue.",
    	"Press 'A' for a light attack and 'S' for an uppercut. Press 'Space' to jump. Press 'D' to block. Press 'N' to see advanced controls.",
        "Special Move #1: Space + A + A (Blue Particle Effect). Special Move #2: A + S + A (Purple Particle Effect). Ultimate: Press 'F' when Ult bar below your health bar fills up (turns red). Try them out!\nWhen you're ready, start the tutorial level by pressing START below. Good luck and have fun! :)"};
    	
    	currInstructionIndex = 0;
    	instructionDisplay.text = instructionsArray[currInstructionIndex];
    	instructionDisplay.fontSize = 15;   

        player.GetComponent<MainCharacterController>().paused = true;
        gamePaused = true;
        Time.timeScale = 0f;
        music.Pause();
        AudioListener.pause = true;
        centerRing.gameObject.SetActive(false);
        outerRing.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
        	currInstructionIndex += 1;
        	if(currInstructionIndex < this.instructionsArray.Length)
        	{
        		instructionDisplay.text = instructionsArray[currInstructionIndex];
        	}

        	// Start Tutorial Level
        	if(currInstructionIndex == 6) {
                instructionDisplay.text = "Light Attack - A\nHeavy Attack - S\nBlock: D\nEngage Ultimate: F\nControl Ultimate\nArrow Keys\nJump: Space";
        		tutorialCompleted = true;
        	}

        }
        totalScore.text = "Total Score : 0";
        multiplier.text = "Multiplier: x1";
        combo.text = "";
    }

    public void ResumeGame()
    {
        player.GetComponent<MainCharacterController>().paused = false;
        gamePaused = false;
        Time.timeScale = 1;
        music.Play();
        AudioListener.pause = false;
        // pausePage.gameObject.SetActive(false);
    }
}
