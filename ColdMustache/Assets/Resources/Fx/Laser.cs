using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public Vector3 Origin;

    public Vector3 End;



	// Use this for initialization
	void Start () {


        transform.position = Origin + ((End - Origin) / 2);
        transform.localScale = new Vector3((End - Origin).magnitude * 32, 1, 1);

        Util.RotateTransformToward(transform, End);


	}
	
	// Update is called once per frame
	void Update () {

    }
}
