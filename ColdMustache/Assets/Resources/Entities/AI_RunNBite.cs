using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_RunNBite : MonoBehaviour {

    Entity e;
    Transform target;
    WaypointNavigator wn;

    public int FramesSincePlayerWasObserved = 999999;

	// Use this for initialization
	void Start () {
        e = GetComponent<Entity>();
        target = UniversalReference.PlayerObject.transform;
        wn = GetComponent<WaypointNavigator>();
	}

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (NavTestStatic.CheckLineOfSight(Util.Vector3To2Int(transform.position), Util.Vector3To2Int(target.position)))
        {
            FramesSincePlayerWasObserved = 0;

            wn.WayPoints = new List<Vector2>();
            wn.WayPoints.Add(target.position);
        }

        if (FramesSincePlayerWasObserved > 1 && FramesSincePlayerWasObserved < 180 && wn.WayPoints.Count <= 1)
        {
            wn.WayPoints = NavTestStatic.FindAPath(Util.Vector3To2Int(transform.position), Util.Vector3To2Int(target.position));
        }

        if (FramesSincePlayerWasObserved < 1000)
        {
            FramesSincePlayerWasObserved++;
        }
    }
    
}
