using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    public float timeVal;
    public Text timeText;
    public AudioSource musicTrack;
    public AudioClip musicClip;

    void Start()
    {
        musicTrack = GetComponent<AudioSource>();
        musicClip = musicTrack.clip;
        timeVal = musicClip.length;
    }

    // Update is called once per frame
    void Update()
    {
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
