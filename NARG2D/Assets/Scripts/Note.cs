using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    private float speed;
    private Renderer renderer;
    private Vector3 currentScale;
    private Vector3 scaleChange;
    private Vector3 minScale;
    public float duration;
    public float i;
    public bool onBeatState;
    public bool firstOnBeat;
    public float colorDuration;
    public float colorIndex;
    public float colorSpeed;
    private Renderer mRenderer;
    void Start()
    {
        minScale = new Vector3(0.25f, 0.25f, 0f);
        currentScale = new Vector3(2.0f, 2.0f, 0f);
        i = 0f;
        speed = 1.0f / duration;
        onBeatState = false;
        firstOnBeat = true;
        colorIndex = 0;
        mRenderer = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        i += Time.deltaTime * speed;
        transform.localScale = Vector3.Lerp(currentScale, minScale, i);
        if (onBeatState && firstOnBeat)
        {
            firstOnBeat = false;
            colorDuration = (1 - i) * duration;
            colorSpeed = 1.0f / colorDuration * 1.5f;
        }
        else if (onBeatState && !firstOnBeat)
        {
            if (colorIndex <= 1.0f)
            {
                colorIndex += Time.deltaTime * colorSpeed;
                mRenderer.material.color = Color.Lerp(Color.white, Color.red, colorIndex);
            }
        }
    }

}
