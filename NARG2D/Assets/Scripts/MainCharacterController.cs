using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Analytics;

public class MainCharacterController : MonoBehaviour
{
    public GameObject enemy;
    private Renderer enemyRenderer;
    private Renderer selfRenderer;
    private Rigidbody2D rigidbody;
    private bool isGrounded = true;

    private float jumpVelocity = 25f;
    private float moveVelocity = 3f;

    private Vector3 heroStartPosition;
    private Health enemyHealth;
    private Health playerHealth;
    public GameObject gameOver;
    private int level = 0;
    private Animator anim;
    public AudioSource[] soundFX;

    public GameObject ultimateAttack;
    private Ultimate playerUltimate;
    public GameObject multi;
    private NoteSystem noteSystem;
    private UltimateScroller ultimate;

    private List<String> specialAttack = new List<String>();
    private SpecialAttack specialLookup;
    private int specialMax = 3;
    private string specialMove;

    public int specialAtkCnt = 0;
    public int specialAtk1Cnt = 0;
    public int specialAtk2Cnt = 0;

    public float time = 0.0f;
    public int seconds = 0; // TOTAL TIME USER SPENT IN TUTORIAL LEVEL (UNITY ANALYTICS)

    // Start is called before the first frame update
    void Start()
    {
        /* prototype
        RenderSettings.ambientLight = Color.white;
        selfRenderer = GetComponent<Renderer>();
        enemyRenderer = enemy.GetComponent<Renderer>();
        rigidbody.transform.position = heroStartPosition;
         Physics2D.IgnoreCollision(enemy.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        heroColor = lv0Color;
        selfRenderer.material.SetColor("_Color", heroColor);
        */


        heroStartPosition = new Vector3(-4f, -3.55f, -5f);
        transform.position = heroStartPosition;
        enemyHealth = enemy.GetComponent<Health>();
        playerHealth = GetComponent<Health>();
        anim = GetComponent<Animator>();
        gameObject.GetComponent<MainCharacterController>().enabled = false;
        playerUltimate = GetComponent<Ultimate>();
        ultimate = ultimateAttack.GetComponent<UltimateScroller>();
        specialLookup = GetComponent<SpecialAttack>();
        noteSystem = multi.GetComponent<NoteSystem>();
        specialAtkCnt = 0;
        specialAtk1Cnt = 0;
        specialAtk2Cnt = 0;
    }

    // Update is called once per frame
    void Update()
    {

        time += Time.deltaTime;
        seconds = (int)Math.Ceiling(time);
        seconds = seconds % 60;

        StringBuilder attack = new StringBuilder();
        // Jump (No double jumping)
        if (Input.GetKeyDown("space"))
        {
            soundFX[0].Play();
            anim.SetTrigger("Jump");
            attack.Append(" ");
            KeyPressAnalytics("Jump", "Space");
        }

        // Attack (Dash Right)
        if (Input.GetKeyDown(KeyCode.J))
        {
            soundFX[1].Play();
            StartCoroutine(Attack());
            attack.Append("J");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            soundFX[2].Play();
            StartCoroutine(Uppercut());
            attack.Append("K");
        }

        // Block
        if (Input.GetKeyDown(KeyCode.S))
        {
            soundFX[2].Play();
            StartCoroutine(Block());
            attack.Append("S");
        }

        if (attack.Length != 0)
        {

            specialAttack.Add(attack.ToString());
            Debug.Log("Attack: " + attack + " SpecailAttack: " + specialAttack[specialAttack.Count - 1]);
            attack.Clear();
        }

        specialMove = specialLookup.CheckSpecial(specialAttack);
        if (specialMove != null)
        {
            print(specialMove);
            switch (specialMove)
            {
                case "Special1":
                    StartCoroutine(Special1());
                    Debug.Log("Special1");
                    break;
                case "Special2":
                    StartCoroutine(Special2());
                    Debug.Log("Special2");
                    break;
            }
        }
        if (specialAttack.Count == specialMax)
        {
            specialAttack.RemoveAt(0);
        }

        if (Input.GetKeyDown(KeyCode.H) && playerUltimate.isFull())
        {
            playerUltimate.resetBar();
            ultimate.Activate();
            enemyHealth.DamagePlayer(10);
        }
    }


    IEnumerator Attack()
    {
        transform.position = new Vector3(0, -3.5f, -5f);
        anim.SetTrigger("Punch");
        enemyHealth.DamagePlayer(5);
        playerUltimate.fillBar(20, noteSystem.GetMultiplier());
        yield return new WaitForSeconds(0.5f);
        transform.position = heroStartPosition;
        KeyPressAnalytics("Attack", "J");
    }

    IEnumerator Uppercut()
    {
        transform.position = new Vector3(0, -3.5f, -5f);
        anim.SetTrigger("Kick");
        enemyHealth.DamagePlayer(5);
        playerUltimate.fillBar(20, noteSystem.GetMultiplier());
        yield return new WaitForSeconds(0.5f);
        transform.position = heroStartPosition;
        KeyPressAnalytics("Attack", "K");
    }

    IEnumerator Block()
    {
        //transform.position = new Vector3(0, -3.5f, -5f);
        anim.SetTrigger("Block");
        playerHealth.isBlocking = true;
        yield return new WaitForSeconds(0.5f);
        playerHealth.isBlocking = false;
        KeyPressAnalytics("Block", "S");
        transform.position = heroStartPosition;
    }

    IEnumerator Special1()
    {
        transform.position = new Vector3(0, -3.5f, -5f);
        anim.SetTrigger("Special1");
        enemyHealth.DamagePlayer(15);
        yield return new WaitForSeconds(0.2f);
        specialAtkCnt++;
        specialAtk1Cnt++;
    }

    IEnumerator Special2()
    {
        transform.position = new Vector3(0, -3.5f, -5f);
        anim.SetTrigger("Special2");
        enemyHealth.DamagePlayer(20);
        yield return new WaitForSeconds(0.2f);
        specialAtkCnt++;
        specialAtk2Cnt++;
    }

    public void KeyPressAnalytics(string actionType, string keyPressed)
    {
        Dictionary<string, string> analytics_inputAction = new Dictionary<string, string>
        {
            {"Attack", "J"},
            {"Block", "S"},
            {"Jump", "Space"},
        };
        AnalyticsResult analytics_actionType = Analytics.CustomEvent("ActionUsed: " + actionType + ", " + keyPressed);
        Debug.Log("Analytics Result(action used): " + analytics_actionType);
    }

}
