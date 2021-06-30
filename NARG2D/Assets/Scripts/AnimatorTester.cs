using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTester : MonoBehaviour
{
    private Animator anim;
    public Animator enemyAnim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            anim.SetTrigger("Jump");
        }

        // Attack (Dash Right)
        if (Input.GetKeyDown(KeyCode.J))
        {
            anim.SetTrigger("Punch");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            anim.SetTrigger("Kick");
        }

        // Block
        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.SetTrigger("Block");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            anim.SetTrigger("Ultimate");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            anim.SetTrigger("Special1");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("Special2");
        }


        // enemy
        if (Input.GetKeyDown(KeyCode.Z))
        {
            enemyAnim.SetTrigger("Punch");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            enemyAnim.SetTrigger("Magic");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            enemyAnim.SetTrigger("Block");
        }
    }
}
