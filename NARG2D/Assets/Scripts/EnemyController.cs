using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class EnemyController : MonoBehaviour
{

    private Rigidbody2D rb;
    private float jumpVelocity = 25f;
    private Vector3 enemyStartPosition;
    private GameObject player;
    private Health playerHealth;
    private Health enemyHealth;
    public GameObject levelComplete;
    private Animator anim;
    private Animator heroAnim;
    public GameObject blueFire;
    public GameObject orangeFire;
    private bool shootFireBall = false;
    private Vector3 fireStartPos;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        enemyStartPosition = new Vector3(4f, -3.55f, -5f);
        transform.position = enemyStartPosition;
        //enemyRenderer.transform.position = enemyStartPosition;
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<Health>();
        enemyHealth = GetComponent<Health>();
        anim = GetComponent<Animator>();
        heroAnim = player.GetComponent<Animator>();
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
        }
        else if (action == ActionNote.Action.Attack)
        {
            Debug.Log("Enemy Attack-Melee");
            StartCoroutine(Attack());
        }
        else if (action == ActionNote.Action.UpperCut)
        {
            Debug.Log("Enemy MagicBall");
            StartCoroutine(Magic());
        }
        else if (action == ActionNote.Action.Block)
        {
            Debug.Log("Enemy Block");
            StartCoroutine(Block());
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

    IEnumerator Block()
    {
        enemyHealth.isBlocking = true;
        anim.SetTrigger("Block");
        yield return new WaitForSeconds(1.00f);
        enemyHealth.isBlocking = false;
    }
    IEnumerator Attack()
    {
        anim.SetTrigger("Punch");
        anim.ResetTrigger("lighthit");
        anim.ResetTrigger("heavyhit");
        transform.position = new Vector3(0, -3.5f, -5f);
        // yield return new WaitForSeconds(0.5f);
      
        if (playerHealth.isBlocking)
        {
            heroAnim.SetTrigger("Parry");
            enemyHealth.DamagePlayer(5);
            GameObject.FindWithTag("Player").GetComponent<MainCharacterController>().soundFX[3].Play();
            AnalyticsResult analytics_blocking = Analytics.CustomEvent("Successful block");
            Debug.Log("Enemy Attack Blocked!");
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
            if(playerHealth.isBlocking)
            {
                heroAnim.SetTrigger("Parry");
                enemyHealth.DamagePlayer(5);
                GameObject.FindWithTag("Player").GetComponent<MainCharacterController>().soundFX[3].Play();
                Debug.Log("Enemy Attack Blocked-backup!");
            }
            else{
                Debug.Log("Enemy Attack Failed to Block!");
                heroAnim.SetTrigger("Hurt");
                GameObject.FindWithTag("Player").GetComponent<MainCharacterController>().soundFX[1].Play();
                playerHealth.DamagePlayer(40);
            }
           
        }
        yield return new WaitForSeconds(0.5f);
        transform.position = enemyStartPosition;
    }

    IEnumerator Magic()
    {
        anim.SetTrigger("Magic");
        anim.ResetTrigger("lighthit");
        anim.ResetTrigger("heavyhit");
        startTime = Time.time;
        
       
        //  
       
        if (playerHealth.isJumping)
        {
            GameObject.FindWithTag("Player").GetComponent<MainCharacterController>().soundFX[3].Play();
            Debug.Log("Enemy Attack Blocked!");
            AnalyticsResult analytics_blocking = Analytics.CustomEvent("Successful jump");
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
            if(playerHealth.isJumping)
            {
                GameObject.FindWithTag("Player").GetComponent<MainCharacterController>().soundFX[3].Play();
                Debug.Log("Enemy Attack Blocked-backup!");
            }
            else
            {
                Debug.Log("Enemy Attack Failed to Block!");
                GameObject.FindWithTag("Player").GetComponent<MainCharacterController>().soundFX[1].Play();
                heroAnim.SetTrigger("Hurt");
                playerHealth.DamagePlayer(60);
            }
        }
        shootFireBall = true;
        yield return new WaitForSeconds(1.00f);
        shootFireBall = false;
        blueFire.transform.position = fireStartPos;
    }

    IEnumerator MeleeCharge()
    {
        anim.SetTrigger("Charge");
        orangeFire.SetActive(true);
        yield return new WaitForSeconds(1.5f);
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
