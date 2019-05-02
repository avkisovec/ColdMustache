using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSegmentManager : MonoBehaviour
{

    public bool ENABLED = true;

    void Start()
    {
        if(!ENABLED) return;

        ENABLED = false;

        //arr to load independent wall segment tiles into (to then form them imto segments)
        WallSegmentTileIndependent[,] Arr = new WallSegmentTileIndependent[NavTestStatic.MapWidth, NavTestStatic.MapHeight];

        //loading all tiles into the arr
        foreach (WallSegmentTileIndependent eo in GameObject.FindObjectsOfType<WallSegmentTileIndependent>())
        {
            Vector2Int IntCoordinates = new Vector2Int(Mathf.RoundToInt(eo.transform.position.x), Mathf.RoundToInt(eo.transform.position.y));

            //load into the arr if theres a space
            if(Arr[IntCoordinates.x, IntCoordinates.y] == null){
                Arr[IntCoordinates.x, IntCoordinates.y] = eo;
            }
            //something already is on these exact coordinates, prolly as a result of a human error during duplication
            //the redundant wall is deleted
            else{
                Destroy(eo);
            }
            
        }


        string DamagedSpritesLastPath = "";
        Sprite[] DamagedSprites = null;


        WallSegment CurrWallSegment = null;
        //false if im currently in a segment, true if im outside of one and need to create one for new walls
        bool DoINeedANewSegment = true;

        for (int x = 0; x < NavTestStatic.MapWidth; x++)
        {
            for (int y = 0; y < NavTestStatic.MapHeight; y++)
            {


                WallSegmentTileIndependent eo = Arr[x,y];

                if (eo!=null)
                {
                    //theres a wall on these coordinates


                    if (DoINeedANewSegment)
                    {
                        //this wall is the beginning of a new segment

                        //initialize the creation of the last segment, unless its for the first time and there isnt any (null check)
                        if(CurrWallSegment!=null) CurrWallSegment.ini(null, true, false, DamagedSprites);
                        
                        //get a new segment
                        CurrWallSegment = GetNewSegment();

                        //load new damaged sprite sheet, unless its the same as the last one
                        if (eo.DamagedSpritesheetPath != DamagedSpritesLastPath) {
                            DamagedSpritesLastPath = eo.DamagedSpritesheetPath;
                            DamagedSprites = Resources.LoadAll<Sprite>(eo.DamagedSpritesheetPath);
                        }

                    }
                    else{

                        //this wall is not the front of a segment

                    }

                    DoINeedANewSegment = false;

                    eo.transform.parent = CurrWallSegment.transform;

                }
                //there is no wall - you're gonna need a new segment when you find a next one
                else{
                    DoINeedANewSegment = true;
                }

                

            }
        }


        //after the loop initialize the last wallsegment that hasnt been yet
        if (CurrWallSegment != null) CurrWallSegment.ini(null, true, false, DamagedSprites);


        
    }

    // Update is called once per frame
    void Update()
    {
        
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

}
