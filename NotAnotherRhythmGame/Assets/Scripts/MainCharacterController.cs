using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    public GameObject enemy;
    private Renderer enemyRenderer;
    private Renderer selfRenderer;
    private Rigidbody rigidbody;
    private bool jumping = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        enemyRenderer = enemy.GetComponent<Renderer>();
        selfRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            jumping = true;
        }
        if (jumping)
        {
            jumping = false;
            Vector3 force = new Vector3(0f, 40f, 0f);
            rigidbody.AddForce(force);
        }

        if (Input.GetKey(KeyCode.F))
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        enemyRenderer.material.SetColor("_Color", Color.black);
        selfRenderer.material.SetColor("_Color", Color.white);
        yield return new WaitForSeconds(0.2f);
        enemyRenderer.material.SetColor("_Color", Color.red);
        Color blueColor = new Color32(4, 160, 248, 255);
        selfRenderer.material.SetColor("_Color", blueColor);
    }

}
