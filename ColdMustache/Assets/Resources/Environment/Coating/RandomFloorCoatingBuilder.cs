using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFloorCoatingBuilder : MonoBehaviour
{
    public bool ENABLED = true;

    public bool DestroyChildren = false;

    public Vector2Int Start_LowerLeft;

    public Vector2Int End_UpperRight_Inclusive;



    public bool SpawnUnderWallsAswell = false;
    public string FloorTexturePath = "Undefined";
    public Vector2Int FloorTilesInSheet;
    public Vector2Int FloorTilesOffset;
    public float Floor_Height = 205;

    public int InverseChanceOfSpawnPerTile = 2;


    void Start()
    {

        if (!ENABLED) return;
        ENABLED = false;

        if (DestroyChildren) Util.DestroyChildren(transform);

        Sprite[] FloorSprites = Resources.LoadAll<Sprite>(FloorTexturePath);


        for (int y = Start_LowerLeft.y; y <= End_UpperRight_Inclusive.y; y++)
        {
            for (int x = Start_LowerLeft.x; x <= End_UpperRight_Inclusive.x; x++)
            {
            
                if (SpawnUnderWallsAswell || NavTestStatic.IsTileFloor(x, y))
                {

                    if(Random.Range(0,InverseChanceOfSpawnPerTile)==0){

                        GameObject go = new GameObject();
                        go.transform.position = new Vector3(x, y, Floor_Height);
                        go.transform.parent = transform;
                        int SpriteY = (y + FloorTilesOffset.y) % FloorTilesInSheet.y;
                        int SpriteX = (x + FloorTilesOffset.x) % FloorTilesInSheet.x;
                        go.AddComponent<SpriteRenderer>().sprite =
                            FloorSprites[SpriteY * FloorTilesInSheet.y + SpriteX];
                        go.GetComponent<SpriteRenderer>().color = new Color(1,1,1,Random.Range(0f,1f));

                    }

                }      


            }
        }

    }

}
