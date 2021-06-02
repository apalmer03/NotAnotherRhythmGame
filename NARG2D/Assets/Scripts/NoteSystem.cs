using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSystem : MonoBehaviour
{
    public int bpm;
    private float beatRate;
    public GameObject note;
    public GameObject circle;
    private Renderer circleRenderer;
    public GameObject capsule;
    private Vector2 pos;
    private float nextTick = 0f;
    private bool color;
    // Start is called before the first frame update
    void Start()
    {
        beatRate = 60f / bpm;
        pos = new Vector2(-8f, -3f);
        circleRenderer = circle.GetComponent<Renderer>();
        InvokeRepeating("SpawnNote", 0f, beatRate);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void SpawnNote()
    {
        Instantiate(note, pos, Quaternion.identity);
        //ToggleColor();
    }

    private void ToggleColor()
    {
        if (color) {
            circleRenderer.material.SetColor("_Color", Color.blue);
            color = false;
        }
        else
        {
            circleRenderer.material.SetColor("_Color", Color.white);
            color = true;
        }
    }

}
