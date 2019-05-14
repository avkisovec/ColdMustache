using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObjectLargerThan1x1 : EnvironmentObject
{


    public int AdditionalTilesAbovePivot = 0;
    public int AdditionalTilesBelowPivot = 0;
    public int AdditionalTilesLeftOfPivot = 0;
    public int AdditionalTilesRightOfPivot = 0;

    void Start()
    {

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
                if (NavTestStatic.IsTileWithinBounds(new Vector2Int(x, y)))
                {
                    if (!Nav_Walkable) NavTestStatic.NavArray[x, y] = NavTestStatic.ImpassableTileValue;
                    if (!Nav_LightCanPass) NavTestStatic.LightNavArray[x, y] = NavTestStatic.ImpassableTileValue;
                    if (!Nav_ExplosionCanPass) NavTestStatic.ExplosionNavArray[x, y] = NavTestStatic.ImpassableTileValue;
                }
            }
        }

        base.CustomStart();
    }

    public override void Die(){


        int minX = posX - AdditionalTilesLeftOfPivot;
        int maxX = posX + AdditionalTilesRightOfPivot;
        int minY = posY - AdditionalTilesBelowPivot;
        int maxY = posY + AdditionalTilesAbovePivot;

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                if (NavTestStatic.IsTileWithinBounds(new Vector2Int(x, y)))
                {
                    if (!Nav_Walkable) NavTestStatic.NavArray[x, y] = NavTestStatic.EmptyTileValue;
                    if (!Nav_LightCanPass) NavTestStatic.LightNavArray[x, y] = NavTestStatic.EmptyTileValue;
                    if (!Nav_ExplosionCanPass) NavTestStatic.ExplosionNavArray[x, y] = NavTestStatic.EmptyTileValue;
                }
            }
        }

        if (DeathRemains != null)
        {
            GameObject remains = new GameObject();
            remains.transform.position = new Vector3(transform.position.x, transform.position.y, ZIndexManager.Const_Floors - 1);
            remains.AddComponent<SpriteRenderer>().sprite = DeathRemains;
            remains.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        }
        Health = 999999999; //this is to make sure object doesnt die twice - if multiple hits land and once, while target if below 0 hp, each new hit is hit bbringing him below 0hp therefore spawning new corpse etc
        Destroy(this.gameObject);

        return;

    }


}
