using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayFixedRotation : MonoBehaviour {

    Quaternion OriginalRotation;

	// Use this for initialization
	void Start () {
        OriginalRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = OriginalRotation;
	}
}
