using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipThrusterless : Ship {

    /*
     * this script is for things like homing rocket
     * 
     * they behave like ships and have similar movement, but without things like thruster fumes
     * 
     * 
     */
     	
	// Update is called once per frame
	void Update () {
        BaseUpdate();
	}

    public override void MoveForward()
    {
        rb.AddForce(RelativeBow * ForwardSpeed * Throttle * Time.deltaTime);
    }

    public override void StrafePortside()
    {
        rb.AddForce(RelativePort * StrafingSpeed * Throttle * Time.deltaTime);
    }

    public override void StrafeStarboardSide()
    {
        rb.AddForce(RelativeStarboard * StrafingSpeed * Throttle * Time.deltaTime);
    }

    public override void TurnClockwise()
    {
        rb.angularVelocity -= TurningSpeed * Throttle * Time.deltaTime;
    }

    public override void TurnCounterClockwise()
    {
        rb.angularVelocity += TurningSpeed * Throttle * Time.deltaTime;
    }
}
