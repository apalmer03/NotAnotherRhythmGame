using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    private Rigidbody2D rb;
    private bool isGrounded = true;
    private float jumpVelocity = 25f;
    private Vector3 enemyStartPosition;
    private GameObject player;
    private Health playerHealth;
    private Health enemyHealth;
    public GameObject levelComplete;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        enemyStartPosition = new Vector3(4f, -2.55f, 0);
        transform.position = enemyStartPosition;
        //enemyRenderer.transform.position = enemyStartPosition;
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<Health>();
        enemyHealth = GetComponent<Health>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth.currHealth <= 0)
        {
            Time.timeScale = 0;
            levelComplete.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            anim.SetTrigger("Charge");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(Attack());
        }

    }

    public void doAction(ActionNote.Action action)
    {
        if (action == ActionNote.Action.Jump)
        {
            Debug.Log("Enemy Jump");
            //GetComponent<Renderer>().material.SetColor("_Color", Color.magenta);
        }
        else if (action == ActionNote.Action.Attack)
        {
            Debug.Log("Enemy Attack");
            StartCoroutine(Attack());
            //GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
        else if (action == ActionNote.Action.UpperCut)
        {
            Debug.Log("Enemy UpperCut");
            //GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        }
        else if (action == ActionNote.Action.Block)
        {
            Debug.Log("Enemy Block");
            //anim.SetTrigger("Charge");
            //    GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        }
        else
        {
            Debug.Log("Enemy Idle");
            //  GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }
    }
    IEnumerator Attack()
    {
        anim.SetTrigger("Attack");
        transform.position = new Vector3(0, -2.55f, 0);
        if (playerHealth.isBlocking)
        {
            Debug.Log("Enemy Attack Blocked!");
        }
        else
        {
            Debug.Log("Enemy Attack Failed to Block!");
            playerHealth.DamagePlayer(10);
        }
        
        yield return new WaitForSeconds(0.5f);
        transform.position = enemyStartPosition;
    }

}
