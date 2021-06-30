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
    public GameObject player;
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
            yield return new WaitForSeconds(1.0f);
            displayIndex++;
            countdownTime--;
        }

        countdownDisplay.text = "GO!";

        yield return new WaitForSeconds(secPerBeat);

        circle.gameObject.SetActive(true);
        player.gameObject.GetComponent<TutorialMainCharacterController>().enabled = true;
        countdownDisplay.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
    	countdownDisplay.text = "Welcome to our Tutorial Level!";
    	countdownDisplay.fontSize = 40;
    }

    // Update is called once per frame
    void Update()
    {
    	GameObject go = GameObject.FindGameObjectWithTag("Instruction");
        TutorialInstructionController tic = go.GetComponent<TutorialInstructionController>();
        this.tutorialCompleted = tic.tutorialCompleted;

        if (!oneTime && this.tutorialCompleted) {
        	countdownDisplay.text = "";
        	secPerBeat = 60f / bpm;
        	displayArr = new int[] { 3, 2, 1 };
        	displayIndex = 0;
        	StartCoroutine(CountdownToStart());
        	oneTime = true;
        }

    }
}
