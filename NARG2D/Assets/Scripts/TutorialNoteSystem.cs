using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialNoteSystem : MonoBehaviour
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
    private GameObject enemy;
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
    float songLength = float.MaxValue;
    public int currentBeat = 0;

    float marginOfError = 0.3f;

    private IEnumerator coroutine;

    public object[] actions;
    public GameObject gameOver;


    private bool tutorialCompleted = false;
    private bool oneTime = false;

    // Start is called before the first frame update
    void Start()
    {
    	Debug.Log("BPM is:" + bpm);
        secPerBeat = 60f / bpm;
        noteRingPos = new Vector2(0f, 0f);
        GameObject circle = GameObject.FindGameObjectWithTag("Circle");
        circleRenderer = circle.GetComponent<Renderer>();

        // Load the AudioSource attached to the Conductor GameObject

        // Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;
        songLength = (float)musicSource.clip.length;
        // Initialize notes array
        beats = new float[265];
        actions = new object[265];
        for (var i = 0; i < 265; i++)
        {
            beats[i] = i;
            if (i % 8 == 0 && i != 0)
            {
                actions[i] = ActionNote.Action.Attack;
            }
            else
            {
                actions[i] = ActionNote.Action.Idle;
            }


        }

        beatsShownInAdvance = 1.0f / secPerBeat;
        currentBeat = 18;
        nextIndex = 19;
        player = GameObject.Find("Player");
        enemy = GameObject.Find("Enemy");
        playerHealth = player.GetComponent<Health>();
        // Start the music
        musicSource.Play();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    	GameObject go = GameObject.FindGameObjectWithTag("Instruction");
        TutorialInstructionController tic = go.GetComponent<TutorialInstructionController>();
        this.tutorialCompleted = tic.tutorialCompleted;

        /*
        if(!this.oneTime && this.tutorialCompleted) {
        	Debug.Log("BPM is:" + bpm);
	        secPerBeat = 60f / bpm;
	        noteRingPos = new Vector2(0f, 0f);
	        GameObject circle = GameObject.FindGameObjectWithTag("Circle");
	        circleRenderer = circle.GetComponent<Renderer>();

	        // Load the AudioSource attached to the Conductor GameObject
	        // musicSource = GetComponent<AudioSource>();

	        // Record the time when the music starts
	        dspSongTime = (float)AudioSettings.dspTime;
	        songLength = (float)musicSource.clip.length;
	        // Initialize notes array
	        beats = new float[265];
	        actions = new object[265];
	        for (var i = 0; i < 265; i++)
	        {
	            beats[i] = i;
	            if (i % 8 == 0 && i != 0)
	            {
	                actions[i] = ActionNote.Action.Attack;
	            }
	            else
	            {
	                actions[i] = ActionNote.Action.Idle;
	            }
	        }

	        beatsShownInAdvance = 1.0f / secPerBeat;
	        currentBeat = 18;
	        nextIndex = 19;
	        player = GameObject.Find("Player");
	        enemy = GameObject.Find("Enemy");
	        playerHealth = player.GetComponent<Health>();
	        // Start the music
	        musicSource.Play();
	        gameObject.SetActive(false);
	        this.oneTime = true;
        }
        */

        if(this.tutorialCompleted) {
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
	            GameObject.Find("CurrentBeat").GetComponent<Text>().text = string.Format("{0}/{1}", (int)songPositionInBeats, (int)songLength / secPerBeat);
	            SpawnNote();
	            if ((ActionNote.Action)actions[nextIndex] != ActionNote.Action.Idle)
	            {
	                SpawnActionNote((ActionNote.Action)actions[nextIndex]);
	                Debug.Log(string.Format("Execute at song(beat) number {0}",songPositionInBeats));
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
    }

    private void SpawnNote()
    {
        Note noteRing = Instantiate(note, noteRingPos, Quaternion.identity);
        noteRing.transform.parent = GameObject.Find("NoteSystem").transform;
        noteRing.duration = 1.0f;
    }

    private void SpawnActionNote(ActionNote.Action action)
    {
        enemy.GetComponent<TutorialEnemyController>().doAction((ActionNote.Action)actions[nextIndex]);

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
