using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBuilderDirtlike : MonoBehaviour
{
    public bool ENABLED = true;
    public string CenterSheetPath;
    public Sprite Edge;
    public Sprite Corner;
    public Texture2D Map;

    public Vector2Int Modulo_ScaleRelativeTo32x = new Vector2Int(1,1);
    public Vector2Int Modulo_TilesInSheet = new Vector2Int(8, 8);

    public float Zposition = 230;

    // Start is called before the first frame update
    void Start()
    {
        if(!ENABLED) return;

        GenerateDirtLayer();
        
        ENABLED = false;
    }

    void GenerateDirtLayer()
    {
        UnityEngine.Color[] all = Map.GetPixels();

        for (int y = 0; y < Map.height; y++)
        {
            for (int x = 0; x < Map.width; x++)
            {
                if (all[y * Map.width + x].a != 0)
                {
                    GameObject center = new GameObject();
                    center.AddComponent<SpriteRenderer>().color = all[y * Map.width + x];
                    center.AddComponent<ModuloTexture>().SpriteSheetPath = CenterSheetPath;
                    center.GetComponent<ModuloTexture>().ScaleRelativeTo32x32 = Modulo_ScaleRelativeTo32x;
                    center.GetComponent<ModuloTexture>().TilesInSheet = Modulo_TilesInSheet;
                    center.transform.position = new Vector3(x, y, Zposition - 0.01f);
                    center.transform.parent = transform;
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

                    #region Edges

                    //bottom edges
                    if (ConnectedUp && ConnectedRightUp && !ConnectedRight)
                    {

                        GameObject center = new GameObject();
                        center.AddComponent<SpriteRenderer>().color = clr;
                        center.GetComponent<SpriteRenderer>().sprite = Edge;
                        center.transform.position = new Vector3(x + 0.5f, y + 0.5f, Zposition);
                        center.transform.parent = transform;
                    }

                    //top edges
                    if (ConnectedDown && ConnectedRightDown && !ConnectedRight)
                    {

                        GameObject center = new GameObject();
                        center.AddComponent<SpriteRenderer>().color = clr;
                        center.GetComponent<SpriteRenderer>().sprite = Edge;
                        center.transform.position = new Vector3(x + 0.5f, y - 0.5f, Zposition);
                        center.transform.parent = transform;
                    }

                    //left edges
                    if (ConnectedRight && ConnectedRightUp && !ConnectedUp)
                    {
                        GameObject center = new GameObject();
                        center.AddComponent<SpriteRenderer>().color = clr;
                        center.GetComponent<SpriteRenderer>().sprite = Edge;
                        center.transform.position = new Vector3(x + 0.5f, y + 0.5f, Zposition);
                        center.transform.rotation = Quaternion.Euler(0, 0, 90);
                        center.transform.parent = transform;
                    }

                    //right edges
                    if (ConnectedLeft && ConnectedLeftUp && !ConnectedUp)
                    {
                        GameObject center = new GameObject();
                        center.AddComponent<SpriteRenderer>().color = clr;
                        center.GetComponent<SpriteRenderer>().sprite = Edge;
                        center.transform.position = new Vector3(x - 0.5f, y + 0.5f, Zposition);
                        center.transform.rotation = Quaternion.Euler(0, 0, 90);
                        center.transform.parent = transform;
                    }

                    #endregion

                    #region OuterCorners

                    //outer corners
                    if (Connections == 1)
                    {
                        if (ConnectedRightUp)
                        {
                            GameObject center = new GameObject();
                            center.AddComponent<SpriteRenderer>().color = clr;
                            center.GetComponent<SpriteRenderer>().sprite = Corner;
                            center.transform.position = new Vector3(x + 0.5f, y + 0.5f, Zposition);
                            center.transform.parent = transform;
                        }
                        else if (ConnectedLeftUp)
                        {
                            GameObject center = new GameObject();
                            center.AddComponent<SpriteRenderer>().color = clr;
                            center.GetComponent<SpriteRenderer>().sprite = Corner;
                            center.transform.position = new Vector3(x - 0.5f, y + 0.5f, Zposition);
                            center.transform.rotation = Quaternion.Euler(0, 0, 90);
                            center.transform.parent = transform;
                        }
                        else if (ConnectedLeftDown)
                        {
                            GameObject center = new GameObject();
                            center.AddComponent<SpriteRenderer>().color = clr;
                            center.GetComponent<SpriteRenderer>().sprite = Corner;
                            center.transform.position = new Vector3(x - 0.5f, y - 0.5f, Zposition);
                            center.transform.rotation = Quaternion.Euler(0, 0, 180);
                            center.transform.parent = transform;
                        }
                        else if (ConnectedRightDown)
                        {
                            GameObject center = new GameObject();
                            center.AddComponent<SpriteRenderer>().color = clr;
                            center.GetComponent<SpriteRenderer>().sprite = Corner;
                            center.transform.position = new Vector3(x + 0.5f, y - 0.5f, Zposition);
                            center.transform.rotation = Quaternion.Euler(0, 0, 270);
                            center.transform.parent = transform;
                        }
                    }

                    #endregion

                    #region InnerCorners

                    //inner corners
                    if (Connections >= 3)
                    {
                        if (ConnectedRight && ConnectedUp)
                        {
                            GameObject center = new GameObject();
                            center.AddComponent<SpriteRenderer>().color = clr;
                            center.GetComponent<SpriteRenderer>().sprite = Corner;
                            center.transform.position = new Vector3(x + 0.5f, y + 0.5f, Zposition);
                            center.transform.rotation = Quaternion.Euler(0, 0, 180);
                            center.transform.parent = transform;
                        }
                        else if (ConnectedUp && ConnectedLeft)
                        {
                            GameObject center = new GameObject();
                            center.AddComponent<SpriteRenderer>().color = clr;
                            center.GetComponent<SpriteRenderer>().sprite = Corner;
                            center.transform.position = new Vector3(x - 0.5f, y + 0.5f, Zposition);
                            center.transform.rotation = Quaternion.Euler(0, 0, 270);
                            center.transform.parent = transform;
                        }
                        else if (ConnectedLeft && ConnectedDown)
                        {
                            GameObject center = new GameObject();
                            center.AddComponent<SpriteRenderer>().color = clr;
                            center.GetComponent<SpriteRenderer>().sprite = Corner;
                            center.transform.position = new Vector3(x - 0.5f, y - 0.5f, Zposition);
                            center.transform.parent = transform;
                        }
                        else if (ConnectedDown && ConnectedRight)
                        {
                            GameObject center = new GameObject();
                            center.AddComponent<SpriteRenderer>().color = clr;
                            center.GetComponent<SpriteRenderer>().sprite = Corner;
                            center.transform.position = new Vector3(x + 0.5f, y - 0.5f, Zposition);
                            center.transform.rotation = Quaternion.Euler(0, 0, 90);
                            center.transform.parent = transform;
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
