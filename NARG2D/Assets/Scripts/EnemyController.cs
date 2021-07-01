using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

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
    public GameObject orangeFire;
    private bool shootFireBall = false;
    private Vector3 fireStartPos;
    private float startTime;

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
        fireStartPos = blueFire.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (shootFireBall)
        {
            blueFire.transform.Translate(Vector3.forward * (Time.time-startTime));
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
            StartCoroutine(Magic());
            //GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        }
        else if (action == ActionNote.Action.Block)
        {
            Debug.Log("Enemy Block");
            //StartCoroutine(Charge());
            //anim.SetTrigger("Charge");
            //    GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        }
        else if (action == ActionNote.Action.Charge_1)
        {
            Debug.Log("Enemy Charge_1");
            StartCoroutine(MeleeCharge());
            //anim.SetTrigger("Charge");
            //    GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        }
        else if (action == ActionNote.Action.Charge_2)
        {
            Debug.Log("Enemy Magic");
            StartCoroutine(MagicCharge());
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
        yield return new WaitForSeconds(0.5f);
        if (playerHealth.isBlocking)
        {

            GameObject.FindWithTag("Player").GetComponent<MainCharacterController>().soundFX[3].Play();
            AnalyticsResult analytics_blocking = Analytics.CustomEvent("Successful block");
            Debug.Log("Enemy Attack Blocked!");
        }
        else
        {
            Debug.Log("Enemy Attack Failed to Block!");
            GameObject.FindWithTag("Player").GetComponent<MainCharacterController>().soundFX[1].Play();
            playerHealth.DamagePlayer(40);
        }

        transform.position = enemyStartPosition;
    }

    IEnumerator Magic()
    {
        anim.SetTrigger("Magic");
        startTime = Time.time;
        shootFireBall = true;
        yield return new WaitForSeconds(0.5f);
        if (playerHealth.isJumping)
        {
            Debug.Log("Enemy Attack Blocked!");
            AnalyticsResult analytics_blocking = Analytics.CustomEvent("Successful jump");
        }
        else
        {
            Debug.Log("Enemy Attack Failed to Block!");
            GameObject.FindWithTag("Player").GetComponent<MainCharacterController>().soundFX[1].Play();
            playerHealth.DamagePlayer(60);
        }
        shootFireBall = false;
        blueFire.transform.position = fireStartPos;
    }

    IEnumerator MeleeCharge()
    {
        anim.SetTrigger("Charge");
        orangeFire.SetActive(true);
        yield return new WaitForSeconds(2f);
        orangeFire.SetActive(false);
    }

    IEnumerator MagicCharge()
    {
        anim.SetTrigger("Charge");
        blueFire.SetActive(true);
        yield return new WaitForSeconds(2f);
        blueFire.SetActive(false);

    }

}
