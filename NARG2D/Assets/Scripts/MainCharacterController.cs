using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    public GameObject enemy;
<<<<<<< Updated upstream
    private Renderer enemyRenderer;
    private Renderer selfRenderer;
    private Rigidbody2D rigidbody;
    private bool isGrounded = true;
    
=======

>>>>>>> Stashed changes
    private float jumpVelocity = 25f;
    private float moveVelocity = 3f;
    private Color heroColor;
    private Color lv0Color = new Color32(105, 152, 255, 255);
    private Color lv1Color = new Color32(24, 160, 238, 255);
    private Color lv2Color = new Color32(0, 119, 188, 255);
    private Color lv3Color = new Color32(0, 40, 126, 255);
    private Vector3 heroStartPosition;
    private Health enemyHealth;
    private Health playerHealth;
    public GameObject gameOver;
    private int level = 0;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.ambientLight = Color.white;

        rigidbody = GetComponent<Rigidbody2D>();
        enemyRenderer = enemy.GetComponent<Renderer>();
        selfRenderer = GetComponent<Renderer>();
        heroStartPosition = new Vector3((float)-4.0000, 0, 0);
        rigidbody.transform.position = heroStartPosition;
<<<<<<< Updated upstream
        selfRenderer.transform.position = heroStartPosition;
        enemyHealth = enemy.GetComponent<Health>();
        playerHealth = GetComponent<Health>();
        Physics2D.IgnoreCollision(enemy.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        heroColor = lv0Color;
        selfRenderer.material.SetColor("_Color", heroColor);
=======
         Physics2D.IgnoreCollision(enemy.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        heroColor = lv0Color;
        selfRenderer.material.SetColor("_Color", heroColor);
        */


        heroStartPosition = new Vector3(-4f, -3.5f, -5f);
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
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {

        if (playerHealth.currHealth == 0)
        {
            Time.timeScale = 0;
            gameOver.SetActive(true);
        }
        // Jump (No double jumping)
        if (Input.GetKeyDown("space"))
        {
<<<<<<< Updated upstream
            rigidbody.velocity = Vector2.up * jumpVelocity;
=======
            soundFX[0].Play();
            anim.SetTrigger("Jump");
            attack.Append(" ");
            KeyPressAnalytics("Jump", "Space");
>>>>>>> Stashed changes
        }

        // Attack (Dash Right)
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(Attack());
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
<<<<<<< Updated upstream
            StartCoroutine(Uppercut());
=======
            soundFX[2].Play();
            StartCoroutine(Uppercut());
            attack.Append("K");
>>>>>>> Stashed changes
        }

        // Block
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(Block());
        }
    }

    public void SetLevel(int newLvNum)
    {
        level = newLvNum;
        if (level == 0)
        {
            heroColor = lv0Color;
        }
        else if (level == 1)
        {
            heroColor = lv1Color;
        }
        else if (level == 2)
        {
            heroColor = lv2Color;
        }
        else if (level == 3)
        {
            heroColor = lv3Color;
        }
        selfRenderer.material.SetColor("_Color", heroColor);
    }

<<<<<<< Updated upstream
    public int GetLevel()
    {
        return level;
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
        rigidbody.transform.position = new Vector3(3, 0, 0);
        enemyRenderer.material.SetColor("_Color", Color.black);
        selfRenderer.material.SetColor("_Color", Color.white);
        enemyHealth.DamagePlayer(5 + level*2);
        yield return new WaitForSeconds(0.2f);
        enemyRenderer.material.SetColor("_Color", Color.red);
        selfRenderer.material.SetColor("_Color", heroColor);
        rigidbody.transform.position = heroStartPosition;
    }

=======
    IEnumerator Attack()
    {
        transform.position = new Vector3(0, -3.5f, -5f);
        anim.SetTrigger("Punch");
        enemyHealth.DamagePlayer(5);
        playerUltimate.fillBar(20, noteSystem.GetMultiplier());
        yield return new WaitForSeconds(0.5f);
        transform.position = heroStartPosition;
        KeyPressAnalytics("Attack", "S");
    }

    
>>>>>>> Stashed changes
    IEnumerator Uppercut()
    {
        transform.position = new Vector3(0, -3.5f, -5f);
        anim.SetTrigger("Kick");
        enemyHealth.DamagePlayer(5);
        playerUltimate.fillBar(20, noteSystem.GetMultiplier());
        yield return new WaitForSeconds(0.5f);
        transform.position = heroStartPosition;
        KeyPressAnalytics("Attack", "S");
    }
<<<<<<< Updated upstream

    IEnumerator Block()
    {
        selfRenderer.material.SetColor("_Color", Color.green);
=======
    
    IEnumerator Block()
    {
        anim.SetTrigger("Block");
>>>>>>> Stashed changes
        playerHealth.isBlocking = true;
        yield return new WaitForSeconds(0.5f);
        selfRenderer.material.SetColor("_Color", heroColor);
        playerHealth.isBlocking = false;
<<<<<<< Updated upstream
=======
        KeyPressAnalytics("Block", "S");
    }
    
    IEnumerator Special1()
    {
        anim.SetTrigger("Special1");
        enemyHealth.DamagePlayer(15);
        yield return new WaitForSeconds(0.2f);
        specialAtkCnt++;
        specialAtk1Cnt++;
    }
    
    IEnumerator Special2()
    {
        anim.SetTrigger("Special2");
        enemyHealth.DamagePlayer(20);
        yield return new WaitForSeconds(0.2f);
        specialAtkCnt++;
        specialAtk2Cnt++;
>>>>>>> Stashed changes
    }

}
