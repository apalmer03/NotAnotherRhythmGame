using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    public GameObject enemy;
    public GameObject ultButtons;
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


        heroStartPosition = new Vector3((float)-4.0000, -2.55f, 0);
        transform.position = heroStartPosition;
        enemyHealth = enemy.GetComponent<Health>();
        playerHealth = GetComponent<Health>();
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        gameObject.GetComponent<MainCharacterController>().enabled = false;
        playerUltimate = GetComponent<Ultimate>();
        ultimate = ultimateAttack.GetComponent<UltimateScroller>();
        specialLookup = GetComponent<SpecialAttack>();
        noteSystem = multi.GetComponent<NoteSystem>();
        specialAtkCnt = 0;
        specialAtk1Cnt = 0;
        specialAtk2Cnt = 0;
        ultButtons.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        StringBuilder attack = new StringBuilder();
        // Jump (No double jumping)
        if (Input.GetKeyDown("space") && isGrounded)
        {
            soundFX[0].Play();
            rigidbody.velocity = Vector2.up * jumpVelocity;
            anim.SetTrigger("Jump");
            attack.Append(" ");
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
            //StartCoroutine(Uppercut());
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
            switch(specialMove)
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
        
        if (Input.GetKeyDown(KeyCode.H) && playerUltimate.isFull() )
        {
            playerUltimate.resetBar();
            ultimate.Activate();
            ultButtons.SetActive(true);
            enemyHealth.DamagePlayer(10);
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    IEnumerator Attack()
    {
        anim.SetTrigger("Attack");
        transform.position = new Vector3(0, -2.55f, 0);
        enemyHealth.DamagePlayer(5);
        playerUltimate.fillBar(20, noteSystem.GetMultiplier());
        yield return new WaitForSeconds(0.5f);
        transform.position = heroStartPosition;

    }

    /*
    IEnumerator Uppercut()
    {
        rigidbody.transform.position = new Vector3(3, 3, 0);
        enemyRenderer.material.SetColor("_Color", Color.black);
        selfRenderer.material.SetColor("_Color", Color.white);
        enemyHealth.DamagePlayer(5 + level*2);
        yield return new WaitForSeconds(0.2f);
        enemyRenderer.material.SetColor("_Color", Color.red);
        selfRenderer.material.SetColor("_Color", heroColor);
        rigidbody.transform.position = heroStartPosition;
    }
    */
    IEnumerator Block()
    {
        anim.SetTrigger("Sit");
        playerHealth.isBlocking = true;
        yield return new WaitForSeconds(0.5f);
        playerHealth.isBlocking = false;
    }
    
    IEnumerator Special1()
    {
        enemyHealth.DamagePlayer(15);
        yield return new WaitForSeconds(0.2f);
        specialAtkCnt++;
        specialAtk1Cnt++;
    }
    
    IEnumerator Special2()
    {
        enemyHealth.DamagePlayer(20);
        yield return new WaitForSeconds(0.2f);
        specialAtkCnt++;
        specialAtk2Cnt++;
    }
   
}
