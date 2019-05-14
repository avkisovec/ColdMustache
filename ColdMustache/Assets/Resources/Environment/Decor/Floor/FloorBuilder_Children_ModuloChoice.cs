using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBuilder_Children_ModuloChoice : MonoBehaviour
{


    /*

        based on:
        FloorBuilder_ModuloChoice

        but instead of an image map it works with its children
    
        
     */


    public bool ENABLED = true;

    public string[] SpriteSheetPaths;
    //weights - how likely is each sprite to be chosen (higher number = higher chance)
    public int[] Weights;

    //not using relative scale, no need for it
    //public Vector2Int ScaleRelativeTo32x32;
    public Vector2Int TilesInSheet;

    public bool RandomXFlip = false;
    public bool RandomYFlip = false;


    int HighestX = -999;
    int HighestY = -999;
    int LowestX = 9999;
    int LowestY = 9999;

    // Start is called before the first frame update
    void Start()
    {
        if (!ENABLED) return;

        Generate();

        ENABLED = false;
    }

    void Generate()
    {

        Sprite[][] SpriteSheets = new Sprite[SpriteSheetPaths.Length][];
        for (int i = 0; i < SpriteSheets.Length; i++)
        {
            SpriteSheets[i] = Resources.LoadAll<Sprite>(SpriteSheetPaths[i]);
        }



        SpriteRenderer[,] Srs = new SpriteRenderer[NavTestStatic.MapHeight, NavTestStatic.MapWidth];


        foreach (SpriteRenderer sr in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Vector2Int v = Util.Vector3To2Int(sr.transform.position);
            Srs[v.x, v.y] = sr;

            if (v.x > HighestX) HighestX = v.x;
            if (v.x < LowestX) LowestX = v.x;
            if (v.y > HighestY) HighestY = v.y;
            if (v.y < LowestY) LowestY = v.y;
        }



        //splitting the texture into areas
        for (int AreaStartY = LowestY; AreaStartY <= HighestY; AreaStartY += TilesInSheet.y)
        {
            for (int AreaStartX = LowestX; AreaStartX <= HighestX; AreaStartX += TilesInSheet.x)
            {

                //the start of a new area

                //choose spritesheet for this area
                int SpriteSheetIndex = WeightedIndex(Weights);


                for (int y = 0; y < TilesInSheet.y; y++)
                {
                    for (int x = 0; x < TilesInSheet.x; x++)
                    {

                        //each relative tile in this area

                        //bounds check, necessary as the area can partially be outside bounds
                        if (IsTileWithinMapBounds(AreaStartX + x, AreaStartY + y))
                        {


                            if (Srs[AreaStartX + x, AreaStartY + y] != null)
                            {


                                #region ModuloTexture

                                Sprite[] sprites = Resources.LoadAll<Sprite>(SpriteSheetPaths[SpriteSheetIndex]);

                                Sprite ChosenSprite = sprites[(TilesInSheet.y - 1 - y) * TilesInSheet.y + x];


                                #endregion;


                                Srs[AreaStartX + x, AreaStartY + y].sprite = ChosenSprite;

                                if (RandomXFlip) Srs[AreaStartX + x, AreaStartY + y].flipX = Util.Coinflip();
                                if (RandomYFlip) Srs[AreaStartX + x, AreaStartY + y].flipY = Util.Coinflip();


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
            x < NavTestStatic.MapWidth &&
            y >= 0 &&
            y < NavTestStatic.MapHeight
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

        int ChosenNumber = Random.Range(0, CombinedWeight + 1);

        for (int i = 0; i < Weights.Length; i++)
        {
            ChosenNumber -= Weights[i];
            if (ChosenNumber <= 0) return i;
        }


        return 0;


    }
}