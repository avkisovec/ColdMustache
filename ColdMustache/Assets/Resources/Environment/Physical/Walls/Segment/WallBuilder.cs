using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuilder : MonoBehaviour
{
    public bool ENABLED = true;
    public Texture2D Map;
    public string WallPrefabPath = "";


    // Start is called before the first frame update
    void Start()
    {
        if (!ENABLED) return;

        Generate();

        ENABLED = false;
    }

    void Generate()
    {
        UnityEngine.Color[] all = Map.GetPixels();

        WallSegment CurrWallSegment = null;
        //false if im currently in a segment, true if im outside of one and need to create one for new walls
        bool DoINeedANewSegment = true;

        for (int x = 0; x < Map.width; x++)
        {
            for (int y = 0; y < Map.height; y++)
            {


                if (all[y * Map.width + x].a != 0)
                {
                    if(DoINeedANewSegment){
                        CurrWallSegment = GetNewSegment();
                        DoINeedANewSegment = false;
                    }

                    GameObject go = Instantiate(Resources.Load<GameObject>(WallPrefabPath) as GameObject);
                    go.transform.position = new Vector3(x,y,0);
                    go.transform.parent = CurrWallSegment.transform;

                    /*

                    GameObject grass = new GameObject();
                    grass.AddComponent<SpriteRenderer>().color = all[y * Map.width + x];
                    grass.transform.position = new Vector3(x, y, Zposition);
                    grass.name = MyDistanceFromEdge.ToString();

                    grass.GetComponent<SpriteRenderer>().sprite = SpriteSheets[MyDistanceFromEdge][Random.Range(0, SpriteSheets[MyDistanceFromEdge].Length)];

                    grass.transform.parent = transform;

                    */
                }
                else{
                    DoINeedANewSegment = true;
                }



            }
        }
    }


    WallSegment GetNewSegment(){

        GameObject go = new GameObject();
        go.transform.position = new Vector3(0,0,0);
        go.transform.localScale = new Vector3(1,1,1);
        go.transform.parent = transform;
        WallSegment ws = go.AddComponent<WallSegment>();

        ws.UseChildrenAsTiles = true;
        ws.NewParentForTiles = transform;
        
        return ws;

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
