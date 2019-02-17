using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuloCliff : MonoBehaviour
{

    public string SpritePath;
    public int BaseTilesPerSheet;
    public int ReflectionTilesPerBase;
    public int TileWidth;

    public SpriteRenderer Base;
    public SpriteRenderer Reflection;

    Sprite[] MySpritesheet;

    int MyModuloChosenSubTileSet = -1;

    int MyFirstReflectionSpriteIndex = -1;
    int MyLastReflectionSpriteIndex = -1;


    // Start is called before the first frame update
    void Start()
    {
        MyModuloChosenSubTileSet = Mathf.RoundToInt(transform.position.x) / TileWidth % BaseTilesPerSheet;

        MySpritesheet = Resources.LoadAll<Sprite>(SpritePath);

        Base.sprite = MySpritesheet[MyModuloChosenSubTileSet * (1 + ReflectionTilesPerBase)];
        MyFirstReflectionSpriteIndex = MyModuloChosenSubTileSet * (1 + ReflectionTilesPerBase) + 1;
        MyLastReflectionSpriteIndex = MyModuloChosenSubTileSet * (1 + ReflectionTilesPerBase) + ReflectionTilesPerBase;

        AnimPhase = MyFirstReflectionSpriteIndex;

    }

    float AnimPhase = -1;
    void Update()
    {
        AnimPhase += Time.deltaTime * 10;
        if(AnimPhase > MyLastReflectionSpriteIndex) AnimPhase = MyFirstReflectionSpriteIndex;

        Reflection.sprite = MySpritesheet[Mathf.RoundToInt(AnimPhase)];

    }
}
