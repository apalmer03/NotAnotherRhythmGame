using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    public GameObject enemy;
    private Renderer enemyRenderer;
    private Renderer selfRenderer;
    private Rigidbody2D rigidbody;
    private bool isGrounded = true;
    private float jumpVelocity = 25f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        enemyRenderer = enemy.GetComponent<Renderer>();
        selfRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && isGrounded)
        {
            isGrounded = false;
            rigidbody.velocity = Vector2.up * jumpVelocity;
        }

        if (Input.GetKey(KeyCode.F))
        {
            StartCoroutine(Attack());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
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
