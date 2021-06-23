using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownController : MonoBehaviour
{
    public int countdownTime;
    public Text countdownDisplay;
    public int bpm;
    public GameObject circle;
    private float secPerBeat;
    private int[] displayArr;
    private int displayIndex;
    IEnumerator CountdownToStart()
    {
        while (countdownTime > 0)
        {
            countdownDisplay.text = displayArr[displayIndex].ToString();
            yield return new WaitForSeconds(secPerBeat * 4.5f);
            displayIndex++;
            countdownTime--;
        }

        countdownDisplay.text = "GO!";

        yield return new WaitForSeconds(2.0f * secPerBeat);

        circle.gameObject.SetActive(true);

        countdownDisplay.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        secPerBeat = 60f / bpm;
        displayArr = new int[] { 3, 2, 1 };
        displayIndex = 0;
        StartCoroutine(CountdownToStart());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
