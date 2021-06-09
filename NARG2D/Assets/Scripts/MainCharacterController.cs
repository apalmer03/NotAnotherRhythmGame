using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    public GameObject enemy;
    private Renderer enemyRenderer;
    private Renderer selfRenderer;
    private Rigidbody2D rigidbody;
    private bool isGrounded = true;
    
    private float jumpVelocity = 25f;
    private float moveVelocity = 3f;
    private Color heroColor = new Color32(4, 160, 248, 255);
    private Vector3 heroStartPosition;
    private Health enemyHealth;
    private Health playerHealth;
    public GameObject gameOver;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.ambientLight = Color.white;

        rigidbody = GetComponent<Rigidbody2D>();
        enemyRenderer = enemy.GetComponent<Renderer>();
        selfRenderer = GetComponent<Renderer>();
        heroStartPosition = new Vector3((float)-4.0000, 0, 0);
        rigidbody.transform.position = heroStartPosition;
        selfRenderer.transform.position = heroStartPosition;
        enemyHealth = enemy.GetComponent<Health>();
        playerHealth = GetComponent<Health>();
        Physics2D.IgnoreCollision(enemy.GetComponent<Collider2D>(), GetComponent<Collider2D>());
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
        if (Input.GetKeyDown("space") && isGrounded)
        {
            rigidbody.velocity = Vector2.up * jumpVelocity;
        }

        // Attack (Dash Right)
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(Attack());
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(Uppercut());
        }

        // Block
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(Block());
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
        rigidbody.transform.position = new Vector3(3, 0, 0);
        enemyRenderer.material.SetColor("_Color", Color.black);
        selfRenderer.material.SetColor("_Color", Color.white);
        enemyHealth.DamagePlayer(5);
        yield return new WaitForSeconds(0.2f);
        enemyRenderer.material.SetColor("_Color", Color.red);
        selfRenderer.material.SetColor("_Color", heroColor);
        rigidbody.transform.position = heroStartPosition;
    }

    IEnumerator Uppercut()
    {
        rigidbody.transform.position = new Vector3(3, 3, 0);
        enemyRenderer.material.SetColor("_Color", Color.black);
        selfRenderer.material.SetColor("_Color", Color.white);
        enemyHealth.DamagePlayer(5);
        yield return new WaitForSeconds(0.2f);
        enemyRenderer.material.SetColor("_Color", Color.red);
        selfRenderer.material.SetColor("_Color", heroColor);
        rigidbody.transform.position = heroStartPosition;
    }

    IEnumerator Block()
    {
        selfRenderer.material.SetColor("_Color", Color.green);
        playerHealth.isBlocking = true;
        yield return new WaitForSeconds(0.5f);
        selfRenderer.material.SetColor("_Color", heroColor);
        playerHealth.isBlocking = false;
    }

}
