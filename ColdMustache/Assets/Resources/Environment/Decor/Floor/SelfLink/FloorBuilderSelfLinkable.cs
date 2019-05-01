using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBuilderSelfLinkable : MonoBehaviour
{

    /*
    
        HOW THIS BUILDER WORKS

    for every opaque tile (based on cutoff), it spawns a tile
    the sprite is selected based on the indexes

    tiles will link to one another

    tiles will also link to semi transparent pixels, though no tiles will spawn on semi-transparent
    (tiles made from opaque pixels link as if there was a tile on semi-transparent pixels)
    
    
    
    
     */


    /*
    sprites indexes:

    13 in total:

    0   center

    1   right edge
    2   top edge
    3   left edge
    4   bot edge

    5   right top outer corner
    6   top left outer corner
    7   left bot outer corner
    8   bot right outer corner

    9   right top inner corner
    10  top left inner corner
    11  left bot inner corner
    12  bot right inner corner  
    
    
    
    
     */

    public bool ENABLED = true;

    public Texture2D Map;

    public string SpritesheetPath;

    public Transform TilesParent;

    // Start is called before the first frame update
    void Start()
    {
        if (!ENABLED) return;

        GenerateDirtLayer();
        
        ENABLED = false;
    }

    void GenerateDirtLayer()
    {
        
        Sprite[] Sprites = Resources.LoadAll<Sprite>(SpritesheetPath);

        //anything below this value is considered transparent
        float AlphaCutoffFullyTransparent = 0.1f;

        //pixels inbetween these values are considered semi-transparent
        //no tiles will spawn on semi-transparent but tiles will link to them

        //anything above this value is considered opaque
        float AlphaCutoffFullyOpaque = 0.9f;

        UnityEngine.Color[] all = Map.GetPixels();

        for (int y = 0; y < Map.height; y++)
        {
            for (int x = 0; x < Map.width; x++)
            {
                //transparent tiles are ignored
                if(all[y * Map.width + x].a < AlphaCutoffFullyOpaque ) continue;
                
                
                bool ConnectedRight = false;
                bool ConnectedRightUp = false;
                bool ConnectedUp = false;
                bool ConnectedLeftUp = false;
                bool ConnectedLeft = false;
                bool ConnectedLeftDown = false;
                bool ConnectedDown = false;
                bool ConnectedRightDown = false;
                int Connections = 0;

                Color MapPixel = all[y * Map.width + x];
                Color clr = MapPixel;

                //right
                if (IsTileWithinMapBounds(x + 1, y)) { if (all[y * Map.width + x + 1].a > AlphaCutoffFullyTransparent) { ConnectedRight = true; Connections++; } }
                //right up
                if (IsTileWithinMapBounds(x + 1, y + 1)) { if (all[(y + 1) * Map.width + x + 1].a > AlphaCutoffFullyTransparent) { ConnectedRightUp = true; Connections++;  } }
                //up
                if (IsTileWithinMapBounds(x, y + 1)) { if (all[(y + 1) * Map.width + x].a > AlphaCutoffFullyTransparent) { ConnectedUp = true; Connections++;  } }
                //left up
                if (IsTileWithinMapBounds(x - 1, y + 1)) { if (all[(y + 1) * Map.width + x - 1].a > AlphaCutoffFullyTransparent) { ConnectedLeftUp = true; Connections++;  } }
                //left
                if (IsTileWithinMapBounds(x - 1, y)) { if (all[y * Map.width + x - 1].a > AlphaCutoffFullyTransparent) { ConnectedLeft = true; Connections++;  } }
                //left down
                if (IsTileWithinMapBounds(x - 1, y - 1)) { if (all[(y - 1) * Map.width + x - 1].a > AlphaCutoffFullyTransparent) { ConnectedLeftDown = true; Connections++;  } }
                //down
                if (IsTileWithinMapBounds(x, y - 1)) { if (all[(y - 1) * Map.width + x].a > AlphaCutoffFullyTransparent) { ConnectedDown = true; Connections++;  } }
                //right down
                if (IsTileWithinMapBounds(x + 1, y - 1)) { if (all[(y - 1) * Map.width + x + 1].a > AlphaCutoffFullyTransparent) { ConnectedRightDown = true; Connections++; } }


                #region Center

                if(Connections == 8)
                {
                    GameObject center = new GameObject();
                    center.AddComponent<SpriteRenderer>().color = clr;
                    center.GetComponent<SpriteRenderer>().sprite = Sprites[0];
                    center.transform.position = new Vector3(x, y, TilesParent.position.z);
                    center.transform.parent = TilesParent;
                    continue;
                }

                #endregion


                #region InnerCorners

                //inner corners
                if (Connections == 7)
                {
                    if (!ConnectedRightUp)
                    {
                        GameObject center = new GameObject();
                        center.AddComponent<SpriteRenderer>().color = clr;
                        center.GetComponent<SpriteRenderer>().sprite = Sprites[9];
                        center.transform.position = new Vector3(x, y, TilesParent.position.z);
                        center.transform.parent = TilesParent;
                        continue;
                    }
                    else if (!ConnectedLeftUp)
                    {
                        GameObject center = new GameObject();
                        center.AddComponent<SpriteRenderer>().color = clr;
                        center.GetComponent<SpriteRenderer>().sprite = Sprites[10];
                        center.transform.position = new Vector3(x, y, TilesParent.position.z);
                        center.transform.parent = TilesParent;
                        continue;
                    }
                    else if (!ConnectedLeftDown)
                    {
                        GameObject center = new GameObject();
                        center.AddComponent<SpriteRenderer>().color = clr;
                        center.GetComponent<SpriteRenderer>().sprite = Sprites[11];
                        center.transform.position = new Vector3(x, y, TilesParent.position.z);
                        center.transform.parent = TilesParent;
                        continue;
                    }
                    else if (!ConnectedRightDown)
                    {
                        GameObject center = new GameObject();
                        center.AddComponent<SpriteRenderer>().color = clr;
                        center.GetComponent<SpriteRenderer>().sprite = Sprites[12];
                        center.transform.position = new Vector3(x, y, TilesParent.position.z);
                        center.transform.parent = TilesParent;
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
                        GameObject center = new GameObject();
                        center.AddComponent<SpriteRenderer>().color = clr;
                        center.GetComponent<SpriteRenderer>().sprite = Sprites[5];
                        center.transform.position = new Vector3(x, y, TilesParent.position.z);
                        center.transform.parent = TilesParent;
                        continue;
                    }
                    else if (!ConnectedUp && !ConnectedLeft)
                    {
                        GameObject center = new GameObject();
                        center.AddComponent<SpriteRenderer>().color = clr;
                        center.GetComponent<SpriteRenderer>().sprite = Sprites[6];
                        center.transform.position = new Vector3(x, y, TilesParent.position.z);
                        center.transform.parent = TilesParent;
                        continue;
                    }
                    else if (!ConnectedLeft && !ConnectedDown)
                    {
                        GameObject center = new GameObject();
                        center.AddComponent<SpriteRenderer>().color = clr;
                        center.GetComponent<SpriteRenderer>().sprite = Sprites[7];
                        center.transform.position = new Vector3(x, y, TilesParent.position.z);
                        center.transform.parent = TilesParent;
                        continue;
                    }
                    else if (!ConnectedDown && !ConnectedRight)
                    {
                        GameObject center = new GameObject();
                        center.AddComponent<SpriteRenderer>().color = clr;
                        center.GetComponent<SpriteRenderer>().sprite = Sprites[8];
                        center.transform.position = new Vector3(x, y, TilesParent.position.z);
                        center.transform.parent = TilesParent;
                        continue;
                    }
                }

                #endregion



                #region Edges

                if(true){

                    //right edge
                    if (!ConnectedRight)
                    {
                        GameObject center = new GameObject();
                        center.AddComponent<SpriteRenderer>().color = clr;
                        center.GetComponent<SpriteRenderer>().sprite = Sprites[1];
                        center.transform.position = new Vector3(x, y, TilesParent.position.z);
                        center.transform.parent = TilesParent;
                        continue;
                    }

                    //top edges
                    if (!ConnectedUp)
                    {
                        GameObject center = new GameObject();
                        center.AddComponent<SpriteRenderer>().color = clr;
                        center.GetComponent<SpriteRenderer>().sprite = Sprites[2];
                        center.transform.position = new Vector3(x, y, TilesParent.position.z);
                        center.transform.parent = TilesParent;
                        continue;
                    }

                    //left edges
                    if (!ConnectedLeft)
                    {
                        GameObject center = new GameObject();
                        center.AddComponent<SpriteRenderer>().color = clr;
                        center.GetComponent<SpriteRenderer>().sprite = Sprites[3];
                        center.transform.position = new Vector3(x, y, TilesParent.position.z);
                        center.transform.parent = TilesParent;
                        continue;
                    }

                    //right edges
                    if (!ConnectedDown)
                    {
                        GameObject center = new GameObject();
                        center.AddComponent<SpriteRenderer>().color = clr;
                        center.GetComponent<SpriteRenderer>().sprite = Sprites[4];
                        center.transform.position = new Vector3(x, y, TilesParent.position.z);
                        center.transform.parent = TilesParent;
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
            x < Map.width &&
            y >= 0 &&
            y < Map.height
            )
        {
            return true;
        }
        return false;
    }

}
