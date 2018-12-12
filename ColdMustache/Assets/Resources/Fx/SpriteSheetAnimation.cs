using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSheetAnimation : MonoBehaviour {

    public enum Modes { Repeat, Destroy };

    public Modes Mode = SpriteSheetAnimation.Modes.Destroy;

    public float LifeSpanInSeconds = 1;
    float MaxLifeSpan;

    int CurrFrameRounded = 0;

    SpriteRenderer sr;

    public Sprite[] Sprites;

    // Use this for initialization
    void Start () {

        MaxLifeSpan = LifeSpanInSeconds;
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = Sprites[CurrFrameRounded];
    }
	
	// Update is called once per frame
	void Update () {

        LifeSpanInSeconds -= Time.deltaTime;
        CurrFrameRounded = Sprites.Length - Mathf.RoundToInt(LifeSpanInSeconds / MaxLifeSpan * Sprites.Length);

        if (!(CurrFrameRounded < Sprites.Length))
        {
            if(Mode == Modes.Destroy)
            {
                Destroy(this.gameObject);
                return;
            }
            if(Mode == Modes.Repeat)
            {
                LifeSpanInSeconds = MaxLifeSpan;
                CurrFrameRounded = 0;
            }
        }

        sr.sprite = Sprites[CurrFrameRounded];
    }
}
