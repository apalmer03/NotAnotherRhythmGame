using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTimer : MonoBehaviour
{
    public float timeVal;
    public Text timeText;
    public AudioSource musicTrack;
    public AudioClip musicClip;

    private bool tutorialCompleted = false;
    private bool oneTime = false;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    	GameObject go = GameObject.FindGameObjectWithTag("Instruction");
        TutorialInstructionController tic = go.GetComponent<TutorialInstructionController>();
        this.tutorialCompleted = tic.tutorialCompleted;

        if (!oneTime && this.tutorialCompleted) {
        	musicClip = musicTrack.clip;
        	timeVal = musicClip.length;
        	oneTime = true;
        }


        if (timeVal > 0)
        {
            timeVal = timeVal - Time.deltaTime;
        }
        else
        {
            timeVal = 0;
        }

        printTime(timeVal);
    }

    void printTime(float displayTime)
    {
        if(displayTime < 0)
        {
            displayTime = 0;
        }

        float minutes = Mathf.FloorToInt(displayTime / 60);
        float seconds = Mathf.FloorToInt(displayTime % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
