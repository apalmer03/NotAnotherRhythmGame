using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateNoteObject : MonoBehaviour
{
    // Start is called before the first frame update
    public bool canBePressed;
    public KeyCode keyToPress;
    public GameObject hitEffect, goodEffect, perfectEffect, missEffect;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            if (canBePressed)
            {
                gameObject.SetActive(false);
                //NoteSystem.instance.UltNoteHit();

                if (transform.position.y > 3.95 && transform.position.y < 4.05)
                {
                    Debug.Log("Perfect");
                    NoteSystem.instance.UltPerfectHit();
                    Instantiate(perfectEffect, transform.position, perfectEffect.transform.rotation);
                }
                else if (transform.position.y > 3.85f && transform.position.y < 4.15)
                {
                    Debug.Log("GOOD");
                    NoteSystem.instance.UltGoodHit();
                    Instantiate(goodEffect, transform.position, goodEffect.transform.rotation);
                }
                else
                {
                    Debug.Log("OK");
                    NoteSystem.instance.UltNormalHit();
                    Instantiate(hitEffect, transform.position, hitEffect.transform.rotation);
                }
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            canBePressed = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            canBePressed = false;
            if (gameObject.activeSelf)
            {
                NoteSystem.instance.UltNoteMissed();
                Instantiate(missEffect, transform.position, missEffect.transform.rotation);
            }
        }
    }
}
