using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMove : MonoBehaviour
{
    public float minX;
    public float maxX;
    public bool movingLeft = true;
    public float speed = 1;

    // Update is called once per frame
    void Update()
    {
        if (movingLeft == true)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        else
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        if (transform.position.x > maxX)
        {
            movingLeft = true;
        }

        if (transform.position.x < minX)
        {
            movingLeft = false;
        }
    }
}
