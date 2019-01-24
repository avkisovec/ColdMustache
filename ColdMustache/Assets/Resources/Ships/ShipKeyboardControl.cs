using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipKeyboardControl : MonoBehaviour {

    Ship ship;
    
	// Use this for initialization
	void Start () {
		if(ship == null)
        {
            ship = GetComponent<Ship>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKey(KeyCode.Q))
        {
            ship.TurnCounterClockwise();
        }
        if (Input.GetKey(KeyCode.E))
        {
            ship.TurnClockwise();
        }
        if (Input.GetKey(KeyCode.W))
        {
            ship.MoveForward();
        }
        if (Input.GetKey(KeyCode.A))
        {
            ship.StrafePortside();
        }
        if (Input.GetKey(KeyCode.D))
        {
            ship.StrafeStarboardSide();
        }

    }
}
