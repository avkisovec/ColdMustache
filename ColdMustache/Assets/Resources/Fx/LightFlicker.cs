using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {

    public Color ColorA;
    public Color ColorB;

    public float AlphaMin = 0.3f;
    public float AlphaMax = 1;
    public float MaxAlphaDeltaPerFrame = 0.1f;
    public float MaxColorShiftPerFrame = 0.1f;

    float MinR;
    float MaxR;
    float MinG;
    float MaxG;
    float MinB;
    float MaxB;

    SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        if(ColorA.r > ColorB.r)
        {
            MaxR = ColorA.r;
            MinR = ColorB.r;
        }
        else
        {
            MaxR = ColorB.r;
            MinR = ColorA.r;
        }
        if (ColorA.g > ColorB.g)
        {
            MaxG = ColorA.g;
            MinG = ColorB.g;
        }
        else
        {
            MaxG = ColorB.g;
            MinG = ColorA.g;
        }
        if (ColorA.b > ColorB.b)
        {
            MaxB = ColorA.b;
            MinB = ColorB.b;
        }
        else
        {
            MaxB = ColorB.b;
            MinB = ColorA.b;
        }

        sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        float r = sr.color.r + Random.Range(-MaxColorShiftPerFrame, MaxColorShiftPerFrame);
        if (r < MinR)
        {
            r = MinR;
        }
        else if (r > MaxR)
        {
            r = MaxR;
        }
        float g = sr.color.g + Random.Range(-MaxColorShiftPerFrame, MaxColorShiftPerFrame);
        if (g < MinG)
        {
            g = MinG;
        }
        else if (g > MaxG)
        {
            g = MaxG;
        }
        float b = sr.color.b + Random.Range(-MaxColorShiftPerFrame, MaxColorShiftPerFrame);
        if (b < MinB)
        {
            b = MinB;
        }
        else if (b > MaxB)
        {
            b = MaxB;
        }

        float a = sr.color.a + Random.Range(-MaxAlphaDeltaPerFrame, MaxAlphaDeltaPerFrame);
        if (a < AlphaMin)
        {
            a = AlphaMin;
        }
        else if (a > AlphaMax)
        {
           a = AlphaMax;
        }

        sr.color = new Color(r, g, b, a);

	}
}
