using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCountdownController : MonoBehaviour
{
    public int countdownTime;
    public Text countdownDisplay;
    public int bpm;
    public GameObject circle;
    private float secPerBeat;
    private int[] displayArr;
    private int displayIndex;

    private bool tutorialCompleted = false;
    private bool oneTime = false;

    IEnumerator CountdownToStart()
    {
        while (countdownTime > 0)
        {
            countdownDisplay.text = displayArr[displayIndex].ToString();
            yield return new WaitForSeconds(secPerBeat * 5.0f);
            displayIndex++;
            countdownTime--;
        }

        countdownDisplay.text = "GO!";

        yield return new WaitForSeconds(secPerBeat);

        circle.gameObject.SetActive(true);

        countdownDisplay.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {

    	countdownDisplay.text = "Welcome to our Tutorial Level!";
    	countdownDisplay.fontSize = 40;


    	/*
        secPerBeat = 60f / bpm;
        displayArr = new int[] { 3, 2, 1 };
        displayIndex = 0;
        StartCoroutine(CountdownToStart());
        */
    }

    // Update is called once per frame
    void Update()
    {
    	GameObject go = GameObject.FindGameObjectWithTag("Instruction");
        TutorialInstructionController tic = go.GetComponent<TutorialInstructionController>();
        this.tutorialCompleted = tic.tutorialCompleted;

        if (!oneTime && this.tutorialCompleted) {
        	secPerBeat = 60f / bpm;
        	displayArr = new int[] { 3, 2, 1 };
        	displayIndex = 0;
        	StartCoroutine(CountdownToStart());
        	oneTime = true;
        }

    }
}
