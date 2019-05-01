using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBuilder_MultiModulo : MonoBehaviour
{

    /*
    
    this builder takes several modulo textures, and for each individual pixel on map
    it randomly chooses one of the modulo sheets and picks the corresponding subsprite

    basically it mixes the modulo textures together

    
     */

    public bool ENABLED = true;
    public Texture2D Map;

    public string[] SpriteSheetPaths;
    //weights - how likely is each sprite to be chosen (higher number = higher chance)
    public int[] Weights;

    public Vector2Int ScaleRelativeTo32x32;
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

        for (int y = 0; y < Map.height; y++)
        {
            for (int x = 0; x < Map.width; x++)
            {
                if (all[y * Map.width + x].a != 0)
                {


                    int SpriteSheetIndex = WeightedIndex(Weights);



                    #region ModuloTexture

                    Sprite[] sprites = Resources.LoadAll<Sprite>(SpriteSheetPaths[SpriteSheetIndex]);

                    int TileX = (Mathf.RoundToInt(x) / ScaleRelativeTo32x32.x) % TilesInSheet.x;
                    int TileY = TilesInSheet.y - 1 - (Mathf.RoundToInt(y) / ScaleRelativeTo32x32.y) % TilesInSheet.y;

                    Sprite ChosenSprite = sprites[TilesInSheet.y * TileY + TileX];


                    #endregion;





                    GameObject grass = new GameObject();
                    grass.AddComponent<SpriteRenderer>().color = all[y * Map.width + x];
                    grass.transform.position = new Vector3(x, y, Zposition);

                    grass.GetComponent<SpriteRenderer>().sprite = ChosenSprite;

                    if (RandomXFlip) grass.GetComponent<SpriteRenderer>().flipX = Util.Coinflip();
                    if (RandomYFlip) grass.GetComponent<SpriteRenderer>().flipY = Util.Coinflip();

                    grass.transform.parent = Parent;


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
