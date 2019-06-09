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

    public Vector2Int Offset;

    public bool RandomXFlip = false;
    public bool RandomYFlip = false;

    //false - the lower-right-most subsprite will align with the lower-right-most child
    //true - the lower-right-most subsprite will align with coordinates [0,0],
    //  so multiple these scripts with the same texture will align
    public bool UseCompatibleCoordinates = true;


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

        int TextureStartY = 0;
        int TextureStartX = 0;
        if(UseCompatibleCoordinates){

            //bit of optimisation, move the start of the area so that when you pass through the whole area you skip the empty parts
            while(TextureStartY + TilesInSheet.y < LowestY){
                TextureStartY+=TilesInSheet.y;
            }
            while (TextureStartY + TilesInSheet.y < LowestY)
            {
                TextureStartY += TilesInSheet.y;
            }

        }
        else{
            TextureStartY = LowestY;
            TextureStartX = LowestX;
        }


        //splitting the texture into areas
        for (int AreaStartY = TextureStartY; AreaStartY <= HighestY; AreaStartY += TilesInSheet.y)
        {
            for (int AreaStartX = TextureStartX; AreaStartX <= HighestX; AreaStartX += TilesInSheet.x)
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

                                int OffsetX = x+Offset.x;
                                int OffsetY = y+Offset.y;
                                OffsetX = OffsetX%TilesInSheet.x;
                                OffsetY = OffsetY%TilesInSheet.y;
                                if (OffsetX < 0) OffsetX += TilesInSheet.x;
                                if (OffsetY < 0) OffsetY += TilesInSheet.y;

                                #region ModuloTexture

                                Sprite[] sprites = Resources.LoadAll<Sprite>(SpriteSheetPaths[SpriteSheetIndex]);

                                Sprite ChosenSprite = sprites[(TilesInSheet.y - 1 - OffsetY) * TilesInSheet.y + OffsetX];


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

            //weight of 0 completely breaks it and causes infinite loop, so if there is one return 0
            if(Weights[i] == 0) return 0;
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