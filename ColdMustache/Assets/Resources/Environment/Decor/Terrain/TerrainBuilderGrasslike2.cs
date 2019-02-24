using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBuilderGrasslike2 : MonoBehaviour
{
    public bool ENABLED = true;
    public Texture2D Map;

    //ordered from lowest density to highest density
    //lowest density will be used around edges, highest density more in center
    public string[] SpriteSheetPaths;

    public float Zposition = 230;

    // Start is called before the first frame update
    void Start()
    {
        if (!ENABLED) return;

        GenerateGrassLayer();

        ENABLED = false;
    }

    void GenerateGrassLayer()
    {
        UnityEngine.Color[] all = Map.GetPixels();

        Sprite[][] SpriteSheets = new Sprite[SpriteSheetPaths.Length][];
        for(int i = 0; i < SpriteSheets.Length; i++){
            SpriteSheets[i] = Resources.LoadAll<Sprite>(SpriteSheetPaths[i]);
        }

        for (int y = 0; y < Map.height; y++)
        {
            for (int x = 0; x < Map.width; x++)
            {
                if (all[y * Map.width + x].a != 0)
                {

                    #region FindingDistanceFromEdge


                    int MaxDistanceToCheck = 2*SpriteSheets.Length;
                    int MyDistanceFromEdge = -1;

                    Vector2Int Curr = new Vector2Int(x, y);

                    int CurrLength = 1;
                    int DirectionPointer = 0;
                    Vector2Int[] directions = new Vector2Int[] {
                        new Vector2Int(1,0),
                        new Vector2Int(0,1),
                        new Vector2Int(-1,0),
                        new Vector2Int(0,-1)
                    };

                    //output.SetPixel(Curr.x, Curr.y, palette[0]);

                    for (int i = 0; i < MaxDistanceToCheck; i++)
                    {
                        for (int Doubler = 0; Doubler < 2; Doubler++)
                        {

                            for (int length = 0; length < CurrLength; length++)
                            {
                                Curr = Curr + directions[DirectionPointer];
                                
                                if(IsTileWithinMapBounds(Curr.x,Curr.y)){
                                    if(all[Curr.y * Map.width + Curr.x].a == 0){
                                        //edge was found
                                        MyDistanceFromEdge = Mathf.Abs(Curr.x-x) + Mathf.Abs(Curr.y-y);
                                        goto End;
                                    }
                                }
                            }

                            DirectionPointer++;
                            DirectionPointer = DirectionPointer % directions.Length;

                        }

                        CurrLength++;
                    }
                    //if no edge was found, go for maxvalue (keep in mind theres -- later)
                    MyDistanceFromEdge = SpriteSheets.Length;
                    
                    End:;


                    MyDistanceFromEdge--;
                    if(MyDistanceFromEdge < 0) MyDistanceFromEdge = 0;
                    if(MyDistanceFromEdge >= SpriteSheets.Length) MyDistanceFromEdge = SpriteSheets.Length-1;


                    #endregion;



                    GameObject grass = new GameObject();
                    grass.AddComponent<SpriteRenderer>().color = all[y * Map.width + x];
                    grass.transform.position = new Vector3(x, y, Zposition);
                    grass.name = MyDistanceFromEdge.ToString();

                    grass.GetComponent<SpriteRenderer>().sprite = SpriteSheets[MyDistanceFromEdge][Random.Range(0, SpriteSheets[MyDistanceFromEdge].Length)];

                    grass.transform.parent = transform;
                    

                }
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
