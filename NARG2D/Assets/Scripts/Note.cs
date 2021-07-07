using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    private float speed;
    private Renderer renderer;
    private Vector3 CurrentScale;
    private Vector3 scaleChange;
    private Vector3 minScale;
    public float duration;
    private float i;
    void Start()
    {
        minScale = new Vector3(0.25f, 0.25f, 0f);
        CurrentScale = new Vector3(2.0f, 2.0f, 0f);
        // duration = 1.0f;
        i = 0f;
        speed = 1.0f / duration;
    }

    // Update is called once per frame
    void Update()
    {
        if (i >= 1.0f)
        {
            // Kills the game object
            Destroy(gameObject);

            // Removes this script instance from the game object
            Destroy(this);
        }
        i += Time.deltaTime * speed;
        transform.localScale = Vector3.Lerp(CurrentScale, minScale, i);
    }

}
