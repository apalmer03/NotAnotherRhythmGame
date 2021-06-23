using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteSystem : MonoBehaviour
{

    public int bpm;
    public float speed;
    private float beatRate;
    public Note note;
    public ActionNote actionNote;
    private Renderer circleRenderer;
    // private Vector2 leftPos;
    // private Vector2 rightPos;
    private Vector2 noteRingPos;
    private GameObject player;
    private Health playerHealth;
    public Text comboText;
    private int comboNum = 0;
    public GameObject missText;
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

    public object[] actions;
    public GameObject gameOver;

    // Start is called before the first frame update
    void Start()
    {
        secPerBeat = 60f / bpm;
        noteRingPos = new Vector2(0f, 0f);
        GameObject circle = GameObject.FindGameObjectWithTag("Circle");
        circleRenderer = circle.GetComponent<Renderer>();

        // Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();

        // Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        // Initialize notes array
        beats = new float[200];
        actions = new object[200];
        for (var i = 0; i < 200; i++)
        {

            //float[] beatpattern = { 0.0f, 1.0f, 2.5f, 3.0f, 3.5f };



            ////actions[i] = (Note.BeatAction)(i % 4);
            //beats[i] = 4 * (i / 5) + beatpattern[i % 5] + 3.5f;
            //if (i % 5 == 0)
            //{
            //    actions[i] = Note.BeatAction.Block;
            //}
            //else if (i % 5 == 1)
            //{
            //    actions[i] = Note.BeatAction.Jump;
            //}
            //else if (i % 5 == 2)
            //{
            //    actions[i] = Note.BeatAction.Block;
            //}
            //else if (i % 5 == 3)
            //{
            //    actions[i] = Note.BeatAction.Block;
            //}
            //else if (i % 5 == 4)
            //{
            //    actions[i] = Note.BeatAction.Block;
            //}
            beats[i] = i + 5;
            if (i % 4 == 0 && i != 0)
            {
                actions[i] = ActionNote.Action.Attack;
            }
            else
            {
                actions[i] = ActionNote.Action.Idle;
            }
        }

        beatsShownInAdvance = 1.0f / secPerBeat;
        currentBeat = 13;
        nextIndex = 14;
        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<Health>();
        // Start the music
        musicSource.Play();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
        // determine how many beats since the song started
        songPositionInBeats = songPosition / secPerBeat;

        // check if music ends
        if (songPosition >= musicSource.clip.length)
        {
            Time.timeScale = 0;
            gameOver.SetActive(true);
        }

        if (nextIndex < beats.Length && beats[nextIndex] < songPositionInBeats + beatsShownInAdvance)
        {
            SpawnNote();
            if ((ActionNote.Action)actions[nextIndex] != ActionNote.Action.Idle)
            {
                SpawnActionNote((ActionNote.Action)actions[nextIndex]);
            }
            // enemy.GetComponent<EnemyController>().doAction((Note.BeatAction)actions[nextIndex]);
            //initialize the fields of the music note
            nextIndex++;
        }
        if (Input.anyKeyDown && (!(Input.GetKeyDown(KeyCode.Keypad0) | Input.GetKeyDown(KeyCode.KeypadPeriod) | Input.GetKeyDown(KeyCode.KeypadEnter) | Input.GetKeyDown(KeyCode.Keypad3))))
        {
            float err = Mathf.Abs(songPositionInBeats - beats[currentBeat]);
            // check if hit on beat
            if (err <= marginOfError)
            {
                missText.SetActive(false);
                comboNum++;
                comboText.text = "Combo x " + comboNum.ToString();
                if (comboNum == 2)
                {
                    comboText.gameObject.SetActive(true);
                }
                coroutine = ChangeColor(0.3f, Color.green);
                StartCoroutine(coroutine);
            }
            // check if not on beat
            else
            {
                coroutine = ChangeColor(0.3f, Color.red);
                StartCoroutine(coroutine);
                IEnumerator showMissText = showMiss(0.3f);
                StartCoroutine(showMissText);
                comboNum = 0;
                comboText.gameObject.SetActive(false);
                GameObject[] notes = GameObject.FindGameObjectsWithTag("Note");
                if (notes.Length >= 1)
                {
                    Destroy(notes[0].gameObject);
                    currentBeat++;
                    playerHealth.DamagePlayer(10);
                    Debug.Log("off-beat penalty\n");
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
        Note noteRing = Instantiate(note, noteRingPos, Quaternion.identity);
        noteRing.duration = 1.0f;
    }

    private void SpawnActionNote(ActionNote.Action action)
    {
        // ActionNote leftNote = Instantiate(actionNote, leftPos, Quaternion.identity);
        // leftNote.speed = speed;
        // leftNote.lifeSpan = Mathf.Abs(leftPos.x / speed);
        // leftNote.action = action;
        // ActionNote rightNote = Instantiate(actionNote, rightPos, Quaternion.identity);
        // rightNote.speed = -speed;
        // rightNote.lifeSpan = Mathf.Abs(rightPos.x / speed);
        // rightNote.action = action;
    }

    private IEnumerator ChangeColor(float waitTime, Color col)
    {
        circleRenderer.material.SetColor("_Color", col);
        yield return new WaitForSeconds(waitTime);
        circleRenderer.material.SetColor("_Color", Color.white);
    }

    private IEnumerator showMiss(float waitTime)
    {
        missText.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        missText.SetActive(false);
    }
}
