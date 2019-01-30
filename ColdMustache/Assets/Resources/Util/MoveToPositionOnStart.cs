using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPositionOnStart : MonoBehaviour {

    public Vector3 LocalPosition;

	void Start () {
        transform.localPosition = LocalPosition;
        Destroy(this);
	}
	
}
