using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail_cart_bumpable : MonoBehaviour {

    public bool Horizontal;
    public float HorMinX;
    public float HorMaxX;

    public bool Vertical;
    public float VerMinY;
    public float VerMaxY;

    Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        if (UniversalReference.PlayerObject.transform.position.x > transform.position.x - transform.lossyScale.x &&
               UniversalReference.PlayerObject.transform.position.x < transform.position.x + transform.lossyScale.x &&
               UniversalReference.PlayerObject.transform.position.y > transform.position.y - transform.lossyScale.y &&
               UniversalReference.PlayerObject.transform.position.y < transform.position.y + transform.lossyScale.y)
        {
            if (Horizontal)
            {
                if (UniversalReference.PlayerObject.transform.position.x > transform.position.x)
                {
                    rb.AddForce(Vector2.left * 3);
                }
                else
                {
                    rb.AddForce(Vector2.right * 3);
                }
            }

            if (Vertical)
            {
                if (UniversalReference.PlayerObject.transform.position.y > transform.position.y)
                {
                    rb.AddForce(Vector2.down * 3);
                }
                else
                {
                    rb.AddForce(Vector2.up * 3);
                }
            }
        }

        if (Horizontal)
        {
            if (transform.position.x < HorMinX)
            {
                transform.localPosition = new Vector3(HorMinX, transform.localPosition.y, transform.localPosition.z);
                rb.velocity = -0.4f * rb.velocity;
            }
            if (transform.position.x > HorMaxX)
            {
                transform.localPosition = new Vector3(HorMaxX, transform.localPosition.y, transform.localPosition.z);
                rb.velocity = -0.4f * rb.velocity;
            }
        }
        if (Vertical)
        {
            if (transform.localPosition.y < VerMinY)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, VerMinY, transform.localPosition.z);
                rb.velocity = -0.4f * rb.velocity;
            }
            if (transform.localPosition.y > VerMaxY)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, VerMaxY, transform.localPosition.z);
                rb.velocity = -0.4f * rb.velocity;
            }
        }

        



    }
}
