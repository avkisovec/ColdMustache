using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterBoundsFromMapScript : MonoBehaviour
{
    public Texture2D Map;
    public float Zposition = 230;

    // Start is called before the first frame update
    void Start()
    {

        GenerateWhitePixelBlockingMoveObjectThatStopsYouFromMoving();

    }

    void GenerateWhitePixelBlockingMoveObjectThatStopsYouFromMoving()
    {
        UnityEngine.Color[] all = Map.GetPixels();

        for (int y = 0; y < Map.height; y++)
        {
            for (int x = 0; x < Map.width; x++)
            {
                Color pixel = all[y * Map.width + x];

                if (all[y * Map.width + x].a != 0)
                {
                    
                }
                else
                {
                    int Connections = 0;

                    Color clr = new Color(1, 1, 1, 1);

                    //right
                    if (IsTileWithinMapBounds(x + 1, y)) { if (all[y * Map.width + x + 1].a != 0) {Connections++; clr = all[y * Map.width + x + 1]; } }
                    //right up
                    if (IsTileWithinMapBounds(x + 1, y + 1)) { if (all[(y + 1) * Map.width + x + 1].a != 0) {Connections++; clr = all[(y + 1) * Map.width + x + 1]; } }
                    //up
                    if (IsTileWithinMapBounds(x, y + 1)) { if (all[(y + 1) * Map.width + x].a != 0) {Connections++; clr = all[(y + 1) * Map.width + x]; } }
                    //left up
                    if (IsTileWithinMapBounds(x - 1, y + 1)) { if (all[(y + 1) * Map.width + x - 1].a != 0) {Connections++; clr = all[(y + 1) * Map.width + x - 1]; } }
                    //left
                    if (IsTileWithinMapBounds(x - 1, y)) { if (all[y * Map.width + x - 1].a != 0) {Connections++; clr = all[y * Map.width + x - 1]; } }
                    //left down
                    if (IsTileWithinMapBounds(x - 1, y - 1)) { if (all[(y - 1) * Map.width + x - 1].a != 0) {Connections++; clr = all[(y - 1) * Map.width + x - 1]; } }
                    //down
                    if (IsTileWithinMapBounds(x, y - 1)) { if (all[(y - 1) * Map.width + x].a != 0) {Connections++; clr = all[(y - 1) * Map.width + x]; } }
                    //right down
                    if (IsTileWithinMapBounds(x + 1, y - 1)) { if (all[(y - 1) * Map.width + x + 1].a != 0) {Connections++; clr = all[(y - 1) * Map.width + x + 1]; } }

                    if (Connections >= 1)
                    {
                        GameObject Bound = new GameObject();
                        Bound.transform.position = new Vector3(x, y, Zposition);
                        Bound.AddComponent<BoxCollider2D>();
                        Bound.transform.parent = this.transform;
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
