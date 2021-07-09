using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class NoteSystem : MonoBehaviour
{

    public int bpm;
    public float speed;
    private float beatRate;
    public Note note;

    public UltimateNote ultUp,
        ultDown,
        ultLeft,
        ultRight,
        ultUpDown,
        ultUpLeft,
        ultUpRight,
        ultLeftRight,
        ultDownLeft,
        ultDownRight;
    public ActionNote actionNote;
    private Renderer circleRenderer;
    private Vector2 noteRingPos;
    private GameObject player;
    private GameObject enemy;
    private Health enemyHealth;
    private Health playerHealth;
    public Text comboText;
    public Text scoreText;
    private int comboNum = 0;
    private int missNum = 0;
    private int hitNum = 0;
    public int totalscore = 0;
    public GameObject missText;
    public GameObject perfectText;
    public GameObject goodText;
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

    float marginOfError = 0.35f;
    float colorMargin = 0.7f;
    public List<int> actions_list;
    public int[][] attack_pattern;
    private IEnumerator pressedCoroutine;
    private IEnumerator onBeatCoroutine;
    public object[] actions;
    public GameObject gameOver;
    public GameObject outerRing;
    public int multiplier = 1;
    public UltimateScroller ultimateScroller;
    public int damageUltGoodNote = 10;
    public int damageUltPerfectNote = 20;
    public static NoteSystem instance;
    private Ultimate playerUltimate;
    private bool ultFlag;

    public static readonly int RATIO_CHANCE_UP = 75;
    public static readonly int RATIO_CHANCE_DOWN = 75;
    public static readonly int RATIO_CHANCE_LEFT = 75;
    public static readonly int RATIO_CHANCE_RIGHT = 75;
    public static readonly int RATIO_CHANCE_UPDOWN = 10;
    public static readonly int RATIO_CHANCE_UPLEFT = 10;
    public static readonly int RATIO_CHANCE_UPRIGHT = 10;
    public static readonly int RATIO_CHANCE_LEFTRIGHT = 10;
    public static readonly int RATIO_CHANCE_DOWNLEFT = 10;
    public static readonly int RATIO_CHANCE_DOWNRIGHT = 10;

    public static readonly int RATIO_TOTAL = RATIO_CHANCE_UP
                                             + RATIO_CHANCE_DOWN
                                             + RATIO_CHANCE_LEFT
                                             + RATIO_CHANCE_RIGHT
                                             + RATIO_CHANCE_UPDOWN
                                             + RATIO_CHANCE_UPLEFT
                                             + RATIO_CHANCE_UPRIGHT
                                             + RATIO_CHANCE_LEFTRIGHT
                                             + RATIO_CHANCE_DOWNLEFT
                                             + RATIO_CHANCE_DOWNRIGHT;
    private UltimateNote ultNote;
    private Queue<int> ultAction = new Queue<int>();
    public float ultTimer = 10f;
    public float ultDuration;
    private Renderer outerRingRenderer;
    private Queue<Note> noteRing = new Queue<Note>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        secPerBeat = 60f / bpm;
        noteRingPos = new Vector2(0f, 0f);
        GameObject circle = GameObject.FindGameObjectWithTag("Circle");
        circleRenderer = circle.GetComponent<Renderer>();
        outerRingRenderer = outerRing.GetComponent<Renderer>();
        // Load the AudioSource attached to the Conductor GameObject

        // Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;
        songLength = (float)musicSource.clip.length;
        // Initialize notes array
        beats = new float[265];
        actions = new object[265];
        attack_pattern = new int[8][];
        attack_pattern[0] = new int[] { 4, 4, 4, 4, 4, 4, 4, 4 };
        attack_pattern[1] = new int[] { 4, 4, 5, 4, 1, 4, 4, 4 };
        attack_pattern[2] = new int[] { 4, 4, 4, 4, 6, 4, 2, 4 };
        attack_pattern[3] = new int[] { 5, 4, 1, 4, 4, 6, 4, 2 };
        ultDuration = ultTimer;


        for (var i = 0; i < 133; i++)
        {
            beats[i] = 2 * i;

        }
        for (var p = 0; p < 133 / 8; p++)
        {
            //actions_list.AddRange(attack_pattern[patt]);
            if (p < 4)
            {
                actions_list.AddRange(attack_pattern[0]);
            }
            else
            {
                var patt = Random.Range(1, 3);
                actions_list.AddRange(attack_pattern[patt]);
            }
        }
        for (var i = 0; i < actions_list.ToArray().Length && i < actions.Length; i++)
        {
            actions[i] = (ActionNote.Action)actions_list.ToArray()[i];
        }
        beatsShownInAdvance = 1.0f / secPerBeat;
        // currentBeat = 9;
        // nextIndex = 10;
        player = GameObject.Find("Player");
        enemy = GameObject.Find("Enemy");
        enemyHealth = enemy.GetComponent<Health>();
        playerHealth = player.GetComponent<Health>();
        playerUltimate = player.GetComponent<Ultimate>();
        ultFlag = false;
        // Start the music
        musicSource.Play();
        // gameObject.SetActive(false);
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
            scoreText.text = "Total Score: " + totalscore.ToString();
            scoreText.gameObject.SetActive(true);
            gameOver.SetActive(true);
        }

        if (ultFlag)
        {
            ultDuration = ultDuration - Time.deltaTime;
        }
        if (ultDuration <= 0)
        {
            ultDuration = ultTimer;
            ultFlag = false;
            totalscore = totalscore + 500;
        }

        if (nextIndex < beats.Length && beats[nextIndex] < songPositionInBeats + beatsShownInAdvance)
        {
            GameObject.Find("CurrentBeat").GetComponent<Text>().text = string.Format("{0}/{1}", (int)songPositionInBeats, (int)songLength / secPerBeat);
            if (!ultFlag)
            {
                SpawnNote();
            }
            else
            {
                SpawnUltNote();
            }
            if ((ActionNote.Action)actions[nextIndex] != ActionNote.Action.Idle && !ultFlag)
            {
                SpawnActionNote((ActionNote.Action)actions[nextIndex]);
                // Debug.Log(string.Format("Execute at song(beat) number {0}", songPositionInBeats));
            }
            //initialize the fields of the music note
            nextIndex++;
        }

        updateNote();

        float err = songPositionInBeats - beats[currentBeat];
        // show on beat indicator
        if (noteRing.Count != 0)
        {
            Note topRing = noteRing.Peek();
            if (Mathf.Abs(err) <= colorMargin && err <= 0 && topRing.firstOnBeat)
            {
                topRing.onBeatState = true;
            }
        }
        if (Input.anyKeyDown && (!(Input.GetKeyDown(KeyCode.Keypad0) | Input.GetKeyDown(KeyCode.KeypadPeriod) | Input.GetKeyDown(KeyCode.KeypadEnter) | Input.GetKeyDown(KeyCode.Keypad3))))
        {
            // check if hit on beat
            if (Mathf.Abs(err) <= marginOfError)
            {
                // check note hit score
                checkNoteHit();
                if (!ultFlag)
                {
                    player.gameObject.GetComponent<MainCharacterController>().doAction();
                }
                else
                {
                    player.gameObject.GetComponent<MainCharacterController>().doUltAction(ultAction.Peek());
                }
                missText.SetActive(false);
                comboNum++;
                comboText.text = "Combo x " + comboNum.ToString();
                if (comboNum == 2)
                {
                    comboText.gameObject.SetActive(true);
                }

                if (comboNum >= 70)
                {
                    multiplier = 6;
                }
                else if (comboNum >= 52)
                {
                    multiplier = 5;
                }
                else if (comboNum >= 36)
                {
                    multiplier = 4;
                }
                else if (comboNum >= 22)
                {
                    multiplier = 3;
                }
                else if (comboNum >= 10)
                {
                    multiplier = 2;
                }
                else
                {
                    multiplier = 1;
                }
                totalscore = totalscore + (10 * multiplier);
                scoreText.text = "Total Score: " + totalscore.ToString();
                AnalyticsResult analytics_comboCounter = Analytics.CustomEvent("Combo Length: " + comboNum);
                Debug.Log("Analytics result " + analytics_comboCounter);
                AnalyticsResult analytics_hitCounter = Analytics.CustomEvent("Combo Length: " + hitNum++);
                Debug.Log("Analytics result " + analytics_hitCounter);
            }
            // check if not on beat
            else
            {
                perfectText.SetActive(false);
                goodText.SetActive(false);
                playerUltimate.resetBar();
                pressedCoroutine = ChangeColor(0.3f, Color.red);
                StartCoroutine(pressedCoroutine);
                IEnumerator showMissText = showText(0.3f, missText);
                StartCoroutine(showMissText);
                comboNum = 0;
                comboText.gameObject.SetActive(false);
                AnalyticsResult analytics_missCounter = Analytics.CustomEvent("Miss Counter: " + missNum++);
                Debug.Log("Analytics result" + analytics_missCounter);
            }
            // destroy ring whether hit or miss
            destroyNote();
        }


        // update current beat if passed
        if (songPositionInBeats > beats[currentBeat] + marginOfError)
        {
            currentBeat++;
        }

    }

    private void SpawnNote()
    {
        Note ring = Instantiate(note, noteRingPos, Quaternion.identity);
        ring.transform.parent = GameObject.Find("NoteSystem").transform;
        ring.duration = 1.0f;
        noteRing.Enqueue(ring);
    }

    private void updateNote()
    {
        if (noteRing.Count != 0)
        {
            Note top = noteRing.Peek();
            // dequeue and destroy if note scale is down 
            if (top.i >= 1.0f)
            {
                destroyNote();
            }
        }
    }

    private void destroyNote()
    {
        if (noteRing.Count != 0)
        {
            Note top = noteRing.Dequeue();
            Destroy(top.gameObject);
        }
    }

    private void checkNoteHit()
    {
        if (noteRing.Count != 0)
        {
            Note top = noteRing.Peek();
            float scale = top.transform.localScale.x;
            IEnumerator displayScoreText;
            if (scale <= 0.44f && scale >= 0.24f)
            {
                displayScoreText = showText(0.3f, perfectText);
                pressedCoroutine = ChangeColor(0.3f, Color.green);
            }
            else
            {
                displayScoreText = showText(0.3f, goodText);
                pressedCoroutine = ChangeColor(0.3f, Color.yellow);
            }
            StartCoroutine(displayScoreText);
            StartCoroutine(pressedCoroutine);
        }
    }

    private void SpawnUltNote()
    {
        System.Random random = new System.Random();
        int x = random.Next(0, RATIO_TOTAL);
        if ((x -= RATIO_CHANCE_UP) < 0) // Test for A
        {
            ultNote = Instantiate(ultUp, noteRingPos, Quaternion.identity);
            ultAction.Enqueue(0);
        }
        else if ((x -= RATIO_CHANCE_DOWN) < 0) // Test for B
        {
            ultNote = Instantiate(ultDown, noteRingPos, Quaternion.identity);
            ultAction.Enqueue(1);
        }
        else if ((x -= RATIO_CHANCE_LEFT) < 0) // Test for A
        {
            ultNote = Instantiate(ultLeft, noteRingPos, Quaternion.identity);
            ultAction.Enqueue(2);
        }
        else if ((x -= RATIO_CHANCE_RIGHT) < 0) // Test for B
        {
            ultNote = Instantiate(ultRight, noteRingPos, Quaternion.identity);
            ultAction.Enqueue(3);
        }
        else if ((x -= RATIO_CHANCE_UPDOWN) < 0) // Test for A
        {
            ultNote = Instantiate(ultUpDown, noteRingPos, Quaternion.identity);
            ultAction.Enqueue(4);
        }
        else if ((x -= RATIO_CHANCE_UPLEFT) < 0) // Test for B
        {
            ultNote = Instantiate(ultUpLeft, noteRingPos, Quaternion.identity);
            ultAction.Enqueue(5);
        }
        else if ((x -= RATIO_CHANCE_UPRIGHT) < 0) // Test for A
        {
            ultNote = Instantiate(ultUpRight, noteRingPos, Quaternion.identity);
            ultAction.Enqueue(6);
        }
        else if ((x -= RATIO_CHANCE_LEFTRIGHT) < 0) // Test for B
        {
            ultNote = Instantiate(ultLeftRight, noteRingPos, Quaternion.identity);
            ultAction.Enqueue(7);
        }
        else if ((x -= RATIO_CHANCE_DOWNLEFT) < 0) // Test for A
        {
            ultNote = Instantiate(ultDownLeft, noteRingPos, Quaternion.identity);
            ultAction.Enqueue(8);
        }
        else // No need for final if statement
        {
            ultNote = Instantiate(ultDownRight, noteRingPos, Quaternion.identity);
            ultAction.Enqueue(9);
        }
        ultNote.GetComponent<UltimateNote>().SetNote(instance);
        ultNote.transform.parent = GameObject.Find("NoteSystem").transform;
        ultNote.duration = 1.0f;
    }

    private void SpawnActionNote(ActionNote.Action action)
    {
        enemy.GetComponent<EnemyController>().doAction((ActionNote.Action)actions[nextIndex]);
    }

    private IEnumerator ChangeColor(float waitTime, Color col)
    {
        outerRingRenderer.material.SetColor("_Color", col);
        circleRenderer.material.SetColor("_Color", col);
        yield return new WaitForSeconds(waitTime);
        circleRenderer.material.SetColor("_Color", Color.white);
        outerRingRenderer.material.SetColor("_Color", Color.white);
    }

    private IEnumerator showText(float waitTime, GameObject text)
    {
        text.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        text.SetActive(false);
    }

    public int GetMultiplier()
    {
        return multiplier;
    }

    public int GetTotal()
    {
        return totalscore;
    }

    public void UltNoteHit(int damage)
    {
        enemyHealth.DamagePlayer(damage);
        Debug.Log("Hit On Time");
    }

    public void UltNoteMissed()
    {
        Debug.Log("Missed Note");
    }

    public void UltNormalHit()
    {
        UltNoteHit(1);
    }

    public void UltGoodHit()
    {
        UltNoteHit(1);
    }
    public void UltPerfectHit()
    {
        UltNoteHit(1);
    }

    public void ActivateUlt()
    {
        ultFlag = true;
    }

    public bool IsUltActive()
    {
        return ultFlag;
    }

    public int GetUltAction()
    {
        return ultAction.Peek();
    }

    public void DestroyUltAction()
    {
        ultAction.Dequeue();
    }
}
