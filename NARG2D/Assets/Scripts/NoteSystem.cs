using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSystem : MonoBehaviour
{
  public int bpm;
  public float speed;
  private float beatRate;
  public Note note;
  private Renderer circleRenderer;
  public GameObject capsule;
  private Vector2 leftPos;
  private Vector2 rightPos;
  private bool collision = false;

  // The number of seconds for each song beat
  public float secPerBeat;

  // Current song position, in seconds
  public float songPosition;

  // Current song position, in beats
  public float songPositionInBeats;

  // How many seconds have passed since the song started
  public float dspSongTime;

  // an AudioSource attached to this GameObject that will play the music.
  public AudioSource musicSource;

  // The offset to the first beat of the song in seconds
  public float firstBeatOffset;

  // keep all the position-in-beats of notes in the song
  float[] beats;

  // the index of the next note to be spawned
  int nextIndex = 0;

  float beatsShownInAdvance = 0;

  public int currentBeat = 0;

  float marginOfError = 0.3f;

  private IEnumerator coroutine;

  // Start is called before the first frame update
  void Start()
  {
    secPerBeat = 60f / bpm;
    leftPos = new Vector2(-9f, -3f);
    rightPos = new Vector2(9f, -3f);
    GameObject circle = GameObject.FindGameObjectWithTag("Circle");
    circleRenderer = circle.GetComponent<Renderer>();
    // InvokeRepeating("SpawnNote", 0f, secPerBeat);

    // Load the AudioSource attached to the Conductor GameObject
    musicSource = GetComponent<AudioSource>();

    // Record the time when the music starts
    dspSongTime = (float)AudioSettings.dspTime;

    // Initialize notes array
    beats = new float[200];
    for (var i = 0; i < 200; i++)
    {
      beats[i] = i + 5;
    }

    beatsShownInAdvance = Mathf.Abs(leftPos.x / speed) / secPerBeat;

    // Start the music
    musicSource.Play();
  }

  // Update is called once per frame
  void Update()
  {
    // determine how many seconds since the song started
    songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
    // determine how many beats since the song started
    songPositionInBeats = songPosition / secPerBeat;

    if (nextIndex < beats.Length && beats[nextIndex] < songPositionInBeats + beatsShownInAdvance)
    {
      SpawnNote();
      //initialize the fields of the music note
      nextIndex++;
    }
    if (Input.anyKeyDown)
    {
      float err = Mathf.Abs(songPositionInBeats - beats[currentBeat]);
      // check if hit on beat
      if (err <= marginOfError)
      {
        coroutine = ChangeColor(0.3f);
        StartCoroutine(coroutine);
      }
      // check if not on beat
      else
      {
        GameObject[] notes = GameObject.FindGameObjectsWithTag("Note");
        if (notes.Length >= 2)
        {
          Destroy(notes[0].gameObject);
          Destroy(notes[1].gameObject);
          currentBeat++;
        }
      }
    }
    // update current beat if passed
    if (songPositionInBeats > beats[currentBeat] + marginOfError)
    {
      currentBeat++;
    }
  }

  private void SpawnNote()
  {
    Note leftNote = Instantiate(note, leftPos, Quaternion.identity);
    leftNote.speed = speed;
    leftNote.lifeSpan = Mathf.Abs(leftPos.x / speed);
    Note rightNote = Instantiate(note, rightPos, Quaternion.identity);
    rightNote.speed = -speed;
    rightNote.lifeSpan = Mathf.Abs(rightPos.x / speed);
  }
  private IEnumerator ChangeColor(float waitTime)
  {
    circleRenderer.material.SetColor("_Color", Color.green);
    yield return new WaitForSeconds(waitTime);
    circleRenderer.material.SetColor("_Color", Color.white);
  }
}
