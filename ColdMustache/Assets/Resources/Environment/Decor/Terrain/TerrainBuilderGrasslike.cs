using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBuilderGrasslike : MonoBehaviour
{
    public bool ENABLED = true;
    public Texture2D Map;

    public string LowDensitySheetPath;
    public string MediumDensitySheetPath;
    public string HighDensitySheetPath;
    public string VEryHighDensitySheetPath;

    public float Zposition = 230;

    // Start is called before the first frame update
    void Start()
    {
        if(!ENABLED) return;

        GenerateGrassLayer();
        
        ENABLED = false;
    }

    void GenerateGrassLayer()
    {
        UnityEngine.Color[] all = Map.GetPixels();

        Sprite[] GrassSpritesLow = Resources.LoadAll<Sprite>(LowDensitySheetPath);
        Sprite[] GrassSpritesMedium = Resources.LoadAll<Sprite>(MediumDensitySheetPath);
        Sprite[] GrassSpritesHigh = Resources.LoadAll<Sprite>(HighDensitySheetPath);
        Sprite[] GrassSpritesVeryHigh = Resources.LoadAll<Sprite>(VEryHighDensitySheetPath);

        for (int y = 0; y < Map.height; y++)
        {
            for (int x = 0; x < Map.width; x++)
            {
                if (all[y * Map.width + x].a != 0)
                {

                    int Connections = 0;
                    //right
                    if (IsTileWithinMapBounds(x + 1, y)) { if (all[y * Map.width + x + 1].a != 0) { Connections++; } }
                    //right up
                    if (IsTileWithinMapBounds(x + 1, y + 1)) { if (all[(y + 1) * Map.width + x + 1].a != 0) { Connections++; } }
                    //up
                    if (IsTileWithinMapBounds(x, y + 1)) { if (all[(y + 1) * Map.width + x].a != 0) { Connections++; } }
                    //left up
                    if (IsTileWithinMapBounds(x - 1, y + 1)) { if (all[(y + 1) * Map.width + x - 1].a != 0) { Connections++; } }
                    //left
                    if (IsTileWithinMapBounds(x - 1, y)) { if (all[y * Map.width + x - 1].a != 0) { Connections++; } }
                    //left down
                    if (IsTileWithinMapBounds(x - 1, y - 1)) { if (all[(y - 1) * Map.width + x - 1].a != 0) { Connections++; } }
                    //down
                    if (IsTileWithinMapBounds(x, y - 1)) { if (all[(y - 1) * Map.width + x].a != 0) { Connections++; } }
                    //right down
                    if (IsTileWithinMapBounds(x + 1, y - 1)) { if (all[(y - 1) * Map.width + x + 1].a != 0) { Connections++; } }

                    Connections += Random.Range(-1, 2);



                    GameObject grass = new GameObject();
                    grass.AddComponent<SpriteRenderer>().color = all[y * Map.width + x];
                    grass.transform.position = new Vector3(x, y, Zposition);

                    if (Connections > 8)
                    {
                        grass.GetComponent<SpriteRenderer>().sprite = GrassSpritesVeryHigh[Random.Range(0, GrassSpritesVeryHigh.Length)];
                    }
                    else if (Connections > 4)
                    {
                        grass.GetComponent<SpriteRenderer>().sprite = GrassSpritesHigh[Random.Range(0, GrassSpritesHigh.Length)];
                    }
                    else if (Connections > 2)
                    {
                        grass.GetComponent<SpriteRenderer>().sprite = GrassSpritesMedium[Random.Range(0, GrassSpritesMedium.Length)];
                    }
                    else
                    {
                        grass.GetComponent<SpriteRenderer>().sprite = GrassSpritesLow[Random.Range(0, GrassSpritesLow.Length)];
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

}
