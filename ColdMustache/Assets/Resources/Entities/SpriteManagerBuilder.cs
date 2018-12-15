using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManagerBuilder : MonoBehaviour {

    /*
     * INDEXES:
     * 
     * 0 - body
     * 1 - shirt
     * 2 - head (part of body, not headwear)
     * 3 - coat
     * 4 - eyes
     * 5 - face (facial hair)
     * 6 - hair
     * 7 - hat
     * 
     * corresponds to layering - higher index sprite should be over lower index sprite
     * 
     * 
     */


    public Sprite[][] Sprites = new Sprite[8][];
    public Color[] Colors = new Color[8];
    public SpriteRenderer[] SpriteRenderers = new SpriteRenderer[8];


    public Sprite[] Empty;

	// Use this for initialization
	void Start () {
        Empty = Resources.LoadAll<Sprite>("Entities/Human/Parts/EmptyParts");
        
        LoadNakedBody();
        LoadEmptyClothes();

        for(int i = 0; i < 8; i++)
        {
            GameObject go = new GameObject();
            go.transform.parent = transform;
            go.transform.localPosition = new Vector3(0, 0, -(float)i/1000);
            SpriteRenderers[i] = go.AddComponent<SpriteRenderer>();
            
        }

        UpdateEverything(0);
    }
	
	void Update () {

        Vector2 Delta = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;

        //if target is farther "up/down" than "left/right", choose between up/down
        if (Mathf.Abs(Delta.y) > Mathf.Abs(Delta.x))
        {
            //up (back is visible)
            if (Delta.y > 0)
            {
                UpdateEverythingIfRequired(90);
            }
            //down (front is visible)
            else
            {
                UpdateEverythingIfRequired(270);
            }
        }
        //target is on one of your sides - choose between left/right
        else
        {
            //side (toward right = dafault)
            if (Delta.x > 0)
            {
                UpdateEverythingIfRequired(0);
            }
            //other side (toward left - needs mirrorring)
            else
            {
                UpdateEverythingIfRequired(180);
            }
        }

    }

    void LoadNakedBody()
    {
        Sprites[0] = Resources.LoadAll<Sprite>("Entities/Human/Parts/Body02");
        Colors[0] = new Color(1, 0.8588f, 0.6941f, 1);
        Sprites[2] = Resources.LoadAll<Sprite>("Entities/Human/Parts/Head02");
        Colors[2] = new Color(1, 0.8588f, 0.6941f, 1);
        Sprites[4] = Resources.LoadAll<Sprite>("Entities/Human/Parts/Eyes02");
        Colors[4] = new Color(0, 0, 0, 1);
        Sprites[5] = Resources.LoadAll<Sprite>("Entities/Human/Parts/Face02");
        Colors[5] = new Color(0.4867f, 0.3255f, 0.1373f, 1);
        Sprites[6] = Resources.LoadAll<Sprite>("Entities/Human/Parts/Hair02");
        Colors[6] = new Color(0.4867f, 0.3255f, 0.1373f, 1);
    }

    void LoadEmptyClothes()
    {
        Sprites[1] = Empty;
        Sprites[3] = Empty;
        Sprites[7] = Empty;
    }

    public int LastDirection = -1;
    public void UpdateEverythingIfRequired(int Direction)
    {
        if (Direction != LastDirection)
        {
            LastDirection = Direction;
            UpdateEverything(Direction);
        }
    }

    public void UpdateEverything(int Direction = 0)
    {
        switch (Direction)
        {
            case 0:
                for (int i = 0; i < 8; i++)
                {
                    SpriteRenderers[i].sprite = Sprites[i][1];
                    SpriteRenderers[i].color = Colors[i];
                    SpriteRenderers[i].flipX = false;
                }
                return;
            case 90:
                for (int i = 0; i < 8; i++)
                {
                    SpriteRenderers[i].sprite = Sprites[i][2];
                    SpriteRenderers[i].color = Colors[i];
                    SpriteRenderers[i].flipX = false;
                }
                return;
            case 180:
                for (int i = 0; i < 8; i++)
                {
                    SpriteRenderers[i].sprite = Sprites[i][1];
                    SpriteRenderers[i].color = Colors[i];
                    SpriteRenderers[i].flipX = true;
                }
                return;
            case 270:
                for (int i = 0; i < 8; i++)
                {
                    SpriteRenderers[i].sprite = Sprites[i][0];
                    SpriteRenderers[i].color = Colors[i];
                    SpriteRenderers[i].flipX = false;
                }
                return;


        }
    }

    public void UpdateSpecific(int Direction, int Id)
    {
        switch (Direction)
        {
            case 0:
                SpriteRenderers[Id].sprite = Sprites[Id][1];
                SpriteRenderers[Id].color = Colors[Id];
                SpriteRenderers[Id].flipX = false;
                return;
            case 90:
                SpriteRenderers[Id].sprite = Sprites[Id][2];
                SpriteRenderers[Id].color = Colors[Id];
                SpriteRenderers[Id].flipX = false;
                return;
            case 180:
                SpriteRenderers[Id].sprite = Sprites[Id][1];
                SpriteRenderers[Id].color = Colors[Id];
                SpriteRenderers[Id].flipX = true;
                return;
            case 270:
                SpriteRenderers[Id].sprite = Sprites[Id][0];
                SpriteRenderers[Id].color = Colors[Id];
                SpriteRenderers[Id].flipX = false;
                return;


        }
    }

    public void EquipClothing(InventoryItem.ItemType Type, Sprite[] Sprites, Color Color)
    {
        switch (Type)
        {
            case InventoryItem.ItemType.ClothingJacket:
                this.Sprites[3] = Sprites;
                Colors[3] = Color;
                UpdateSpecific(LastDirection, 3);
                return;
            case InventoryItem.ItemType.ClothingShirt:
                this.Sprites[1] = Sprites;
                Colors[1] = Color;
                UpdateSpecific(LastDirection, 1);
                return;
            case InventoryItem.ItemType.ClothingHead:
                this.Sprites[7] = Sprites;
                Colors[7] = Color;
                UpdateSpecific(LastDirection, 7);
                return;
        }
    }

    public void UnequipClothing(InventoryItem.ItemType Type)
    {
        switch (Type)
        {
            case InventoryItem.ItemType.ClothingJacket:
                this.Sprites[3] = Empty;
                UpdateSpecific(LastDirection, 3);
                return;
            case InventoryItem.ItemType.ClothingShirt:
                this.Sprites[1] = Empty;
                UpdateSpecific(LastDirection, 1);
                return;
            case InventoryItem.ItemType.ClothingHead:
                this.Sprites[7] = Empty;
                UpdateSpecific(LastDirection, 7);
                return;
        }
    }
}
