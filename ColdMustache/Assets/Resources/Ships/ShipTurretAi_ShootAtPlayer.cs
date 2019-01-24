using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTurretAi_ShootAtPlayer : MonoBehaviour {

    ShipTurret turret;

    public Transform Target;

    public Ship ship;

	// Use this for initialization
	void Start () {
        turret = GetComponent<ShipTurret>();
	}
	
	// Update is called once per frame
	void Update () {


        Vector2 target = (Vector2)Target.position;
        float Angle = Vector2.SignedAngle(ship.RelativeBow, target - (Vector2)transform.position);  
        if (Mathf.Abs(Angle) < 10)
        {

            turret.TryShooting(BattleflyMovementManager.GetPositionInSeconds((transform.position - Target.position).magnitude / 500));

        }



        
	}
}
