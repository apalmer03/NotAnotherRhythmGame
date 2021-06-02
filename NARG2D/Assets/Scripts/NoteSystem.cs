using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSystem : MonoBehaviour
{
    public int bpm;
    public float speed;
    private float beatRate;
    public Note note;
    private Renderer circleRenderer;
    public GameObject capsule;
    private Vector2 leftPos;
    private Vector2 rightPos;
    private bool collision = false;
    // Start is called before the first frame update
    void Start()
    {
        beatRate = 60f / bpm;
        leftPos = new Vector2(-9f, -3f);
        rightPos = new Vector2(9f, -3f);
        circleRenderer = GetComponent<Renderer>();
        InvokeRepeating("SpawnNote", 0f, beatRate);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (collision)
                ChangeColor();

            GameObject[] notes = GameObject.FindGameObjectsWithTag("Note");
            //Debug.Log(notes.Length + " notes on screen\n");
            Destroy(notes[0].gameObject);
            Destroy(notes[1].gameObject);
        }
    }

    private void SpawnNote()
    {
        Note leftNote = Instantiate(note, leftPos, Quaternion.identity);
        leftNote.speed = speed;
        Note rightNote = Instantiate(note, rightPos, Quaternion.identity);
        rightNote.speed = -speed;
        //ToggleColor();
    }

    private void OnCollisionEnter2D()
    {
        Debug.Log("collision detected\n");
        collision = true;
    }
    private void OnCollisionExit2D()
    {
        Debug.Log("collision exit\n");
        collision = false;
    }

    private IEnumerator ChangeColor()
    {
        circleRenderer.material.SetColor("_Color", Color.green);
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Setting color to white\n");
        circleRenderer.material.SetColor("_Color", Color.white);
        //Destroy(this.gameObject);
    }
}
