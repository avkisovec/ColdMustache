using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBuilder_ChildrenLineLink : MonoBehaviour
{

    /*
    
    based on FloorBuilderSelfLinkable

    but it doesnt load an image map, instead it takes all its children and builds a map from them
        
    
     */



    public bool ENABLED = true;

    public string SpritesheetPath = "Undefined";


    void Start()
    {
        if (!ENABLED) return;

        Generate();

        ENABLED = false;
    }

    void Generate()
    {

        Sprite[] Sprites = Resources.LoadAll<Sprite>(SpritesheetPath);

        SpriteRenderer[,] Map = new SpriteRenderer[NavTestStatic.MapHeight, NavTestStatic.MapWidth];

        int HighestX = -999;
        int HighestY = -999;
        int LowestX = 9999;
        int LowestY = 9999;

        foreach (SpriteRenderer sr in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Vector2Int v = Util.Vector3To2Int(sr.transform.position);
            Map[v.x, v.y] = sr;

            if (v.x > HighestX) HighestX = v.x;
            if (v.x < LowestX) LowestX = v.x;
            if (v.y > HighestY) HighestY = v.y;
            if (v.y < LowestY) LowestY = v.y;
        }



        for (int y = LowestY; y <= HighestY; y++)
        {
            for (int x = LowestX; x <= HighestX; x++)
            {
                //empty tiles are ignored
                if (Map[x, y] == null) continue;


                bool ConnectedRight = false;
                bool ConnectedUp = false;
                bool ConnectedLeft = false;
                bool ConnectedDown = false;

                //right
                if (IsTileWithinMapBounds(x + 1, y)) { if (Map[x + 1, y] != null) { ConnectedRight = true; } }
                //up
                if (IsTileWithinMapBounds(x, y + 1)) { if (Map[x, y + 1] != null) { ConnectedUp = true; } }
                //left
                if (IsTileWithinMapBounds(x - 1, y)) { if (Map[x - 1, y] != null) { ConnectedLeft = true; } }
                //down
                if (IsTileWithinMapBounds(x, y - 1)) { if (Map[x, y - 1] != null) { ConnectedDown = true; } }
                
                
                Map[x,y].sprite = WallSpriteFinder.Find(Sprites, ConnectedRight, ConnectedUp, ConnectedLeft, ConnectedDown);


            }
        }
    }

    bool IsTileWithinMapBounds(int x, int y)
    {
        if (x >= 0 &&
            x < NavTestStatic.MapWidth &&
            y >= 0 &&
            y < NavTestStatic.MapHeight
            )
        {
            return true;
        }
        return false;
    }


}
