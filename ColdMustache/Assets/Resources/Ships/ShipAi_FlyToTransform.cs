using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAi_FlyToTransform : MonoBehaviour {

    public Transform Target;

    Ship ship;

    Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
        ship = GetComponent<Ship>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {


        Vector2 target = (Vector2)Target.position;
        //Debug.DrawLine(ship.RelativeBow, target);

        //bow vector
        float Angle = Vector2.SignedAngle(ship.RelativeBow, target - (Vector2)transform.position);

        //target is port
        if (Angle > 5)
        {
            if (2 * Angle > rb.angularVelocity)
            {
                ship.TurnCounterClockwise();
            }
            else
            {
                ship.TurnClockwise();
            }
            //ship.StrafePortside();
        }
        //target is starboard
        else if (Angle < -5)
        {
            if (2 * Angle < rb.angularVelocity)
            {
                ship.TurnClockwise();
            }
            else
            {
                ship.TurnCounterClockwise();
            }
            //ship.StrafeStarboardSide();
        }

        if(Angle > 10 && Angle < 170)
        {
            ship.StrafePortside();
        }
        if(Angle < -10 && Angle > -170)
        {
            ship.StrafeStarboardSide();
        }

        if (Mathf.Abs(Angle) < 10)
        {
            ship.MoveForward();

            float angle2 = Vector2.SignedAngle(rb.velocity, target - (Vector2)transform.position);
            //Debug.Log(angle2);
            if (Mathf.Abs(angle2) > 2)
                if (angle2 > 0)
                {
                    ship.StrafePortside();
                }
                else
                {
                    ship.StrafeStarboardSide();
                }
        }



    }
}
