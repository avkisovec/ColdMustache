using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decorator : MonoBehaviour
{

    /*

        this script coud be called "AttatchSelfToAWall"

        
        OUTDATED:
        object with this script attatches itself (becomes child of) to an environmentObject (wall) it collides with
        this is for things like terminals lights etc on walls, so that they can be hand placed, but get destroyed when the wall does
    
        EDIT:
        uses kind of lifelink of environment object on same coordinates as itself
    
    
     */


    //z coordinate relative to the wall it becomes a part of (should be negative)
    Transform Link = null;

    void Start(){


        Vector2Int IntCoordinates = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

        Link = NavTestStatic.WallTransformsArray[IntCoordinates.x, IntCoordinates.y];


    }

    void Update()
    {
        if (Link == null) Destroy(gameObject);
    }

}
