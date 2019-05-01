using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decorator : MonoBehaviour
{

    /*

        this script coud be called "AttatchSelfToAWall"

        

        object with this script attatches itself (becomes child of) to an environmentObject (wall) it collides with

        this is for things like terminals lights etc on walls, so that they can be hand placed, but get destroyed when the wall does
    
    
    
     */


    //z coordinate relative to the wall it becomes a part of (should be negative)
    public float RelativeZ = -0.01f;

    void Start(){


        Vector2Int IntCoordinates = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

        transform.parent = NavTestStatic.WallTransformsArray[IntCoordinates.x,IntCoordinates.y];
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, RelativeZ);

    }

}
