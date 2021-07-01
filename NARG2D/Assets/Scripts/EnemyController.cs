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
    public GameObject blueFire;
    public GameObject OrangeFire;
   
    // Start is called before the first frame update
    void Start()
    {
        enemyStartPosition = new Vector3(4.5f, -3.55f, -5f);
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
            StartCoroutine(Uppercut());
            //GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        }
        else if (action == ActionNote.Action.Block)
        {
            Debug.Log("Enemy Charge");
            StartCoroutine(Charge());
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
        anim.SetTrigger("Punch");
        transform.position = new Vector3(0, -3.5f, -5f);
        if (playerHealth.isBlocking)
        {

            GameObject.FindWithTag("Player").GetComponent<MainCharacterController>().soundFX[3].Play();
            Debug.Log("Enemy Attack Blocked!");
        }
        else
        {
            Debug.Log("Enemy Attack Failed to Block!");
            GameObject.FindWithTag("Player").GetComponent<MainCharacterController>().soundFX[1].Play();
            playerHealth.DamagePlayer(10);
        }
        
        yield return new WaitForSeconds(0.5f);
        transform.position = enemyStartPosition;
    }

    IEnumerator Uppercut()
    {
        anim.SetTrigger("Magic");
        transform.position = new Vector3(0, -3.5f, -5f);
        /*
        if (playerHealth.isBlocking)
        {

            GameObject.FindWithTag("Player").GetComponent<MainCharacterController>().soundFX[3].Play();
            Debug.Log("Enemy Attack Blocked!");
        }
        else
        {
            Debug.Log("Enemy Attack Failed to Block!");
            GameObject.FindWithTag("Player").GetComponent<MainCharacterController>().soundFX[1].Play();
            playerHealth.DamagePlayer(10);
        }
        */
        yield return new WaitForSeconds(0.5f);
        transform.position = enemyStartPosition;
    }
    IEnumerator Charge()
    {
        blueFire.SetActive(true);
        yield return new WaitForSeconds(3f);
        blueFire.SetActive(false);
    }

}
