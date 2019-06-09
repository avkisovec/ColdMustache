using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoatingBuilder : MonoBehaviour
{
    
    public bool ENABLED = true;

    public bool DestroyChildren = false;

    public Vector2Int Start_LowerLeft;

    public Vector2Int End_UpperRight_Inclusive;



    public bool SpawnOnFloors = false;
    public bool SpawnUnderWallsAswell = false;
    public string FloorTexturePath = "Undefined";
    public Vector2Int FloorTilesInSheet;
    public Vector2Int FloorTilesOffset;
    public float Floor_Height = 205;

    public bool SpawnOnWallFronts = false;
    public string WallFrontTexturePath = "Undefined";
    public Vector2Int WallFrontTilesInSheet;
    public Vector2Int WallFrontTilesOffset;
    public float WallFront_RelativeHeight = -0.1f;

    public bool SpawnOnWallTops = false;
    public string WallTopsTexturePath = "Undefined";
    public Vector2Int WallTopsTilesInSheet;
    public Vector2Int WallTopsTilesOffset;
    public float WallTop_RelativeHeight = -0.1f;





    void Start()
    {

        if(!ENABLED) return;
        ENABLED = false;

        if(DestroyChildren) Util.DestroyChildren(transform);

        Sprite[] FloorSprites = Resources.LoadAll<Sprite>(FloorTexturePath);
        Sprite[] WallFrontSprites = Resources.LoadAll<Sprite>(WallFrontTexturePath);
        Sprite[] WallTopsSPrites = Resources.LoadAll<Sprite>(WallTopsTexturePath);


        for(int y = Start_LowerLeft.y; y <= End_UpperRight_Inclusive.y; y++){
            for(int x = Start_LowerLeft.x; x <= End_UpperRight_Inclusive.x; x++){

                if(SpawnOnFloors){
                    if(SpawnUnderWallsAswell || NavTestStatic.IsTileFloor(x,y)){

                        GameObject go = new GameObject();
                        go.transform.position = new Vector3(x,y,Floor_Height);
                        go.transform.parent = transform;
                        int SpriteY = (y + FloorTilesOffset.y) % FloorTilesInSheet.y;
                        int SpriteX = (x + FloorTilesOffset.x) % FloorTilesInSheet.x;
                        go.AddComponent<SpriteRenderer>().sprite = 
                            FloorSprites[SpriteY * FloorTilesInSheet.y + SpriteX];

                    }
                }

                if (SpawnOnWallFronts)
                {
                    if (NavTestStatic.IsTileWallFront(x, y))
                    {

                        GameObject go = new GameObject();
                        go.transform.position = new Vector3(x, y, 0);
                        go.AddComponent<ZIndexManager>().RelativeValue = WallFront_RelativeHeight;
                        go.GetComponent<ZIndexManager>().SingleUse = true;
                        go.AddComponent<Decorator>();
                        go.transform.parent = transform;
                        int SpriteY = (y + WallFrontTilesOffset.y) % WallFrontTilesInSheet.y;
                        int SpriteX = (x + WallFrontTilesOffset.x) % WallFrontTilesInSheet.x;
                        go.AddComponent<SpriteRenderer>().sprite =
                            WallFrontSprites[SpriteY * FloorTilesInSheet.y + SpriteX];

                    }
                }

                if (SpawnOnWallTops)
                {
                    if (NavTestStatic.IsTileWallTop(x, y))
                    {

                        GameObject go = new GameObject();
                        go.transform.position = new Vector3(x, y, 0);
                        go.AddComponent<ZIndexManager>().RelativeValue = WallFront_RelativeHeight;
                        go.GetComponent<ZIndexManager>().SingleUse = true;
                        go.AddComponent<Decorator>();
                        go.transform.parent = transform;
                        int SpriteY = (y + WallTopsTilesOffset.y) % WallTopsTilesInSheet.y;
                        int SpriteX = (x + WallTopsTilesOffset.x) % WallTopsTilesInSheet.x;
                        go.AddComponent<SpriteRenderer>().sprite =
                            WallTopsSPrites[SpriteY * FloorTilesInSheet.y + SpriteX];

                    }
                }

            }
        }
        
    }

}
