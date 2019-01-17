using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManagerGeneric : SpriteManagerBase {

    public SpriteRenderer[] spriteRenderers;
    public string[] spritePaths;
    public Sprite[][] sprites = null; //from paths
    public bool[] sprites_isTriple;  //false if single sprite ([0]) is used for all directions
    public Color[] colors;
    
    Color[] colors_original; //when some color is forced (red on injury...), it will replace all colors[] - this is to return to normal colors

    //public Vector3[] positions;

    Sprite[] Empty;

    public bool GetDirectionFromEntity = true;
    Entity e;

	// Use this for initialization
	void Start () {
        Empty = Resources.LoadAll<Sprite>("Entities/Human/Parts/EmptyParts");

        //loading sprites from paths, if needed
        if(sprites == null)
        {
            sprites = new Sprite[spritePaths.Length][];
            LoadSpritesFromPaths();
        }

        if (GetDirectionFromEntity)
        {
            e = GetComponent<Entity>();
        }
        
        colors_original = (Color[])colors.Clone();

        //UpdateEverything(0);
    }

    public void LoadSpritesFromPaths()
    {
        for (int i = 0; i < spritePaths.Length; i++)
        {
            sprites[i] = Resources.LoadAll<Sprite>(spritePaths[i]);
        }
    }

    // Update is called once per frame
    void Update () {

        if (GetDirectionFromEntity)
        {
            Vector2 delta = e.LookingToward - (Vector2)transform.position;
            
            if(Mathf.Abs(delta.x) > Mathf.Abs(delta.y)){
                if(delta.x > 0)
                {
                    UpdateIfNeeded(0);
                }
                else
                {
                    UpdateIfNeeded(180);
                }
            }
            else
            {
                if (delta.y > 0)
                {
                    UpdateIfNeeded(90);
                }
                else
                {
                    UpdateIfNeeded(270);
                }
            }

            if (TemporaryColorTimeRemaining > 0)
            {
                TemporaryColorTimeRemaining-= Time.deltaTime;
            }
            if (TemporaryColorTimeRemaining != -999999 && TemporaryColorTimeRemaining <= 0)
            {
                TemporaryColorTimeRemaining = -999999;

                colors = (Color[])colors_original.Clone();

                UpdateEverything(LastDirection);
            }
            

        }

        if (Input.GetKey(KeyCode.C))
        {
            RandomColors();
        }

	}


    public int LastDirection = -1;

    public override void UpdateIfNeeded(int Direction)
    {
        if(Direction != LastDirection)
        {
            UpdateEverything(Direction);
            LastDirection = Direction;
        }
    }

    public override void UpdateEverything(int Direction)
    {
        int Length = spriteRenderers.Length;
        switch (Direction)
        {
            case 0:
                for (int i = 0; i < Length; i++)
                {
                    if (sprites_isTriple[i])
                    {
                        spriteRenderers[i].sprite = sprites[i][1];
                    }
                    else
                    {
                        spriteRenderers[i].sprite = sprites[i][0];
                    }
                    spriteRenderers[i].color = colors[i];
                    spriteRenderers[i].flipX = false;
                }
                return;
            case 90:
                for (int i = 0; i < Length; i++)
                {
                    if (sprites_isTriple[i])
                    {
                        spriteRenderers[i].sprite = sprites[i][2];
                    }
                    else
                    {
                        spriteRenderers[i].sprite = sprites[i][0];
                    }
                    spriteRenderers[i].color = colors[i];
                    spriteRenderers[i].flipX = false;
                }
                return;
            case 180:
                for (int i = 0; i < Length; i++)
                {
                    if (sprites_isTriple[i])
                    {
                        spriteRenderers[i].sprite = sprites[i][1];
                    }
                    else
                    {
                        spriteRenderers[i].sprite = sprites[i][0];
                    }
                    spriteRenderers[i].color = colors[i];
                    spriteRenderers[i].flipX = true;
                }
                return;
            case 270:
                for (int i = 0; i < Length; i++)
                {
                    spriteRenderers[i].sprite = sprites[i][0];
                    spriteRenderers[i].color = colors[i];
                    spriteRenderers[i].flipX = false;
                }
                return;


        }
    }
    
    public void UpdateEverything()
    {
        UpdateEverything(LastDirection);
    }

    public float TemporaryColorTimeRemaining = 0;
    public override void TemporaryColor(Color color, float Time)
    {
        for(int i = 0; i < colors.Length; i++)
        {
            colors[i] = color;
        }
        TemporaryColorTimeRemaining = Time;
        UpdateEverything(LastDirection);
    }
    


    public void RandomColors()
    {
        for(int i = 0; i < colors.Length; i++)
        {
            colors[i] = new Color(Random.Range(0,1f), Random.Range(0,1f), Random.Range(0,1f));
        }
        UpdateEverything(LastDirection);
    }

    public void UnifiedColor(int Source, int[] Affected) //takes color from a specific slot, and sets it to all specified slots; case: body color is set to head and hands, hair color is set to facial hair
    {
        for(int i = 0; i < Affected.Length; i++)
        {
            colors[Affected[i]] = colors[Source];
        }
    }
    
    public string[] SaveCurrentAsPreset()
    {
        List<string> OutputLines = new List<string>();

        for(int i = 0; i < spritePaths.Length; i++)
        {
            OutputLines.Add("["+i+"]");
            OutputLines.Add(spritePaths[i]);
            OutputLines.Add("#" + colors[i].r + ";" + colors[i].g + ";" + colors[i].b + ";" + colors[i].a + ";0");
        }

        return OutputLines.ToArray();
    }

}
