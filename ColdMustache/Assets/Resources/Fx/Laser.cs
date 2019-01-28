using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public Vector3 Origin;

    public Vector3 End;
    
	// Use this for initialization
	void Start () {
        
        transform.position = Origin + (Vector3)(((Vector2)End - (Vector2)Origin) / 2);
        transform.localScale = new Vector3(((Vector2)End - (Vector2)Origin).magnitude * 32, transform.localScale.y, 1);

        Util.RotateTransformToward(transform, End);
        
	}
}
