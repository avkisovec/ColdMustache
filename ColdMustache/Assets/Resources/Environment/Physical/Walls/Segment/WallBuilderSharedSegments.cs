using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuilderSharedSegments : MonoBehaviour
{

    /*
    
    variation of the original wallbuilder
    this one can have several maps and a prefab for each, but they all share the same segments

    all maps better be the same size
    
     */



    public bool ENABLED = true;
    public Texture2D[] Maps;
    public string[] WallPrefabPaths;


    // Start is called before the first frame update
    void Start()
    {
        if (!ENABLED) return;

        Generate();

        ENABLED = false;
    }

    void Generate()
    {

        UnityEngine.Color[][] all = new UnityEngine.Color[Maps.Length][];

        for(int i = 0; i < Maps.Length; i++){
            all[i] = Maps[i].GetPixels();
        }



        WallSegment CurrWallSegment = null;
        //false if im currently in a segment, true if im outside of one and need to create one for new walls
        bool DoINeedANewSegment = true;

        for (int x = 0; x < Maps[0].width; x++)
        {
            for (int y = 0; y < Maps[0].height; y++)
            {


                if (DoINeedANewSegment)
                {
                    CurrWallSegment = GetNewSegment();
                }

                //true now, if at least one map doesnt need it then it becomes false for these coordinates
                DoINeedANewSegment = true;

                //go through all pixels on these coordinates
                for(int CurrMapIndex = 0; CurrMapIndex < Maps.Length; CurrMapIndex++)
                {
                    if (all[CurrMapIndex][y * Maps[0].width + x].a != 0)
                    {
                        DoINeedANewSegment = false;

                        GameObject go = Instantiate(Resources.Load<GameObject>(WallPrefabPaths[CurrMapIndex]) as GameObject);
                        go.transform.position = new Vector3(x, y, 0);
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

                }

            }
        }
    }


    WallSegment GetNewSegment()
    {

        GameObject go = new GameObject();
        go.transform.position = new Vector3(0, 0, 0);
        go.transform.localScale = new Vector3(1, 1, 1);
        go.transform.parent = transform;
        WallSegment ws = go.AddComponent<WallSegment>();

        ws.UseChildrenAsTiles = true;
        ws.NewParentForTiles = transform;

        return ws;

    }

    bool IsTileWithinMapBounds(int x, int y)
    {
        if (x >= 0 &&
            x < Maps[0].width &&
            y >= 0 &&
            y < Maps[0].height
            )
        {
            return true;
        }
        return false;
    }

}
