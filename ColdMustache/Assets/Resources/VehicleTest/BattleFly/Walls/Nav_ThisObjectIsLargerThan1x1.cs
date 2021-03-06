﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nav_ThisObjectIsLargerThan1x1 : MonoBehaviour {

    public bool ObstructsMovement = true;
    public bool ObstructsVision = true;
    public bool ObstructsExplosions = false;

    public int AdditionalTilesAbovePivot = 0;
    public int AdditionalTilesBelowPivot = 0;
    public int AdditionalTilesLeftOfPivot = 0;
    public int AdditionalTilesRightOfPivot = 0;

    // Use this for initialization
    void Start () {

        int origX = Mathf.RoundToInt(transform.position.x);
        int origY = Mathf.RoundToInt(transform.position.y);

        int minX = origX - AdditionalTilesLeftOfPivot;
        int maxX = origX + AdditionalTilesRightOfPivot;
        int minY = origY - AdditionalTilesBelowPivot;
        int maxY = origY + AdditionalTilesAbovePivot;
        
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                if (NavTestStatic.IsTileWithinBounds(new Vector2Int(x,y))){
                    if (ObstructsMovement) NavTestStatic.NavArray[x, y] = NavTestStatic.ImpassableTileValue;
                    if (ObstructsVision) NavTestStatic.LightNavArray[x, y] = NavTestStatic.ImpassableTileValue;
                    if (ObstructsExplosions) NavTestStatic.ExplosionNavArray[x, y] = NavTestStatic.ImpassableTileValue;
                }                
            }
        }

        Destroy(GetComponent<Nav_ThisObjectIsLargerThan1x1>());
    }
}
