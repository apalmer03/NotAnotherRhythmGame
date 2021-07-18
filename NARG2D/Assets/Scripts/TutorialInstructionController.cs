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

    // Start is called before the first frame update
    void Start()
    {
    	instructionsArray = new string[] {"Welcome to our Tutorial Level! Let us show you how to play our game!\nPress 'n' to continue.", 
    	"You control the character on the left (\"Hero\"). The enemy is the character on the right (\"Boss\"). Your goal is to defeat the boss. Next, we'll go over how gameplay works.\nPress 'n' to continue.", 
    	"Your Hero can jump, attack, and block. You must perform your actions on the beat (click key when beat circle perfectly overlaps center circle) or take penalty damage. As you hit consecutive beats perfectly, you gain a combo multiplier and deal more damage. We'll go over the UI next.\nPress 'n' to continue.", 
    	"Your Hero's health bar and the Boss's health bars are above the characters. The center timer indicates how much time remaining in the current level (remaining length of song). Next, we'll go over basic controls.\nPress 'n' to continue.",
    	"Press your 'A' key for a light attack and 'S' key for an uppercut. Press your 'Space' key to jump. Press your 'D' key to block. Press 'n' to see advanced controls.",
        "Special Move #1: Jump + A + A (Blue Particle Effect). Special Move #2: A + S + A (Purple Particle Effect). Ultimate: Ult bar on left fills up (turn red) and press H. Try them out!\nWhen you're ready, start the tutorial level by pressing 'n'. Good luck and have fun! :)"};
    	
    	currInstructionIndex = 0;
    	instructionDisplay.text = instructionsArray[currInstructionIndex];
    	instructionDisplay.fontSize = 20;        

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
        		instructionDisplay.text = "";
        		tutorialCompleted = true;
        	}

        }
    }
}
