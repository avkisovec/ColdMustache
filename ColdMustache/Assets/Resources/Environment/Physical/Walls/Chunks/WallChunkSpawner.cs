using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChunkSpawner : MonoBehaviour
{
    
    static List<string> Paths = new List<string>();
    static List<Sprite[]> SpriteSheets = new List<Sprite[]>();
    
    public static void SpawnChunk(Vector3 Position, string ChunkPath = "Environment/Physical/Walls/Chunks/01", string StainPath = "Environment/Physical/Walls/Panel/01rubble")
    {
        GameObject Chunk = new GameObject();
        Chunk.transform.position = new Vector3(Position.x + Random.Range(-1f,1f), Position.y+Random.Range(-1f,1f), 208+Random.Range(-1f,1f));
       
        /*
        ZIndexManager zim = Chunk.AddComponent<ZIndexManager>();
        zim.Type = ZIndexManager.Types.Objects;
        zim.SingleUse = true;
        zim.RelativeValue = Random.Range(-0.1f, 0.1f);
        */
        
        //Chunk.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        Chunk.AddComponent<SpriteRenderer>().sprite = GetRandomSpriteFromASheet(GetSheetIndex(ChunkPath));
        //Chunk.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);


        GameObject Stain = new GameObject();
        Stain.transform.position = new Vector3(Position.x, Position.y, ZIndexManager.Const_Floors - 1 + Random.Range(-1f, 0f));
        Stain.AddComponent<SpriteRenderer>().sprite = SpriteSheets[GetSheetIndex(StainPath)][0];
        Stain.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        Stain.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));


    }


    //loads and caches sprite sheets
    //you request a path to a spritesheet, it checks if it has that spritesheet cached, and if not loads it
    //returns the spritesheet index either way
    static int GetSheetIndex(string path = "Environment/Physical/Walls/Chunks/01"){

        //check if you already loaded this path
        for(int i = 0; i < Paths.Count; i++){
            if(Paths[i] == path) return i;
        }

        //you did not, so load and cache it
        Paths.Add(path);
        SpriteSheets.Add(Resources.LoadAll<Sprite>(path));

        return SpriteSheets.Count-1;
    }

    static Sprite GetRandomSpriteFromASheet(int SheetIndex){
        return SpriteSheets[SheetIndex][Random.Range(0,SpriteSheets[SheetIndex].Length)];
    }

}
