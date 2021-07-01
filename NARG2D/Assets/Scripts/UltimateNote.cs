using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateNote : MonoBehaviour
{
    private float speed;
    private Renderer renderer;
    private Vector3 CurrentScale;
    private Vector3 scaleChange;
    private Vector3 minScale;
    public float duration;
    private float i;
    //private GameObject noteSystem;
    private NoteSystem note;
    void Start()
    {
        //note = noteSystem.GetComponent<NoteSystem>();
        minScale = new Vector3(0.2f, 0.2f, 0f);
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
            note.DestroyUltAction();
            // Kills the game object
            Destroy(gameObject);
            // Removes this script instance from the game object
            Destroy(this);
        }
        i += Time.deltaTime * speed;
        transform.localScale = Vector3.Lerp(CurrentScale, minScale, i);
    }

    public void SetNote(NoteSystem instance)
    {
        note = instance;
    }
}
