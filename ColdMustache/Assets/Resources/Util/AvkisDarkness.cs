using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvkisDarkness : MonoBehaviour {

    /*
     * this script spawns black squares all over the map (and keeps reference to all spriteRenderers)
     * 
     * then, when player moves to a new tile it updates
     * 
     * uses avkis's light to check what tiles player has vision of
     * 
     * those black squares will then turn transparent
     * 
     * 
     * 
     */

    static SpriteRenderer[,] Darkness = new SpriteRenderer[NavTestStatic.MapWidth, NavTestStatic.MapHeight];

    static List<Vector2Int> LastVisible = new List<Vector2Int>();

    private void Awake()
    {
        Darkness = new SpriteRenderer[NavTestStatic.MapWidth, NavTestStatic.MapHeight];
        LastVisible = new List<Vector2Int>();
    }
    
    // Use this for initialization
    void Start () {
		for(int x = 0; x < NavTestStatic.MapWidth; x++)
        {
            for(int y = 0; y < NavTestStatic.MapHeight; y++)
            {
                GameObject go = new GameObject();
                go.transform.position = new Vector3(x, y, -25);
                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = UniversalReference.Pixel;
                sr.color = new Color(0,0,0,1);
                Darkness[x, y] = sr;
            }
        }
	}


    Vector2Int PlayerCurrentTile = new Vector2Int(0, 0);
    Vector2Int PlayerLastTile = new Vector2Int(-1, -1);
    void Update()
    {
        PlayerLastTile = PlayerCurrentTile;
        PlayerCurrentTile = Util.Vector3To2Int(UniversalReference.PlayerObject.transform.position);

        if (PlayerCurrentTile != PlayerLastTile)
        {
            AvkisDarkness.UpdateLight(PlayerCurrentTile);
        }
    }

    public static void UpdateLight(Vector2Int Source)
    {
        
        for(int i = 0; i < LastVisible.Count; i++)
        {
            Darkness[LastVisible[i].x, LastVisible[i].y].color = new Color(0, 0, 0, 1);
        }
        LastVisible.Clear();
        Vector2Int[] Current = NavTestStatic.AvkisLight_cast(Source).ToArray();
        for(int i = 0; i < Current.Length; i++)
        {
            Darkness[Current[i].x, Current[i].y].color = new Color(0, 0, 0, 0);
            LastVisible.Add(Current[i]);
        }

        //softer darkness around the edges
        for (int i = 0; i < Current.Length; i++)
        {
            for (int relX = -1; relX <= 1; relX += 2)
            {
                for (int relY = -1; relY <= 1; relY += 2)
                {
                    if (Darkness[Current[i].x + relX, Current[i].y + relY].color.a != 0)
                    {
                        Darkness[Current[i].x + relX, Current[i].y + relY].color = new Color(0, 0, 0, 0.5f);
                        LastVisible.Add(new Vector2Int(Current[i].x+relX, Current[i].y+relY));
                    }
                }
            }
        }
    }
    
	
}
