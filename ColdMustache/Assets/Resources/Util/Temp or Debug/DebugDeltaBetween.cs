using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDeltaBetween : MonoBehaviour {

    public Transform A;
    public Transform B;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log((Vector2)A.position - (Vector2)B.position);
	}
}
