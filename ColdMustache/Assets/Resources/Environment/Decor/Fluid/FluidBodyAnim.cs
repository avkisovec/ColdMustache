using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidBodyAnim : MonoBehaviour
{


    /*
    
        the old FluidAnim cocept 

        who images overlaid (A is above B (A = Above B = Below))
        A changes in transparency, B is always fully opaque

        lets say A starts Fully opaque, so B is not visible
        A is slowly becoming more and more transparent, slowly revealing B, until A is fully transparent
        this will create a "merging" effect, where A smoothly transitions into B

        when A is fully transparent, it will randomly change (new sprite, color)
        Then the above described process goes in reverse
        just as before the old A sprite slowly transitions into B sprite, B sprite now transitions into new A sprite
        and once A if fully opaque and B not visible, set new sprite for B and repeat
    

        THIS IS EDITED VERSION OF THAT that supports multiple spriteRenderers that it updates centrally
        so you can have a large body of water where every tile will be the same as others (color sprite phase and everything)


     */
    public List<Sprite> Sprites = new List<Sprite>();
    public List<SpriteRenderer> spriteRenderersA = new List<SpriteRenderer>();
    public List<SpriteRenderer> spriteRenderersB = new List<SpriteRenderer>();

    public Color ColorA;
    public Color ColorB;

    public int LastSpriteIndex = -1;

    public float Alpha = 1;

    public bool GoinUp = true;

    //1 is about 1 phase per second (2 seconds per full cycle) higher number means faster animation, lower number (0.5) means longer animation
    public float Speed = 1;


    

    // Update is called once per frame
    void Update()
    {
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
        foreach(SpriteRenderer sr in spriteRenderersA){
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Alpha);
        }
    }

    public void RollA()
    {
        int NewSpriteIndex = GetNewSpriteIndex();
        bool FlipX = Util.Coinflip();
        bool FLipY = Util.Coinflip();
        Color clr = new Color(
                Random.Range(ColorA.r, ColorB.r),
                Random.Range(ColorA.g, ColorB.g),
                Random.Range(ColorA.b, ColorB.b),
                0
                );

        foreach(SpriteRenderer sr in spriteRenderersA){
            sr.sprite = Sprites[NewSpriteIndex];
            sr.color = clr;
            sr.flipX = FlipX;
            sr.flipY = FLipY;
        }
        
    }

    public void RollB()
    {
        int NewSpriteIndex = GetNewSpriteIndex();
        bool FlipX = Util.Coinflip();
        bool FLipY = Util.Coinflip();
        Color clr = new Color(
                Random.Range(ColorA.r, ColorB.r),
                Random.Range(ColorA.g, ColorB.g),
                Random.Range(ColorA.b, ColorB.b),
                1
                );

        foreach (SpriteRenderer sr in spriteRenderersB)
        {
            sr.sprite = Sprites[NewSpriteIndex];
            sr.color = clr;
            sr.flipX = FlipX;
            sr.flipY = FLipY;
        }
    }

    public int GetNewSpriteIndex()
    {
        int Output = Random.Range(0, Sprites.Count);
        while (Output == LastSpriteIndex)
        {
            Output = Random.Range(0, Sprites.Count);
        }
        LastSpriteIndex = Output;
        return Output;
    }
}