using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManagerBuilder : SpriteManagerBase {

    /*
     * INDEXES: (for human)
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

    public SpriteRenderer Hand_Shirt;
    public SpriteRenderer Hand_Jacket;
    public SpriteRenderer OtherHand_Shirt;
    public SpriteRenderer OtherHand_Jacket;

    public SpriteRenderer Hand;
    public SpriteRenderer OtherHand;

    Color[] colors_original;

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


        colors_original = (Color[])Colors.Clone();

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
                UpdateIfNeeded(90);
            }
            //down (front is visible)
            else
            {
                UpdateIfNeeded(270);
            }
        }
        //target is on one of your sides - choose between left/right
        else
        {
            //side (toward right = dafault)
            if (Delta.x > 0)
            {
                UpdateIfNeeded(0);
            }
            //other side (toward left - needs mirrorring)
            else
            {
                UpdateIfNeeded(180);
            }
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            RandomColorsHumanIsh();
            UpdateEverything(LastDirection);
        }

        UpdateHands();


        if (TemporaryColorTimeRemaining > 0)
        {
            TemporaryColorTimeRemaining -= Time.deltaTime;
        }
        if (TemporaryColorTimeRemaining != -999999 && TemporaryColorTimeRemaining <= 0)
        {
            TemporaryColorTimeRemaining = -999999;

            Colors = (Color[])colors_original.Clone();

            UpdateEverything(LastDirection);
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

    public override void UpdateIfNeeded(int Direction)
    {
        if (Direction != LastDirection)
        {
            LastDirection = Direction;
            UpdateEverything(Direction);
        }
    }

    public void UpdateHands()
    {
        // 1 - shirt
        // 3 - jacket

        //mainhand is subsprite _3, other hand is subsprite _4
        
        Hand_Shirt.sprite = Sprites[1][3];
        Hand_Shirt.color = Colors[1];
        Hand_Shirt.flipY = Hand.flipY;

        Hand_Jacket.sprite = Sprites[3][3];
        Hand_Jacket.color = Colors[3];
        Hand_Jacket.flipY = Hand.flipY;
        
        OtherHand_Shirt.sprite = Sprites[1][4];
        OtherHand_Shirt.color = Colors[1];
        OtherHand_Shirt.flipY = OtherHand.flipY;

        OtherHand_Jacket.sprite = Sprites[3][4];
        OtherHand_Jacket.color = Colors[3];
        OtherHand_Jacket.flipY = OtherHand.flipY;


        //hands get the same color as body
        Hand.color = Colors[0];
        OtherHand.color = Colors[0];

    }

    public override void UpdateEverything(int Direction = 0)
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

    public void RandomColors()
    {
        /*
        for(int i = 0; i < Colors.Length; i++)
        {
            Colors[i] = new Color(Random.Range(0,1f), Random.Range(0,1f), Random.Range(0,1f));
        }
        */

        //body + head
        Colors[0] = Colors[2] = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

        //facial hair + hair
        Colors[5] = Colors[6] = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));


        Colors[1] = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

        Colors[3] = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

        Colors[4] = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

        Colors[7] = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

    }

    public void RandomColorsHumanIsh()
    {
        /*
        for(int i = 0; i < Colors.Length; i++)
        {
            Colors[i] = new Color(Random.Range(0,1f), Random.Range(0,1f), Random.Range(0,1f));
        }
        */

        // human skin:
        // r = 0,1765 - 1
        // g = 0,8 * r +- 0,05x
        // b = 0,7 * r +- 0,05x

        float red = Random.Range(0.1765f, 1);

        //body + head
        Colors[0] = Colors[2] = new Color(red, (0.8f+Random.Range(-0.1f, 0.1f))*red, (0.7f + Random.Range(-0.1f, 0.1f)) * red);

        //hair
        Colors[6] = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

        //facial hair - always 10% darker than hair, but same color
        Colors[5] = Colors[6] * 0.9f;

        Colors[1] = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

        Colors[3] = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

        Colors[4] = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

        Colors[7] = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));



    }

    public float TemporaryColorTimeRemaining = 0;
    public override void TemporaryColor(Color color, float Time)
    {
        colors_original = (Color[])Colors.Clone();

        for (int i = 0; i < Colors.Length; i++)
        {
            Colors[i] = color;
        }
        TemporaryColorTimeRemaining = Time;
        UpdateEverything(LastDirection);
    }
}
