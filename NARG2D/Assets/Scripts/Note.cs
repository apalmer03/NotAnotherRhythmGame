using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float speed;
    private Renderer renderer;
    private GameObject circle;
    private Renderer circleRenderer;
    public float livingTime = 0f;
    public float lifeSpan;
    public enum BeatAction
    {
        Jump = 0,
        Attack = 1,
        UpperCut = 2,
        Block = 3

    };
    public BeatAction action;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        circle = GameObject.FindGameObjectWithTag("Circle");
        circleRenderer = circle.GetComponent<Renderer>();
        circleRenderer.material.SetColor("_Color", Color.white);


        if (action == BeatAction.Jump)
        {
            renderer.material.SetColor("_Color", Color.magenta);
        }
        else if (action == BeatAction.Attack)
        {
            renderer.material.SetColor("_Color", Color.red);
        }
        else if (action == BeatAction.UpperCut)
        {
            renderer.material.SetColor("_Color", Color.yellow);
        }
        else if (action == BeatAction.Block)
        {
            renderer.material.SetColor("_Color", Color.green);
        }
        else
        {
            renderer.material.SetColor("_Color", Color.white);
        }
    }

    // Update is called once per frame
    void Update()
    {
        livingTime += Time.deltaTime;
        if (livingTime >= lifeSpan)
        {
            // Kills the game object
            Destroy(gameObject);

            // Removes this script instance from the game object
            Destroy(this);
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

}
