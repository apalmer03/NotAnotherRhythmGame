using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateNote : MonoBehaviour
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
    //private GameObject noteSystem;
    private NoteSystem note;
    void Start()
    {
        minScale = new Vector3(0.0f, 0.0f, 0f);
        currentScale = new Vector3(2.0f, 2.0f, 0f);
        i = 0f;
        speed = 1.0f / duration;
        onBeatState = false;
        firstOnBeat = true;
        colorIndex = 0;
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
            colorSpeed = 1.0f / colorDuration;
        }
        else if (onBeatState && !firstOnBeat)
        {
            colorIndex += Time.deltaTime * colorSpeed;
            // transform to green
            /*if (colorIndex <= 1.0f)
            {
                mRenderer.material.color = Color.Lerp(Color.white, Color.green, colorIndex);
            }*/
        }
    }

    public void SetNote(NoteSystem instance)
    {
        note = instance;
    }

    public void Destroy()
    {
        note.DestroyUltAction();
        // Kills the game object
        Destroy(gameObject);
        // Removes this script instance from the game object
        Destroy(this);
    }
}
