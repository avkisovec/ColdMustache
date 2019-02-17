using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuloTexture : MonoBehaviour
{
    public string SpriteSheetPath;
    public Vector2Int ScaleRelativeTo32x32;
    public Vector2Int TilesInSheet;


    void Start()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(SpriteSheetPath);

        int TileX = (Mathf.RoundToInt(transform.position.x) / ScaleRelativeTo32x32.x) % TilesInSheet.x;
        int TileY = TilesInSheet.y - 1 - (Mathf.RoundToInt(transform.position.y) / ScaleRelativeTo32x32.y) % TilesInSheet.y;
        
        GetComponent<SpriteRenderer>().sprite = sprites[TilesInSheet.y*TileY + TileX];

        Destroy(this);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
