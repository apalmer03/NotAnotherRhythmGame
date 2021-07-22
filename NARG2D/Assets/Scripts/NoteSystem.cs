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
    public Text multiplierText;
    public Text scoreText;
    private int comboNum = 0;
    private int ultMultiplier = 1;
    private int missNum = 0;
    private int hitNum = 0;
    public int totalscore = 0;
    public GameObject missText;
    public GameObject perfectText;
    public GameObject goodText;
    public GameObject ultFireBackground;
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
    float[] ultbeats;


    // the index of the next note to be spawned
    int nextIndex = 0;
    private int ultNextIndex = 0;

    public float beatsShownInAdvance = 0;
    float songLength = float.MaxValue;
    public int currentBeat = 0;
    public int ultCurrentBeat = 0;

    float marginOfError = 0.4f;
    float colorMargin = 0.5f;
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
    public float ultTimer = 1000f;
    public float ultDuration;
    private Renderer outerRingRenderer;
    private Queue<Note> noteRing = new Queue<Note>();
    private Queue<UltimateNote> ultNoteRing = new Queue<UltimateNote>();
    public GameObject rings;
    public bool gameStarted = false;

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
        ultbeats = new float[265];
        actions = new object[265];
        attack_pattern = new int[12][];
        attack_pattern[0] = new int[] { 4, 4, 4, 4, 4, 4, 4, 4 };

        attack_pattern[1] = new int[] { 4, 4, 5, 1, 4, 4, 4, 4 };

        attack_pattern[2] = new int[] { 4, 4, 5, 1, 5, 1, 4, 4 };

        attack_pattern[3] = new int[] { 4, 4, 4, 4, 6, 2, 4, 4 };

        attack_pattern[4] = new int[] { 5, 1, 4, 4, 6, 2, 4, 4 };
        attack_pattern[5] = new int[] { 6, 2, 4, 4, 3, 3, 5, 1 };

        attack_pattern[6] = new int[] { 6, 2, 4, 3, 6, 2, 4, 3 };
        attack_pattern[7] = new int[] { 5, 1, 4, 3, 5, 1, 4, 3 };

        attack_pattern[8] = new int[] { 5, 1, 4, 4, 6, 2, 4, 4 };
        attack_pattern[9] = new int[] { 5, 1, 5, 1, 4, 4, 3, 3 };
        attack_pattern[10] = new int[] { 6, 2, 6, 2, 4, 4, 3, 3 };

        attack_pattern[11] = new int[] { 5, 1, 5, 1, 6, 2, 6, 2 };

        ultDuration = ultTimer;


        for (var i = 0; i < 133; i++)
        {
            beats[i] = 2 * i;
        }

        for (var i = 0; i < 265; i++)
        {
            ultbeats[i] = i;
        }

        for (var p = 0; p < (133 / 8) + 1; p++)
        {
            if (p < 2)
            {
                actions_list.AddRange(attack_pattern[0]);
            }
            else if (p < 5)
            {
                actions_list.AddRange(attack_pattern[p - 1]);
            }
            else if (p <= 6)
            {
                actions_list.AddRange(attack_pattern[8]);
            }
            else if (p <= 8)
            {
                var patt = Random.Range(6, 10);
                actions_list.AddRange(attack_pattern[patt]);
            }
            else if (p <= 9)
            {
                var patt = Random.Range(9, 10);
                actions_list.AddRange(attack_pattern[patt]);
            }
            //final showdown
            else if (p == 10)
            {
                actions_list.AddRange(attack_pattern[11]);
            }
            else
            {
                var patt = Random.Range(2, 11); ;
                actions_list.AddRange(attack_pattern[patt]);
            }
        }
        for (var i = 0; i < actions_list.ToArray().Length && i < actions.Length; i++)
        {
            actions[i] = (ActionNote.Action)actions_list.ToArray()[i];
        }
        // beatsShownInAdvance = 1.0f;
        currentBeat = 7;
        ultCurrentBeat = 7;
        nextIndex = 8;
        ultNextIndex = 8;
        player = GameObject.Find("Player");
        enemy = GameObject.Find("Enemy");
        enemyHealth = enemy.GetComponent<Health>();
        playerHealth = player.GetComponent<Health>();
        playerUltimate = player.GetComponent<Ultimate>();
        ultFlag = false;
        // Start the music
        musicSource.Play();
        rings.gameObject.SetActive(false);
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

        if (!ultFlag)
        {
            if (nextIndex < beats.Length && beats[nextIndex] < songPositionInBeats + beatsShownInAdvance)
            {
                // GameObject.FindWithTag("Player").GetComponent<MainCharacterController>().soundFX[3].Play();
                // GameObject.FindWithTag("Player").GetComponent<MainCharacterController>().doAction((int)actions[nextIndex]);
                GameObject.Find("CurrentBeat").GetComponent<Text>().text = string.Format("{0}/{1}", (int)songPositionInBeats, (int)songLength / secPerBeat);
                SpawnNote();
                if ((ActionNote.Action)actions[nextIndex] != ActionNote.Action.Idle && !ultFlag)
                {
                    SpawnActionNote((ActionNote.Action)actions[nextIndex]);
                    //Debug.Log(string.Format("Execute at song(beat) number {0}", songPositionInBeats));
                }
                //initialize the fields of the music note
                nextIndex++;
                ultNextIndex = ultNextIndex + 2;

            }
        }
        else
        {
            if (ultNextIndex < ultbeats.Length && ultbeats[ultNextIndex] < songPositionInBeats + beatsShownInAdvance)
            {
                // GameObject.FindWithTag("Player").GetComponent<MainCharacterController>().soundFX[3].Play();
                // GameObject.FindWithTag("Player").GetComponent<MainCharacterController>().doAction((int)actions[nextIndex]);
                GameObject.Find("CurrentBeat").GetComponent<Text>().text = string.Format("{0}/{1}", (int)songPositionInBeats, (int)songLength / secPerBeat);
                SpawnUltNote();
                //initialize the fields of the music note
                ultNextIndex++;
                nextIndex = ultNextIndex / 2;

            }
        }

        updateNote();
        updateUltNote();

        float err = songPositionInBeats - beats[currentBeat];
        float ultErr = songPositionInBeats - ultbeats[ultCurrentBeat];
        // show on beat indicator
        if (noteRing.Count != 0)
        {
            Note topRing = noteRing.Peek();
            if (Mathf.Abs(err) <= colorMargin && err <= 0 && topRing.firstOnBeat)
            {
                topRing.onBeatState = true;
            }
        }
        if (Input.anyKeyDown && (!(Input.GetKeyDown(KeyCode.Keypad0) | Input.GetKeyDown(KeyCode.KeypadPeriod) | Input.GetKeyDown(KeyCode.KeypadEnter) | Input.GetKeyDown(KeyCode.Keypad3) | Input.GetKeyDown(KeyCode.Escape))) && gameStarted)
        {
            //Debug.Log("Error margin:" + err);
            // check if hit on beat
            if ((Mathf.Abs(err) <= marginOfError) && !ultFlag)
            {

                // Debug.Log();
                // check note hit score
                checkNoteHit();
                player.gameObject.GetComponent<MainCharacterController>().doAction();
                missText.SetActive(false);
                comboNum++;
                comboText.text = "Combo : " + comboNum.ToString();
                multiplierText.text = "Multiplier: x" + multiplier.ToString();

                if (comboNum == 2)
                {
                    comboText.gameObject.SetActive(true);
                }

                ultMultiplier = 1;
                ultFireBackground.SetActive(false);

                if (comboNum >= 234)
                {
                    multiplier = ultMultiplier * 10;
                }
                else if (comboNum >= 192)
                {
                    multiplier = ultMultiplier * 9;
                }
                else if (comboNum >= 154)
                {
                    multiplier = ultMultiplier * 8;
                }
                else if (comboNum >= 120)
                {
                    multiplier = ultMultiplier * 7;
                }
                else if (comboNum >= 90)
                {
                    multiplier = ultMultiplier * 6;
                }
                else if (comboNum >= 64)
                {
                    multiplier = ultMultiplier * 5;
                }
                else if (comboNum >= 42)
                {
                    multiplier = ultMultiplier * 4;
                }
                else if (comboNum >= 24)
                {
                    multiplier = ultMultiplier * 3;
                }
                else if (comboNum >= 10)
                {
                    multiplier = ultMultiplier * 2;
                }
                else
                {
                    multiplier = ultMultiplier * 1;
                }
                totalscore = totalscore + (10 * multiplier);
                scoreText.text = "Total Score : " + totalscore.ToString();
                AnalyticsResult analytics_comboCounter = Analytics.CustomEvent("Combo Length: " + comboNum);
                Debug.Log("Analytics result " + analytics_comboCounter);
                AnalyticsResult analytics_hitCounter = Analytics.CustomEvent("Combo Length: " + hitNum++);
                Debug.Log("Analytics result " + analytics_hitCounter);
            }
            else if ((Mathf.Abs(ultErr) <= marginOfError) && ultFlag)
            {
                if (ultNoteRing.Count != 0)
                {
                    UltimateNote ultTopRing = ultNoteRing.Peek();
                    if (Mathf.Abs(ultErr) <= colorMargin && ultErr <= 0 && ultTopRing.firstOnBeat)
                    {
                        ultTopRing.onBeatState = true;
                    }
                }
                // Debug.Log();
                // check note hit score
                checkUltNoteHit();
                if (ultAction.Count != 0)
                {
                    player.gameObject.GetComponent<MainCharacterController>().doUltAction(ultAction.Peek());
                }

                missText.SetActive(false);
                comboNum++;
                comboText.text = "Combo : " + comboNum.ToString();
                multiplierText.text = "Multiplier: x" + multiplier.ToString();

                if (comboNum == 2)
                {
                    comboText.gameObject.SetActive(true);
                }

                //Double multiplier during ultimate like in Guitar Hero
                if (ultFlag == true)
                {
                    ultMultiplier = 2;
                    ultFireBackground.SetActive(true);
                }
                else
                {
                    ultMultiplier = 1;
                    ultFireBackground.SetActive(false);
                }

                if (comboNum >= 234)
                {
                    multiplier = ultMultiplier * 10;
                }
                else if (comboNum >= 192)
                {
                    multiplier = ultMultiplier * 9;
                }
                else if (comboNum >= 154)
                {
                    multiplier = ultMultiplier * 8;
                }
                else if (comboNum >= 120)
                {
                    multiplier = ultMultiplier * 7;
                }
                else if (comboNum >= 90)
                {
                    multiplier = ultMultiplier * 6;
                }
                else if (comboNum >= 64)
                {
                    multiplier = ultMultiplier * 5;
                }
                else if (comboNum >= 42)
                {
                    multiplier = ultMultiplier * 4;
                }
                else if (comboNum >= 24)
                {
                    multiplier = ultMultiplier * 3;
                }
                else if (comboNum >= 10)
                {
                    multiplier = ultMultiplier * 2;
                }
                else
                {
                    multiplier = ultMultiplier * 1;
                }
                totalscore = totalscore + (10 * multiplier);
                scoreText.text = "Total Score : " + totalscore.ToString();
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
            destroyUltNote();
        }


        // update current beat if passed
        if (songPositionInBeats > beats[currentBeat] + marginOfError)
        {
            currentBeat++;
        }
        if (songPositionInBeats > ultbeats[ultCurrentBeat] + marginOfError)
        {
            ultCurrentBeat++;
        }

    }

    private void SpawnNote()
    {
        Note ring = Instantiate(note, noteRingPos, Quaternion.identity);
        ring.transform.parent = GameObject.Find("NoteSystem").transform;
        ring.duration = 2 * secPerBeat;
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
        if (noteRing.Count != 0 && noteRing.Peek().transform.localScale.x <= 0.55f)
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
                displayScoreText = showText(0.3f, goodText);
                pressedCoroutine = ChangeColor(0.3f, Color.yellow);
            }
            else
            {
                displayScoreText = showText(0.3f, perfectText);
                pressedCoroutine = ChangeColor(0.3f, Color.green);
            }
            StartCoroutine(displayScoreText);
            StartCoroutine(pressedCoroutine);
        }
    }

    private void checkUltNoteHit()
    {
        if (ultNoteRing.Count != 0)
        {
            UltimateNote top = ultNoteRing.Peek();
            float scale = top.transform.localScale.x;
            IEnumerator displayScoreText;
            if (scale <= 0.44f && scale >= 0.24f)
            {
                displayScoreText = showText(0.3f, goodText);
                pressedCoroutine = ChangeColor(0.3f, Color.yellow);
            }
            else
            {
                displayScoreText = showText(0.3f, perfectText);
                pressedCoroutine = ChangeColor(0.3f, Color.green);
            }
            StartCoroutine(displayScoreText);
            StartCoroutine(pressedCoroutine);
        }
    }

    private void updateUltNote()
    {
        if (ultNoteRing.Count != 0)
        {
            UltimateNote top = ultNoteRing.Peek();
            // dequeue and destroy if note scale is down 
            if (top.i >= 1.0f)
            {
                destroyUltNote();
            }
        }
    }

    private void destroyUltNote()
    {
        if (ultNoteRing.Count != 0 && ultNoteRing.Peek().transform.localScale.x <= 0.55f)
        {
            UltimateNote top = ultNoteRing.Dequeue();
            top.Destroy();
            Destroy(top.gameObject);
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
        ultNoteRing.Enqueue(ultNote);
        ultNote.GetComponent<UltimateNote>().SetNote(instance);
        ultNote.transform.parent = GameObject.Find("NoteSystem").transform;
        ultNote.duration = secPerBeat;
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
        //Debug.Log("Hit On Time");
    }

    public void UltNoteMissed()
    {
        //Debug.Log("Missed Note");
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
