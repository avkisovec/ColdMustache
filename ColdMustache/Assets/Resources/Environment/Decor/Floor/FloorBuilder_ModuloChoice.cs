using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBuilder_ModuloChoice : MonoBehaviour
{


    /*
    
    similar to FloorBuilder_MultiModulo

    the difference is this one doesnt mix subsprites from several modulo sheets,
    instead this "splits" the map into areas of size N1 by N2 (N being the dimensions of modulo sheets, measured in subsprites)
    and the chosen sheet stays same throughout the area

    basically prevents the mixing of different of different sheets, new one is chosen only after one ends

    
     */


    public bool ENABLED = true;
    public Texture2D Map;

    public string[] SpriteSheetPaths;
    //weights - how likely is each sprite to be chosen (higher number = higher chance)
    public int[] Weights;

    //not using relative scale, no need for it
    //public Vector2Int ScaleRelativeTo32x32;
    public Vector2Int TilesInSheet;

    public float Zposition = 230;

    public bool RandomXFlip = false;
    public bool RandomYFlip = false;

    public Transform Parent;

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

        Sprite[][] SpriteSheets = new Sprite[SpriteSheetPaths.Length][];
        for (int i = 0; i < SpriteSheets.Length; i++)
        {
            SpriteSheets[i] = Resources.LoadAll<Sprite>(SpriteSheetPaths[i]);
        }

        //splitting the texture into areas
        for(int AreaStartY = 0; AreaStartY < Map.height; AreaStartY+=TilesInSheet.y){
            for(int AreaStartX = 0; AreaStartX < Map.width; AreaStartX+=TilesInSheet.x){

                //the start of a new area


                //choose spritesheet for this area
                int SpriteSheetIndex = WeightedIndex(Weights);


                for(int y = 0; y < TilesInSheet.y; y++){
                    for(int x = 0; x < TilesInSheet.x; x++){

                        //each relative tile in this area

                        //bounds check, necessary as the area can partially be outside bounds
                        if(IsTileWithinMapBounds(AreaStartX+ x,AreaStartY +y)){


                            if (all[(AreaStartY+y) * Map.width + (AreaStartX+x)].a != 0)
                            {
                                


                                #region ModuloTexture

                                Sprite[] sprites = Resources.LoadAll<Sprite>(SpriteSheetPaths[SpriteSheetIndex]);

                                Sprite ChosenSprite = sprites[(TilesInSheet.y - 1 - y) * TilesInSheet.y + x];


                                #endregion;

                                GameObject go = new GameObject();
                                go.AddComponent<SpriteRenderer>().color = all[(AreaStartY+ y) * Map.width + (AreaStartX+ x)];
                                go.transform.position = new Vector3(AreaStartX+ x,AreaStartY+ y, Zposition);

                                go.GetComponent<SpriteRenderer>().sprite = ChosenSprite;

                                if (RandomXFlip) go.GetComponent<SpriteRenderer>().flipX = Util.Coinflip();
                                if (RandomYFlip) go.GetComponent<SpriteRenderer>().flipY = Util.Coinflip();

                                go.transform.parent = Parent;


                            }


                        }
                    }
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



    //used for random selection from array of multiple options,
    //where each option has different weight (=likeliness to be chosen)
    //input: array of integers, representing weight of each choice
    //output: index of thing that was chosen
    public static int WeightedIndex(int[] Weights)
    {
        int CombinedWeight = 0;
        for (int i = 0; i < Weights.Length; i++)
        {
            CombinedWeight += Weights[i];
        }

        int ChosenNumber = Random.Range(0, CombinedWeight+1);

        for (int i = 0; i < Weights.Length; i++)
        {
            ChosenNumber -= Weights[i];
            if (ChosenNumber <= 0) return i;
        }


        return 0;


    }
}