using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralModuloCliff : MonoBehaviour
{

    public string SpritePath;
    public int BaseTilesPerSheet;
    public int ReflectionTilesPerBase;
    Sprite[] FullSpriteSheet;
    int MyModuloChosenSubTileSet = -1;

    int MyFirstReflectionSpriteIndex = -1;
    int MyLastReflectionSpriteIndex = -1;

    public int Duration = 3;

    // Start is called before the first frame update
    void Start()
    {

        List<Sprite>AboveWaterSprites = new List<Sprite>();
        List<List<Sprite>>ReflectionSheets = new List<List<Sprite>>();

        int CurrSheetPointer = -1;

        FullSpriteSheet = Resources.LoadAll<Sprite>(SpritePath);

        for(int i = 0; i < FullSpriteSheet.Length; i++){

            //if you found new above water sprite
            if(i%(ReflectionTilesPerBase+1)==0){
                AboveWaterSprites.Add(FullSpriteSheet[i]);
                ReflectionSheets.Add(new List<Sprite>());
                CurrSheetPointer++;
            }
            else{
                ReflectionSheets[CurrSheetPointer].Add(FullSpriteSheet[i]);
            }
        }

        //for each child (individual cliff section)
        for(int ChildIndex = 0; ChildIndex < transform.childCount; ChildIndex++){
            Transform child = transform.GetChild(ChildIndex);

            //delete all child's children (reflection)
            for(int ChildsChild = 0; ChildsChild < child.childCount; ChildsChild++){
                Destroy(child.GetChild(ChildsChild).gameObject);
            }

            // 3*y to skip some sprites where the edge "breaks" (the following cliff is on different y)
            MyModuloChosenSubTileSet = (Mathf.RoundToInt(child.position.x) + 3 * Mathf.RoundToInt(child.position.y)) % BaseTilesPerSheet;

            child.GetComponent<SpriteRenderer>().sprite = AboveWaterSprites[MyModuloChosenSubTileSet];

            GameObject reflection = new GameObject();
            reflection.transform.parent = child;
            reflection.transform.localPosition = new Vector3(0,-4);

            reflection.AddComponent<SpriteRenderer>().color = new Color(0.7f,0.7f,0.8f);
            SimpleAnimation_codefed sa = reflection.AddComponent<SimpleAnimation_codefed>();
            sa.Duration = Duration;
            sa.sprites = ReflectionSheets[MyModuloChosenSubTileSet].ToArray();
            
        }

        Destroy(this);
    }
}
