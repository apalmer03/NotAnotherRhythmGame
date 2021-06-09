using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Renderer enemyRenderer;

    private Rigidbody2D rb;
    private bool isGrounded = true;
    private float jumpVelocity = 25f;
    private Vector3 enemyStartPosition;
    private Color enemyColor = new Color32(4, 160, 248, 255);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyRenderer = GetComponent<Renderer>();
        enemyRenderer.material.SetColor("_Color", enemyColor);
        enemyStartPosition = new Vector3((float)4, 0, (float)0);
        rb.transform.position = enemyStartPosition;
        enemyRenderer.transform.position = enemyStartPosition;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0) && isGrounded)
        {
            isGrounded = false;
            rb.velocity = Vector2.up * jumpVelocity;
        }

        if (Input.GetKey(KeyCode.KeypadPeriod))
        {
            StartCoroutine(Attack());
        }
        if (Input.GetKey(KeyCode.KeypadEnter))
        {
            StartCoroutine(Uppercut());
        }

        // Block
        if (Input.GetKey(KeyCode.Keypad3))
        {
            StartCoroutine(Block());
        }
    }

    public void doAction(Note.BeatAction action)
    {

        if (action == Note.BeatAction.Jump)
        {
            Debug.Log("Enemy Jump");
            StartCoroutine(Jump());
            //enemyRenderer.material.SetColor("_Color", Color.cyan);

        }
        if (action == Note.BeatAction.Attack)
        {
            Debug.Log("Enemy Attack");
            StartCoroutine(Attack());
            //enemyRenderer.material.SetColor("_Color", Color.red);
        }
        if (action == Note.BeatAction.UpperCut)
        {
            Debug.Log("Enemy UpperCut");
            StartCoroutine(Uppercut());
            //enemyRenderer.material.SetColor("_Color", Color.red);
        }
        if (action == Note.BeatAction.Block)
        {
            Debug.Log("Enemy Block");
            StartCoroutine(Block());
            //enemyRenderer.material.SetColor("_Color", Color.red);
        }

    }
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Ground")
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
    IEnumerator Jump()
    {
        isGrounded = false;
        enemyRenderer.material.SetColor("_Color", Color.magenta);

        rb.velocity = Vector2.up * jumpVelocity;
        yield return new WaitUntil(() => isGrounded == true);
        enemyRenderer.material.SetColor("_Color", enemyColor);
    }
    IEnumerator Attack()
    {
        rb.transform.position = new Vector3((float)-3.5, 0, 0);

        enemyRenderer.material.SetColor("_Color", Color.red);
        yield return new WaitForSeconds(0.25f);

        enemyRenderer.material.SetColor("_Color", enemyColor);
        rb.transform.position = enemyStartPosition;
    }

    IEnumerator Uppercut()
    {
        rb.transform.position = new Vector3((float)-3.5, 3, 0);
        enemyRenderer.material.SetColor("_Color", Color.yellow);

        yield return new WaitForSeconds(0.25f);
        enemyRenderer.material.SetColor("_Color", enemyColor);

        rb.transform.position = enemyStartPosition;
    }

    IEnumerator Block()
    {
        enemyRenderer.material.SetColor("_Color", Color.green);
        yield return new WaitForSeconds(0.25f);
        enemyRenderer.material.SetColor("_Color", enemyColor);
    }

}