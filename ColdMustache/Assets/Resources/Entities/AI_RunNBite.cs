﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_RunNBite : AI_Base {

    Entity e;
    Transform target;
    WaypointNavigator wn;

    public int FramesSincePlayerWasObserved = 999999;

    public int DisallowWaypointChangeForFrames = 0;

	// Use this for initialization
	void Start () {
        e = GetComponent<Entity>();
        target = UniversalReference.PlayerObject.transform;
        wn = GetComponent<WaypointNavigator>();
        wn.WayPoints = new List<Vector2>();
	}


    int FramesSinceTilePosSample = 0;
    Vector2Int TilePosSample = new Vector2Int(0,0);

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (DisallowWaypointChangeForFrames <= 0)
        {

            Vector2Int MyPos = Util.Vector3To2Int(transform.position);
            Vector2Int TargetPos = Util.Vector3To2Int(target.position);






            //line of sight toward target (player) - null if obstructed
            List<Vector2Int> LineOfSight = NavTestStatic.GetLineOfSightOptimised(MyPos, TargetPos);

            //if you have a vision of player
            if (LineOfSight != null)
            {
                FramesSincePlayerWasObserved = 0;

                //you have a vision and clear path in front of you - run at him
                if (NavTestStatic.IsPathWalkable(LineOfSight))
                {
                    wn.WayPoints.Clear();
                    wn.WayPoints.Add(target.position);
                }

                //you have a vision but something is obstructing your path (like a hole) - pathfind to him
                else
                {
                    List<Vector2> path = NavTestStatic.FindAPath(MyPos, TargetPos);
                    if (path != null)
                    {
                        wn.WayPoints = path;
                    }
                    DisallowWaypointChangeForFrames = 10;
                }
            }

            //you have recently seen the player, and you went to his last seen position - for some time you can now "sense" his position - pathfind toward him
            else if (FramesSincePlayerWasObserved > 1 && FramesSincePlayerWasObserved < 180 && wn.WayPoints.Count <= 1)
            {
                List<Vector2> path = NavTestStatic.FindAPath(MyPos, TargetPos);
                if (path != null)
                {
                    wn.WayPoints = path;
                }
            }

            else
            {
            
                if(wn.WayPoints.Count <= 1){
                    List<Vector2> path = NavTestStatic.FindAPath(MyPos, MyPos + new Vector2Int(Random.Range((int)-5, (int)6), Random.Range((int)-5, (int)6)));
                    if (path != null)
                    {
                        wn.WayPoints = path;
                    }
                }

            }

            /*

            make sample of your tile position every n frames
            if your position havent changed, pathfind to some new place randomly

            */

            if (FramesSinceTilePosSample > 10)
            {
                if (TilePosSample == MyPos)
                {
                    List<Vector2> path = NavTestStatic.FindAPath(MyPos, MyPos + new Vector2Int(Random.Range((int)-5, (int)6), Random.Range((int)-5, (int)6)));
                    if (path != null)
                    {
                        wn.WayPoints = path;
                    }
                }

                FramesSinceTilePosSample = 0;
                TilePosSample = MyPos;

            }
            else
            {
                FramesSinceTilePosSample++;
            }


        }

        if (FramesSincePlayerWasObserved < 1000)
        {
            FramesSincePlayerWasObserved++;
        }
        if(DisallowWaypointChangeForFrames > 0)
        {
            DisallowWaypointChangeForFrames--;
        }
    }
    
}
