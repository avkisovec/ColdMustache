using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowlySlowDown : MonoBehaviour {

    Rigidbody2D rb;
    
    public float Factor = 0.99f;

    Vector2 Origin;

    public float EffectiveRange = 10;

    bool StartSlowingDown = false;


    //need a new method of slowing down, maybe having an effective range and when you surpass that the bullets starts quickly slowing down with unified factor, mby .99

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        Origin = (Vector2)transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (!StartSlowingDown)
        {
            if(((Vector2)transform.position - Origin).magnitude > EffectiveRange)
            {
                StartSlowingDown = true;
            }
        }
        else
        {
            for (int i = 0; i < FramerateManager.FramesRequiredToRenderToKeepUp; i++)
            {
                rb.velocity *= Factor;

                Factor *= Factor;

                if (rb.velocity.magnitude < 0.1f)
                {
                    Destroy(this.gameObject);
                }
            }
        }

	}
}
