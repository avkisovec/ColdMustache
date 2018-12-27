using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal01Loader : MonoBehaviour {

    string[] Slot0Options = {
        "Entities/Animal/Animal01/body",
        "Entities/Animal/Animal01/body2",
        "Entities/Animal/Animal01/body3"
    };

    string[] Slot1Options = {
        "Entities/Animal/Animal01/spikes",
        "Entities/Animal/Animal01/spikes2",
        "Entities/Animal/Animal01/spikes3"
    };

    string[] Slot2Options = {
        "Entities/Animal/Animal01/eyes",
        "Entities/Animal/Animal01/eyes2",
        "Entities/Animal/Animal01/eyes3"
    };

    public Color[] Slot0Color;
    public Color[] Slot1Color;
    public Color[] Slot2Color;

    // Use this for initialization
    void Start () {

        sprtmng = GetComponent<SpriteManagerGeneric>();
        load();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            load();
        }
    }

    SpriteManagerGeneric sprtmng;

    void load()
    {
        sprtmng.spritePaths = new string[3];
        sprtmng.spritePaths[0] = Slot0Options[Random.Range((int)0, (int)3)];
        sprtmng.spritePaths[1] = Slot1Options[Random.Range((int)0, (int)3)];
        sprtmng.spritePaths[2] = Slot2Options[Random.Range((int)0, (int)3)];

        sprtmng.colors = new Color[3];
        sprtmng.colors[0] = ColorBetween(Slot0Color[0], Slot0Color[1]);
        sprtmng.colors[1] = ColorBetween(Slot1Color[0], Slot1Color[1]);
        sprtmng.colors[2] = ColorBetween(Slot2Color[0], Slot2Color[1]);


        sprtmng.LoadSpritesFromPaths();
        sprtmng.UpdateEverything(sprtmng.LastDirection);
    }

    Color ColorBetween(Color a, Color b)
    {
        return new Color(
            Random.Range(a.r, b.r),
            Random.Range(a.g, b.g),
            Random.Range(a.b, b.b),
            1
            );
    }

}
