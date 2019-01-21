using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {

    public bool AllowGoingUp;
    public Vector3 TargetPositionGoingUp;

    public bool AllowGoingDown;
    public Vector3 TargetPositionGoingDown;

    public Sprite SpriteDefault;
    public Sprite SpriteActivatable;

    SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        if ((UniversalReference.PlayerObject.transform.position - transform.position).magnitude < 2)
        {
            sr.sprite = SpriteActivatable;

            if (Input.GetKeyDown(KeyCode.KeypadPlus) && AllowGoingUp)
            {
                UniversalReference.PlayerObject.transform.position = TargetPositionGoingUp;

            }
            if (Input.GetKeyDown(KeyCode.KeypadMinus) && AllowGoingDown)
            {
                UniversalReference.PlayerObject.transform.position = TargetPositionGoingDown;

            }
        }
        else
        {
            sr.sprite = SpriteDefault;
        }
    }
}
