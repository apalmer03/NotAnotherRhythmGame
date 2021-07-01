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
    public GameObject special1;
    public GameObject special2;
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
        special1.SetActive(false);
        special2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        seconds = (int)Math.Ceiling(time);
        seconds = seconds % 60;
    }

    public void doAction()
    {
        StringBuilder attack = new StringBuilder();
        // Jump (No double jumping)
        if (Input.GetKeyDown("space"))
        {
            soundFX[0].Play();
            StartCoroutine(Jump());
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
            //ultimate.Activate();
            noteSystem.ActivateUlt();
            enemyHealth.DamagePlayer(20);
            StartCoroutine(Ultimate());
        }
    }
    
    public void doUltAction(int ultAction)
    {

        switch (ultAction)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    soundFX[1].Play();
                    enemyHealth.DamagePlayer(5);
                    //KeyPressAnalytics("Jump", "Space");
                }
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    soundFX[1].Play();
                    enemyHealth.DamagePlayer(5);
                    //KeyPressAnalytics("Jump", "Space");
                }
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    soundFX[1].Play();
                    enemyHealth.DamagePlayer(5);
                    //KeyPressAnalytics("Jump", "Space");
                }
                break;
            case 3:
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    soundFX[1].Play();
                    enemyHealth.DamagePlayer(5);
                    //KeyPressAnalytics("Jump", "Space");
                }
                break;
            case 4:
                if (Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.DownArrow))
                {
                    soundFX[1].Play();
                    enemyHealth.DamagePlayer(5);
                    //KeyPressAnalytics("Jump", "Space");
                }
                break;
            case 5:
                if (Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    soundFX[1].Play();
                    enemyHealth.DamagePlayer(5);
                    //KeyPressAnalytics("Jump", "Space");
                }
                break;
            case 6:
                if (Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.RightArrow))
                {
                    soundFX[1].Play();
                    enemyHealth.DamagePlayer(5);
                    //KeyPressAnalytics("Jump", "Space");
                }
                break;
            case 7:
                if (Input.GetKeyDown(KeyCode.LeftArrow) && Input.GetKeyDown(KeyCode.RightArrow))
                {
                    soundFX[1].Play();
                    enemyHealth.DamagePlayer(5);
                    //KeyPressAnalytics("Jump", "Space");
                }
                break;
            case 8:
                if (Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    soundFX[1].Play();
                    enemyHealth.DamagePlayer(5);
                    //KeyPressAnalytics("Jump", "Space");
                }
                break;
            case 9:
                if (Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.RightArrow))
                {
                    soundFX[1].Play();
                    enemyHealth.DamagePlayer(5);
                    //KeyPressAnalytics("Jump", "Space");
                }
                break;
        }
    }

    IEnumerator Jump()
    {
        anim.SetTrigger("Jump");
        playerHealth.isJumping = true;
        yield return new WaitForSeconds(1f);
        playerHealth.isJumping = false;
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

        anim.SetTrigger("Block");
        playerHealth.isBlocking = true;
        yield return new WaitForSeconds(1f);
        playerHealth.isBlocking = false;
        KeyPressAnalytics("Block", "S");
    }

    IEnumerator Special1()
    {
        IEnumerator showSpecial1 = ShowSpecial1(1.2f);
        StartCoroutine(showSpecial1);
        //transform.position = new Vector3(0, -3.5f, -5f);
        //anim.SetTrigger("Special1");
        enemyHealth.DamagePlayer(20);
        yield return new WaitForSeconds(0.2f);
        specialAtkCnt++;
        specialAtk1Cnt++;
    }

    IEnumerator Special2()
    {
        IEnumerator showSpecial2 = ShowSpecial2(1.2f);
        StartCoroutine(showSpecial2);
        //transform.position = new Vector3(0, -3.5f, -5f);
        //anim.SetTrigger("Special2");
        enemyHealth.DamagePlayer(10);
        yield return new WaitForSeconds(0.2f);
        specialAtkCnt++;
        specialAtk2Cnt++;
    }
    
    IEnumerator Ultimate()
    {
        anim.SetTrigger("Ultimate");
        transform.position = new Vector3(0, -3.5f, -5f);
        yield return new WaitForSeconds(15f);
        transform.position = heroStartPosition;

    }
    public void KeyPressAnalytics(string actionType, string keyPressed)
    {
        Dictionary<string, string> analytics_inputAction = new Dictionary<string, string>
        {
            {"Attack", "J"},
            {"Attack", "K"},
            {"Block", "S"},
            {"Jump", "Space"},
            {"Special1", "SpaceJJ"},
            {"Special2", "JKJ"},
            { "Ultimate", "H"}
        };
        AnalyticsResult analytics_actionType = Analytics.CustomEvent("ActionUsed: " + actionType + ", " + keyPressed);
        Debug.Log("Analytics Result(action used): " + analytics_actionType);
    }
    
    private IEnumerator ShowSpecial1(float waitTime)
    {
        special1.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        special1.SetActive(false);
    }
    private IEnumerator ShowSpecial2(float waitTime)
    {
        special2.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        special2.SetActive(false);
    }
}
