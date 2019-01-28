using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleflyMovementManager : MonoBehaviour {

    static Ship ship;
    static Rigidbody2D rb;
    static Transform tr;

	// Use this for initialization
	void Start () {
        ship = GetComponent<Ship>();
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
	}
	
    public static Vector2 GetPositionInSeconds(float Seconds)
    {
        return (Vector2)tr.position + (rb.velocity * Seconds);
    }

    public static Vector2 GetCollisionCourse(float VelocityMagnitude)
    {
        return (Vector2)tr.position + (rb.velocity / VelocityMagnitude);
    }
}
