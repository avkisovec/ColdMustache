using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : CentrallyUpdatable {

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
        CentralUpdater.Scripts.Add(this);
    }
    public override void CentralUpdate()
    {
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
