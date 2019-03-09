using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnwalkableAreaFromMap : MonoBehaviour
{
    public Texture2D Map;


    public bool BlockMovement = true;
    public bool BlockLight = false;
    public bool BlockExplosions = false;

    public enum Modes {TransparentIsUnwalkable, OpaqueIsUnwalkable}

    public Modes Mode = Modes.TransparentIsUnwalkable;

    public float AlphaThreshold = 0.1f;

    void Start()
    {

        GenerateGrassLayer();

    }

    void GenerateGrassLayer()
    {
        UnityEngine.Color[] all = Map.GetPixels();

        for (int y = 0; y < Map.height; y++)
        {
            for (int x = 0; x < Map.width; x++)
            {
                Color pixel = all[y * Map.width + x];

                if(Mode == Modes.TransparentIsUnwalkable && pixel.a <= AlphaThreshold ||
                    Mode == Modes.OpaqueIsUnwalkable && pixel.a >= AlphaThreshold
                ){
                    if (BlockMovement) NavTestStatic.NavArray[x, y] = NavTestStatic.ImpassableTileValue;
                    if (BlockLight) NavTestStatic.LightNavArray[x, y] = NavTestStatic.ImpassableTileValue;
                    if (BlockExplosions) NavTestStatic.ExplosionNavArray[x, y] = NavTestStatic.ImpassableTileValue;
                }

            }
        }
    }

}
