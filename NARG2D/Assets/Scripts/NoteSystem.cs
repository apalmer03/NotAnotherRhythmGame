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
    public List<int> actions_list;
    public int[][] attack_pattern;
    private IEnumerator coroutine;

    public object[] actions;
    public GameObject gameOver;
    public int multiplier = 1;
    public UltimateScroller ultimateScroller;
    public int damageUltGoodNote = 10;
    public int damageUltPerfectNote = 20;
    public static NoteSystem instance;
    private Ultimate playerUltimate;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
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
        attack_pattern = new int[8][];
        attack_pattern[0] = new int[] { 4, 4, 4, 4 };
        attack_pattern[1] = new int[] { 4, 4, 4, 3 };
        attack_pattern[2] = new int[] { 4, 3, 4, 1 };
        attack_pattern[3] = new int[] { 4, 1, 4, 1 };
        attack_pattern[4] = new int[] { 3, 4, 3, 4 };
        attack_pattern[5] = new int[] { 4, 4, 4, 3 };
        attack_pattern[6] = new int[] { 3, 3, 3, 1 };
        attack_pattern[7] = new int[] { 3, 1, 3, 1 };


        for (var i = 0; i < 265; i++)
        {
            beats[i] = i;

        }
        for (var p = 0; p < 265 / 4; p++)
        {
            //actions_list.AddRange(attack_pattern[patt]);
            if (p < 4)
            {
                actions_list.AddRange(attack_pattern[0]);
            }
            //else if (p < 15)
            //{
            //    var patt = Random.Range(0, 2);
            //    actions_list.AddRange(attack_pattern[patt]);
            //}
            //else if (p < 30)
            //{
            //    var patt = Random.Range(1, 4);
            //    actions_list.AddRange(attack_pattern[patt]);
            //}
            //else if (p < 40)
            //{
            //    var patt = Random.Range(1, 5);
            //    actions_list.AddRange(attack_pattern[patt]);
            //}
            else
            {
                var patt = Random.Range(5, 7);
                actions_list.AddRange(attack_pattern[patt]);
            }
        }
        for (var i = 0; i < actions_list.ToArray().Length && i < actions.Length; i++)
        {
            actions[i] = (ActionNote.Action)actions_list.ToArray()[i];
        }
        beatsShownInAdvance = 1.0f / secPerBeat;
        currentBeat = 18;
        nextIndex = 19;
        player = GameObject.Find("Player");
        enemy = GameObject.Find("Enemy");
        enemyHealth = enemy.GetComponent<Health>();
        playerHealth = player.GetComponent<Health>();
        playerUltimate = player.GetComponent<Ultimate>();
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
            scoreText.text = "Total Score: " + totalscore.ToString();
            scoreText.gameObject.SetActive(true);
            gameOver.SetActive(true);
        }

        if (nextIndex < beats.Length && beats[nextIndex] < songPositionInBeats + beatsShownInAdvance)
        {
            GameObject.Find("CurrentBeat").GetComponent<Text>().text = string.Format("{0}/{1}", (int)songPositionInBeats, (int)songLength / secPerBeat);
            SpawnNote();
            if ((ActionNote.Action)actions[nextIndex] != ActionNote.Action.Idle)
            {
                SpawnActionNote((ActionNote.Action)actions[nextIndex]);
                Debug.Log(string.Format("Execute at song(beat) number {0}", songPositionInBeats));
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
                player.gameObject.GetComponent<MainCharacterController>().enabled = true;
                missText.SetActive(false);
                comboNum++;
                comboText.text = "Combo x " + comboNum.ToString();
                if (comboNum == 2)
                {
                    comboText.gameObject.SetActive(true);
                }

                if (comboNum >= 35)
                {
                    multiplier = 6;
                }
                else if (comboNum >= 26)
                {
                    multiplier = 5;
                }
                else if (comboNum >= 18)
                {
                    multiplier = 4;
                }
                else if (comboNum >= 11)
                {
                    multiplier = 3;
                }
                else if (comboNum >= 5)
                {
                    multiplier = 2;
                }
                else
                {
                    multiplier = 1;
                }
                totalscore = totalscore + (10 * multiplier);
                scoreText.text = "Total Score: " + totalscore.ToString();
                coroutine = ChangeColor(0.3f, Color.green);
                StartCoroutine(coroutine);
                AnalyticsResult analytics_comboCounter = Analytics.CustomEvent("Combo Length: " + comboNum);
                Debug.Log("Analytics result " + analytics_comboCounter);
                AnalyticsResult analytics_hitCounter = Analytics.CustomEvent("Combo Length: " + hitNum++);
                Debug.Log("Analytics result " + analytics_hitCounter);
            }
            // check if not on beat
            else
            {
                player.gameObject.GetComponent<MainCharacterController>().enabled = false;
                playerUltimate.resetBar();
                coroutine = ChangeColor(0.3f, Color.red);
                StartCoroutine(coroutine);
                IEnumerator showMissText = showMiss(0.3f);
                StartCoroutine(showMissText);
                comboNum = 0;
                comboText.gameObject.SetActive(false);
                GameObject[] notes = GameObject.FindGameObjectsWithTag("Note");
                if (notes.Length >= 1 && err <= 0.6)
                {
                    Destroy(notes[0].gameObject);
                    currentBeat++;
                }
                AnalyticsResult analytics_missCounter = Analytics.CustomEvent("Miss Counter: " + missNum++);
                Debug.Log("Analytics result" + analytics_missCounter);
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
        noteRing.transform.parent = GameObject.Find("NoteSystem").transform;
        noteRing.duration = 1.0f;
    }

    private void SpawnActionNote(ActionNote.Action action)
    {
        enemy.GetComponent<EnemyController>().doAction((ActionNote.Action)actions[nextIndex]);
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
        UltNoteHit(3);
    }
    public void UltPerfectHit()
    {
        UltNoteHit(5);
    }
}
