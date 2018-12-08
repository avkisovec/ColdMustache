using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassMoveWhenWalked : MonoBehaviour {

    public float AngleLimit = 60;
    public float AngularVelocity = 15; //maybe per second
    public float BaseAngularVelocityRemaining;
    float AngularMovementRemaining = 0;
    bool IsMovementNegative = false;
    float TargetAngle = 0; //this is to avoid modulo problems with transform's rotation

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(1 * Time.deltaTime*60);
        if(AngularMovementRemaining > 0)
        {
            float ThisFrameAngularVelocity = AngularVelocity * Time.deltaTime;
            AngularMovementRemaining -= ThisFrameAngularVelocity;


            //positive movement
            if (!IsMovementNegative)
            {
                if (TargetAngle < AngleLimit)
                {
                    TargetAngle += ThisFrameAngularVelocity;
                }
                else
                {
                    IsMovementNegative = true;
                }
            }
            else if (IsMovementNegative)
            {
                if (TargetAngle > -AngleLimit)
                {
                    TargetAngle -= ThisFrameAngularVelocity;
                }
                else
                {
                    IsMovementNegative = false;
                }
            }



            transform.rotation = Quaternion.Euler(0, 0, TargetAngle);
        }
	}

    private void OnTriggerEnter2D(Collider2D coll)
    {
        Entity hit = coll.GetComponent<Entity>();
        if (hit != null)
        {
            AngularMovementRemaining+=BaseAngularVelocityRemaining;
            //coll.gameObject.AddComponent<FxFootSteps>().ini(LifespanInSeconds, CooldownInSeconds, sprite, color);
        }
    }
}
