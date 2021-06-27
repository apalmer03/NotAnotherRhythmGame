using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialMainCharacterController : MonoBehaviour
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

    private bool tutorialCompleted = false;

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
        // gameObject.GetComponent<TutorialMainCharacterController>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
    	GameObject go = GameObject.FindGameObjectWithTag("Instruction");
        TutorialInstructionController tic = go.GetComponent<TutorialInstructionController>();
        this.tutorialCompleted = tic.tutorialCompleted;

        Debug.Log(this.tutorialCompleted);

        // if (playerHealth.currHealth == 0)
        // {
        //     Time.timeScale = 0;
        //     gameOver.SetActive(true);
        // }
        // Jump (No double jumping)
        if (Input.GetKeyDown("space") && isGrounded)
        {
            soundFX[0].Play();
            rigidbody.velocity = Vector2.up * jumpVelocity;
            anim.SetTrigger("Jump");
        }

        // Attack (Dash Right)
        if (Input.GetKeyDown(KeyCode.J))
        {
    		soundFX[1].Play();
        	StartCoroutine(Attack());
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            soundFX[2].Play();
            //StartCoroutine(Uppercut());
        }

        // Block
        if (Input.GetKeyDown(KeyCode.S))
        {
            soundFX[3].Play();
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
    	// Regular
    	if (this.tutorialCompleted) 
    	{
			anim.SetTrigger("Attack");
		    transform.position = new Vector3(0, -2.55f, 0);
		    enemyHealth.DamagePlayer(5);
		    yield return new WaitForSeconds(0.5f);
		    transform.position = heroStartPosition;
		}
		// Tutorial - No damage
		else
		{
			anim.SetTrigger("Attack");
			transform.position = new Vector3(0, -2.55f, 0);
        	yield return new WaitForSeconds(0.5f);
        	transform.position = heroStartPosition;
		}
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

}
