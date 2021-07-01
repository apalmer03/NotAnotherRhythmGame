using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateScroller : MonoBehaviour
{
    public float beatTempo;
    public bool hasStarted;
    public GameObject ultimateButtons;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        beatTempo = beatTempo / 60f;
        player = GameObject.Find("Player");
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(hasStarted)
        {
            transform.position += new Vector3(0f, beatTempo * Time.deltaTime, 0f);
        }

        if (transform.position.y > 30)
        {
            hasStarted = false;
            ultimateButtons.SetActive(false);
            player.gameObject.GetComponent<MainCharacterController>().enabled = true;
            transform.position = new Vector3(0f, 0f, 0f);
        }
    }

    public void Activate()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        ultimateButtons.SetActive(true);
        player.gameObject.GetComponent<MainCharacterController>().enabled = false;
        hasStarted = true;
        //this.gameObject.SetActive(true);
        //transform.position -= new Vector3(0f, -beatTempo * Time.deltaTime, 0f);
    }
    
}
