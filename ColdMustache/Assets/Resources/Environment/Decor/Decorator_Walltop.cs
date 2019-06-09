using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decorator_Walltop : MonoBehaviour
{

    /*

        this object will destroy itself if there no longer is a walltop on its coordinates    
    
     */


    //z coordinate relative to the wall it becomes a part of (should be negative)
    Vector2Int TilePos;

    void Start()
    {
        TilePos = Util.Vector3To2Int(transform.position);

    }

    void Update()
    {
        if (!NavTestStatic.IsTileWallTop(TilePos.x, TilePos.y)) Destroy(gameObject);
    }

}
