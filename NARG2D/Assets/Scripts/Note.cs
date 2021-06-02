using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float speed = 1f;
    private Renderer renderer;
    private GameObject circle;
    private Renderer circleRenderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        circle = GameObject.FindGameObjectWithTag("Circle");
        circleRenderer = circle.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        
        if (Input.anyKey)
        {
            if (collision.gameObject.tag == "Circle")
            {
                circleRenderer.material.SetColor("_Color", Color.yellow);
                //StartCoroutine(Waitforit());
                Destroy(this.gameObject);
            }
            if (collision.gameObject.tag == "Capsule")
            {
                circleRenderer.material.SetColor("_Color", Color.green);
                //StartCoroutine(Waitforit());
                Destroy(this.gameObject);

            }

        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Circle")
        {
            renderer.material.SetColor("_Color", Color.red);
        }
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator Waitforit()
    {
        yield return new WaitForSeconds(0.5f);
        circleRenderer.material.SetColor("_Color", Color.white);
        Destroy(this.gameObject);
    }
}
