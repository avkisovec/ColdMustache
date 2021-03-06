﻿using UnityEngine;

public class FluidAnim : MonoBehaviour {

    /*
    
        the cocept 

        who images overlaid (A is above B (A = Above B = Below))
        A changes in transparency, B is always fully opaque

        lets say A starts Fully opaque, so B is not visible
        A is slowly becoming more and more transparent, slowly revealing B, until A is fully transparent
        this will create a "merging" effect, where A smoothly transitions into B

        when A is fully transparent, it will randomly change (new sprite, color)
        Then the above described process goes in reverse
        just as before the old A sprite slowly transitions into B sprite, B sprite now transitions into new A sprite
        and once A if fully opaque and B not visible, set new sprite for B and repeat
    
     */

    public SpriteRenderer srA;
    public SpriteRenderer srB;

    public Color ColorA;
    public Color ColorB;

    public string SpriteSheetPath;
    Sprite[] Sprites;

    public int LastSpriteIndex = -1;

    public float Alpha = 1;

    public bool GoinUp = true;

    //1 is about 1 phase per second (2 seconds per full cycle) higher number means faster animation, lower number (0.5) means longer animation
    public float Speed = 1;

	// Use this for initialization
	void Start () {
        Sprites = Resources.LoadAll<Sprite>(SpriteSheetPath);
        
	}
	
	// Update is called once per frame
	void Update () {

        if (GoinUp)
        {
            if (Alpha + Time.deltaTime * Speed >= 1)
            {
                //phase 1 - A is completely visible, randomly roll B
                Alpha = 1;
                RollB();
                GoinUp = false;
            }
            else
            {
                //phase 4 - A slowly becoming visible
                Alpha += Time.deltaTime * Speed;
            }
        }
        else
        {
            //phase 3 - A completely invisible, randomly roll A
            if (Alpha - Time.deltaTime * Speed <= 0)
            {
                Alpha = 0;
                RollA();
                GoinUp = true;
            }
            else
            {
                //phase 2 - A slowly becoming transparent untill invisible
                Alpha -= Time.deltaTime * Speed;
            }
        }

        srA.color = new Color(srA.color.r, srA.color.g, srA.color.b, Alpha);
    }

    public void RollA()
    {
        srA.sprite = Sprites[GetNewSpriteIndex()];
        srA.color = new Color(
            Random.Range(ColorA.r, ColorB.r),
            Random.Range(ColorA.g, ColorB.g),
            Random.Range(ColorA.b, ColorB.b),
            0
            );
        srA.flipX = Util.Coinflip();
        srA.flipY = Util.Coinflip();
    }

    public void RollB()
    {
        srB.sprite = Sprites[GetNewSpriteIndex()];
        srB.color = new Color(
            Random.Range(ColorA.r, ColorB.r),
            Random.Range(ColorA.g, ColorB.g),
            Random.Range(ColorA.b, ColorB.b),
            1
            );
        srB.flipX = Util.Coinflip();
        srB.flipY = Util.Coinflip();
    }

    public int GetNewSpriteIndex()
    {
        int Output = Random.Range(0, Sprites.Length);
        while (Output == LastSpriteIndex)
        {
            Output = Random.Range(0, Sprites.Length);
        }
        LastSpriteIndex = Output;
        return Output;
    }
}
