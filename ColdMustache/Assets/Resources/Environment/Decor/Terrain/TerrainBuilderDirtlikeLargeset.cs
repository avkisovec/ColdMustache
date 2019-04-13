using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBuilderDirtlikeLargeset : MonoBehaviour
{
    public bool ENABLED = true;
    
    public Texture2D Map;

    public bool UseFirstPathAsDirectoryAndAutocompleteSlash0Through12 = false;
    public string[] SpritesheetPaths;

    public Vector2Int Modulo_ScaleRelativeTo32x = new Vector2Int(1, 1);
    public Vector2Int Modulo_TilesInSheet = new Vector2Int(8, 8);

    public float Zposition = 230;

    // Start is called before the first frame update
    void Start()
    {
        if (!ENABLED) return;

        GenerateDirtLayer();

        ENABLED = false;
    }

    void GenerateDirtLayer()
    {
        if(UseFirstPathAsDirectoryAndAutocompleteSlash0Through12){
            string Directory = SpritesheetPaths[0];
            SpritesheetPaths = new string[13];
            for(int i = 0; i <= 12; i++){
                SpritesheetPaths[i] = Directory+"/"+i.ToString();
            }
        }

        Sprite[][] Sprites = new Sprite[SpritesheetPaths.Length][];

        for(int i = 0; i < SpritesheetPaths.Length; i++){
            Sprites[i] = Resources.LoadAll<Sprite>(SpritesheetPaths[i]);
        }


        UnityEngine.Color[] all = Map.GetPixels();

        for (int y = 0; y < Map.height; y++)
        {
            for (int x = 0; x < Map.width; x++)
            {
                if (all[y * Map.width + x].a != 0)
                {
                    GameObject center = new GameObject();
                    center.AddComponent<SpriteRenderer>().color = all[y * Map.width + x];
                    center.AddComponent<ModuloTexture>().SpriteSheetPath = SpritesheetPaths[0];
                    center.GetComponent<ModuloTexture>().ScaleRelativeTo32x32 = Modulo_ScaleRelativeTo32x;
                    center.GetComponent<ModuloTexture>().TilesInSheet = Modulo_TilesInSheet;
                    center.transform.position = new Vector3(x, y, Zposition + 0.01f);
                    center.transform.parent = transform;
                    continue;
                }
                else
                {
                    bool ConnectedRight = false;
                    bool ConnectedRightUp = false;
                    bool ConnectedUp = false;
                    bool ConnectedLeftUp = false;
                    bool ConnectedLeft = false;
                    bool ConnectedLeftDown = false;
                    bool ConnectedDown = false;
                    bool ConnectedRightDown = false;
                    int Connections = 0;

                    Color clr = new Color(1, 1, 1, 1);

                    //right
                    if (IsTileWithinMapBounds(x + 1, y)) { if (all[y * Map.width + x + 1].a != 0) { ConnectedRight = true; Connections++; clr = all[y * Map.width + x + 1]; } }
                    //right up
                    if (IsTileWithinMapBounds(x + 1, y + 1)) { if (all[(y + 1) * Map.width + x + 1].a != 0) { ConnectedRightUp = true; Connections++; clr = all[(y + 1) * Map.width + x + 1]; } }
                    //up
                    if (IsTileWithinMapBounds(x, y + 1)) { if (all[(y + 1) * Map.width + x].a != 0) { ConnectedUp = true; Connections++; clr = all[(y + 1) * Map.width + x]; } }
                    //left up
                    if (IsTileWithinMapBounds(x - 1, y + 1)) { if (all[(y + 1) * Map.width + x - 1].a != 0) { ConnectedLeftUp = true; Connections++; clr = all[(y + 1) * Map.width + x - 1]; } }
                    //left
                    if (IsTileWithinMapBounds(x - 1, y)) { if (all[y * Map.width + x - 1].a != 0) { ConnectedLeft = true; Connections++; clr = all[y * Map.width + x - 1]; } }
                    //left down
                    if (IsTileWithinMapBounds(x - 1, y - 1)) { if (all[(y - 1) * Map.width + x - 1].a != 0) { ConnectedLeftDown = true; Connections++; clr = all[(y - 1) * Map.width + x - 1]; } }
                    //down
                    if (IsTileWithinMapBounds(x, y - 1)) { if (all[(y - 1) * Map.width + x].a != 0) { ConnectedDown = true; Connections++; clr = all[(y - 1) * Map.width + x]; } }
                    //right down
                    if (IsTileWithinMapBounds(x + 1, y - 1)) { if (all[(y - 1) * Map.width + x + 1].a != 0) { ConnectedRightDown = true; Connections++; clr = all[(y - 1) * Map.width + x + 1]; } }


                    #region InnerCorners

                    //inner corners
                    if (Connections >= 3)
                    {
                        if (ConnectedLeft && ConnectedDown)
                        {
                            GameObject center = new GameObject();
                            center.AddComponent<SpriteRenderer>().color = clr;
                            center.AddComponent<ModuloTexture>().SpriteSheetPath = SpritesheetPaths[9];
                            center.GetComponent<ModuloTexture>().ScaleRelativeTo32x32 = Modulo_ScaleRelativeTo32x;
                            center.GetComponent<ModuloTexture>().TilesInSheet = Modulo_TilesInSheet;
                            center.transform.position = new Vector3(x, y, Zposition + 0.01f);
                            center.transform.parent = transform;
                            continue;
                        }
                        else if (ConnectedDown && ConnectedRight)
                        {
                            GameObject center = new GameObject();
                            center.AddComponent<SpriteRenderer>().color = clr;
                            center.AddComponent<ModuloTexture>().SpriteSheetPath = SpritesheetPaths[10];
                            center.GetComponent<ModuloTexture>().ScaleRelativeTo32x32 = Modulo_ScaleRelativeTo32x;
                            center.GetComponent<ModuloTexture>().TilesInSheet = Modulo_TilesInSheet;
                            center.transform.position = new Vector3(x, y, Zposition + 0.01f);
                            center.transform.parent = transform;
                            continue;
                        }
                        else if (ConnectedRight && ConnectedUp)
                        {
                            GameObject center = new GameObject();
                            center.AddComponent<SpriteRenderer>().color = clr;
                            center.AddComponent<ModuloTexture>().SpriteSheetPath = SpritesheetPaths[11];
                            center.GetComponent<ModuloTexture>().ScaleRelativeTo32x32 = Modulo_ScaleRelativeTo32x;
                            center.GetComponent<ModuloTexture>().TilesInSheet = Modulo_TilesInSheet;
                            center.transform.position = new Vector3(x, y, Zposition + 0.01f);
                            center.transform.parent = transform;
                            continue;
                        }
                        else if (ConnectedUp && ConnectedLeft)
                        {
                            GameObject center = new GameObject();
                            center.AddComponent<SpriteRenderer>().color = clr;
                            center.AddComponent<ModuloTexture>().SpriteSheetPath = SpritesheetPaths[12];
                            center.GetComponent<ModuloTexture>().ScaleRelativeTo32x32 = Modulo_ScaleRelativeTo32x;
                            center.GetComponent<ModuloTexture>().TilesInSheet = Modulo_TilesInSheet;
                            center.transform.position = new Vector3(x, y, Zposition + 0.01f);
                            center.transform.parent = transform;
                            continue;
                        }
                    }

                    #endregion


                    #region Edges
                    //right edge
                    if (ConnectedLeft)
                    {
                        GameObject center = new GameObject();
                        center.AddComponent<SpriteRenderer>().color = clr;
                        center.AddComponent<ModuloTexture>().SpriteSheetPath = SpritesheetPaths[1];
                        center.GetComponent<ModuloTexture>().ScaleRelativeTo32x32 = Modulo_ScaleRelativeTo32x;
                        center.GetComponent<ModuloTexture>().TilesInSheet = Modulo_TilesInSheet;
                        center.transform.position = new Vector3(x, y, Zposition + 0.01f);
                        center.transform.parent = transform;
                        center.gameObject.name="edge";
                        continue;
                    }

                    //top edges
                    if (ConnectedDown)
                    {
                        GameObject center = new GameObject();
                        center.AddComponent<SpriteRenderer>().color = clr;
                        center.AddComponent<ModuloTexture>().SpriteSheetPath = SpritesheetPaths[2];
                        center.GetComponent<ModuloTexture>().ScaleRelativeTo32x32 = Modulo_ScaleRelativeTo32x;
                        center.GetComponent<ModuloTexture>().TilesInSheet = Modulo_TilesInSheet;
                        center.transform.position = new Vector3(x, y, Zposition + 0.01f);
                        center.transform.parent = transform;
                        continue;
                    }

                    //left edges
                    if (ConnectedRight)
                    {
                        GameObject center = new GameObject();
                        center.AddComponent<SpriteRenderer>().color = clr;
                        center.AddComponent<ModuloTexture>().SpriteSheetPath = SpritesheetPaths[3];
                        center.GetComponent<ModuloTexture>().ScaleRelativeTo32x32 = Modulo_ScaleRelativeTo32x;
                        center.GetComponent<ModuloTexture>().TilesInSheet = Modulo_TilesInSheet;
                        center.transform.position = new Vector3(x, y, Zposition + 0.01f);
                        center.transform.parent = transform;
                        continue;
                    }

                    //right edges
                    if (ConnectedUp)
                    {
                        GameObject center = new GameObject();
                        center.AddComponent<SpriteRenderer>().color = clr;
                        center.AddComponent<ModuloTexture>().SpriteSheetPath = SpritesheetPaths[4];
                        center.GetComponent<ModuloTexture>().ScaleRelativeTo32x32 = Modulo_ScaleRelativeTo32x;
                        center.GetComponent<ModuloTexture>().TilesInSheet = Modulo_TilesInSheet;
                        center.transform.position = new Vector3(x, y, Zposition + 0.01f);
                        center.transform.parent = transform;
                        continue;
                    }

                    #endregion

                    #region OuterCorners

                    //outer corners
                    if (Connections == 1)
                    {
                        if (ConnectedLeftDown)
                        {
                            GameObject center = new GameObject();
                            center.AddComponent<SpriteRenderer>().color = clr;
                            center.AddComponent<ModuloTexture>().SpriteSheetPath = SpritesheetPaths[5];
                            center.GetComponent<ModuloTexture>().ScaleRelativeTo32x32 = Modulo_ScaleRelativeTo32x;
                            center.GetComponent<ModuloTexture>().TilesInSheet = Modulo_TilesInSheet;
                            center.transform.position = new Vector3(x, y, Zposition + 0.01f);
                            center.transform.parent = transform;
                            continue;
                        }
                        else if (ConnectedRightDown)
                        {
                            GameObject center = new GameObject();
                            center.AddComponent<SpriteRenderer>().color = clr;
                            center.AddComponent<ModuloTexture>().SpriteSheetPath = SpritesheetPaths[6];
                            center.GetComponent<ModuloTexture>().ScaleRelativeTo32x32 = Modulo_ScaleRelativeTo32x;
                            center.GetComponent<ModuloTexture>().TilesInSheet = Modulo_TilesInSheet;
                            center.transform.position = new Vector3(x, y, Zposition + 0.01f);
                            center.transform.parent = transform;
                            continue;
                        }
                        else if (ConnectedRightUp)
                        {
                            GameObject center = new GameObject();
                            center.AddComponent<SpriteRenderer>().color = clr;
                            center.AddComponent<ModuloTexture>().SpriteSheetPath = SpritesheetPaths[7];
                            center.GetComponent<ModuloTexture>().ScaleRelativeTo32x32 = Modulo_ScaleRelativeTo32x;
                            center.GetComponent<ModuloTexture>().TilesInSheet = Modulo_TilesInSheet;
                            center.transform.position = new Vector3(x, y, Zposition + 0.01f);
                            center.transform.parent = transform;
                            continue;
                        }
                        else if (ConnectedLeftUp)
                        {
                            GameObject center = new GameObject();
                            center.AddComponent<SpriteRenderer>().color = clr;
                            center.AddComponent<ModuloTexture>().SpriteSheetPath = SpritesheetPaths[8];
                            center.GetComponent<ModuloTexture>().ScaleRelativeTo32x32 = Modulo_ScaleRelativeTo32x;
                            center.GetComponent<ModuloTexture>().TilesInSheet = Modulo_TilesInSheet;
                            center.transform.position = new Vector3(x, y, Zposition + 0.01f);
                            center.transform.parent = transform;
                            continue;
                        }
                    }

                    #endregion


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
