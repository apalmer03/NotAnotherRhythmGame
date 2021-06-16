using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float speed;
    private Renderer renderer;
    // private GameObject circle;
    // private GameObject enemy;
    // private Renderer circleRenderer;
    // public float livingTime = 0f;
    // public float lifeSpan;
    private Vector3 CurrentScale;
    private Vector3 scaleChange;
    private Vector3 minScale;
    private float duration;
    private float rate;
    private float i;
    // Start is called before the first frame update
    // void Start()
    // {
    //     renderer = GetComponent<Renderer>();
    //     // circle = GameObject.FindGameObjectWithTag("Circle");
    //     // circleRenderer = circle.GetComponent<Renderer>();
    //     // circleRenderer.material.SetColor("_Color", Color.white);
    //     scaleChange = new Vector3(-0.5f, -0.5f, 0f);
    //     minScale = new Vector3(1.0f, 1.0f, 0f);
    // }
    void Start()
    {
        minScale = new Vector3(1.0f, 1.0f, 0f);
        CurrentScale = new Vector3(25.0f, 25.0f, 0f);
        duration = 1.0f;
        i = 0f;
        rate = (1.0f / duration) * speed;
    }

    // Update is called once per frame
    void Update()
    {
        // livingTime += Time.deltaTime;
        if (i >= 1.0f)
        {
            // Kills the game object
            Destroy(gameObject);

            // Removes this script instance from the game object
            Destroy(this);
        }
        // transform.Translate(Vector2.right * speed * Time.deltaTime);
        // transform.localScale += scaleChange;
        i += Time.deltaTime * rate;
        transform.localScale = Vector3.Lerp(CurrentScale, minScale, i);
    }

}
