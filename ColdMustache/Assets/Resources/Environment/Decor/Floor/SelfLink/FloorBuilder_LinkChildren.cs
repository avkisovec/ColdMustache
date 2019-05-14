using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBuilder_LinkChildren : MonoBehaviour
{

    /*
    
    based on FloorBuilderSelfLinkable

    but it doesnt load an image map, instead it takes all its children and builds a map from them
        
    
     */



    public bool ENABLED = true;

    public string SpritesheetPath;


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

        foreach(SpriteRenderer sr in transform.GetComponentsInChildren<SpriteRenderer>()){
            Vector2Int v = Util.Vector3To2Int(sr.transform.position);
            Map[v.x, v.y] = sr;

            if(v.x > HighestX) HighestX = v.x;
            if(v.x < LowestX) LowestX = v.x;
            if(v.y > HighestY) HighestY = v.y;
            if(v.y < LowestY) LowestY = v.y;
        }






        for (int y = LowestY; y <= HighestY; y++)
        {
            for (int x = LowestX; x <= HighestX; x++)
            {
                //empty tiles are ignored
                if (Map[x,y] == null) continue;


                bool ConnectedRight = false;
                bool ConnectedRightUp = false;
                bool ConnectedUp = false;
                bool ConnectedLeftUp = false;
                bool ConnectedLeft = false;
                bool ConnectedLeftDown = false;
                bool ConnectedDown = false;
                bool ConnectedRightDown = false;
                int Connections = 0;


                //right
                if (IsTileWithinMapBounds(x + 1, y)) { if (Map[x+1, y] != null) { ConnectedRight = true; Connections++; } }
                //right up
                if (IsTileWithinMapBounds(x + 1, y + 1)) { if (Map[x+1, y+1] != null) { ConnectedRightUp = true; Connections++; } }
                //up
                if (IsTileWithinMapBounds(x, y + 1)) { if (Map[x, y+1] != null) { ConnectedUp = true; Connections++; } }
                //left up
                if (IsTileWithinMapBounds(x - 1, y + 1)) { if (Map[x-1, y+1] != null) { ConnectedLeftUp = true; Connections++; } }
                //left
                if (IsTileWithinMapBounds(x - 1, y)) { if (Map[x-1,y] != null) { ConnectedLeft = true; Connections++; } }
                //left down
                if (IsTileWithinMapBounds(x - 1, y - 1)) { if (Map[x-1, y-1] != null) { ConnectedLeftDown = true; Connections++; } }
                //down
                if (IsTileWithinMapBounds(x, y - 1)) { if (Map[x, y-1] != null) { ConnectedDown = true; Connections++; } }
                //right down
                if (IsTileWithinMapBounds(x + 1, y - 1)) { if (Map[x+1, y-1] != null) { ConnectedRightDown = true; Connections++; } }

                //Map[x,y].gameObject.name = Connections.ToString();

                #region Center

                if (Connections == 8)
                {
                    Map[x, y].sprite = Sprites[0];
                    continue;
                }

                #endregion


                #region InnerCorners

                //inner corners
                if (Connections == 7)
                {
                    if (!ConnectedRightUp)
                    {
                        Map[x, y].sprite = Sprites[9];
                        continue;
                    }
                    else if (!ConnectedLeftUp)
                    {
                        Map[x, y].sprite = Sprites[10];
                        continue;
                    }
                    else if (!ConnectedLeftDown)
                    {
                        Map[x, y].sprite = Sprites[11];
                        continue;
                    }
                    else if (!ConnectedRightDown)
                    {
                        Map[x, y].sprite = Sprites[12];
                        continue;
                    }
                }

                #endregion



                #region OuterCorners

                //outer corners
                if (true)
                {
                    if (!ConnectedRight && !ConnectedUp)
                    {
                        Map[x, y].sprite = Sprites[5];
                        continue;
                    }
                    else if (!ConnectedUp && !ConnectedLeft)
                    {
                        Map[x, y].sprite = Sprites[6];
                        continue;
                    }
                    else if (!ConnectedLeft && !ConnectedDown)
                    {
                        Map[x, y].sprite = Sprites[7];
                        continue;
                    }
                    else if (!ConnectedDown && !ConnectedRight)
                    {
                        Map[x, y].sprite = Sprites[8];
                        continue;
                    }
                }

                #endregion



                #region Edges

                if (true)
                {

                    //right edge
                    if (!ConnectedRight)
                    {
                        Map[x, y].sprite = Sprites[1];
                        continue;
                    }

                    //top edges
                    if (!ConnectedUp)
                    {
                        Map[x, y].sprite = Sprites[2];
                        continue;
                    }

                    //left edges
                    if (!ConnectedLeft)
                    {
                        Map[x, y].sprite = Sprites[3];
                        continue;
                    }

                    //right edges
                    if (!ConnectedDown)
                    {
                        Map[x, y].sprite = Sprites[4];
                        continue;
                    }

                }

                #endregion

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
