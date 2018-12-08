using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigator : MonoBehaviour {

    public List<Vector2> WayPoints;

    public bool FromLastToFirst = true;

    Entity e;

	// Use this for initialization
	void Start () {
        e = GetComponent<Entity>();
	}
	
	// Update is called once per frame
	void Update () {

        if (FromLastToFirst)
        {
            if(WayPoints.Count >= 1)
            {
                if (((Vector2)transform.position - WayPoints[WayPoints.Count - 1]).magnitude > 0.3f)
                {
                    e.MoveInDirection(WayPoints[WayPoints.Count - 1] - (Vector2)transform.position);
                }
                else if(WayPoints.Count > 1)
                {
                    WayPoints.RemoveAt(WayPoints.Count - 1);
                    e.MoveInDirection(WayPoints[WayPoints.Count - 1] - (Vector2)transform.position);
                }
                else
                {
                    e.MoveInDirection(new Vector2(0, 0));
                }
            }
            else
            {
                e.MoveInDirection(new Vector2(0, 0));
            }
        }

	}
}
