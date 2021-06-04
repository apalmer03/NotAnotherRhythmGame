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
