using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNote : MonoBehaviour
{
    public float speed;
    private Renderer renderer;
    private GameObject circle;
    private GameObject enemy;
    private Renderer circleRenderer;
    public float livingTime = 0f;
    public float lifeSpan;
    public enum Action
    {
        Jump = 0,
        Attack = 1,
        UpperCut = 2,
        Block = 3,
        Idle = 4,
        Charge_1 = 5,
        Charge_2 = 6
    };

    public Action action;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.Find("enemy");
        if (action == Action.Jump)
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.magenta);
        }
        else if (action == Action.Attack)
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
        else if (action == Action.UpperCut)
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        }
        else if (action == Action.Block)
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        }
        else
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }
    }

    // Update is called once per frame
    void Update()
    {
        livingTime += Time.deltaTime;
        if (livingTime >= lifeSpan)
        {
            //enemy.GetComponent<EnemyController>().doAction(action);
            // Kills the game object
            Destroy(gameObject);

            // Removes this script instance from the game object
            Destroy(this);
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}
