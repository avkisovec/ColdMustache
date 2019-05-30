using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoader : MonoBehaviour
{
    
    static List<string> LoadedSheetPaths = new List<string>();
    static List<Sprite[]> LoadedSpriteSheets = new List<Sprite[]>();



    public static Sprite[] RequestSheet(string Path){
        
        for(int i = 0; i < LoadedSheetPaths.Count; i++){

            if(Path == LoadedSheetPaths[i]) return LoadedSpriteSheets[i];

        }

        Sprite[] NewSheet = Resources.LoadAll<Sprite>(Path);
        LoadedSheetPaths.Add(Path);
        LoadedSpriteSheets.Add(NewSheet);

        return NewSheet;
    }


}
