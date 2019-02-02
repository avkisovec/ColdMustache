using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEffect_ColorWave : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = StartingColor[0];

        ShiftR = new float[Phases];
        ShiftG = new float[Phases];
        ShiftB = new float[Phases];
        ShiftA = new float[Phases];

        for (int i = 0; i < Phases; i++)
        {            
            ShiftR[i] = ((float)EndingColor[i].r - (float)StartingColor[i].r) / (float)Lifespan[i];
            ShiftG[i] = ((float)EndingColor[i].g - (float)StartingColor[i].g) / (float)Lifespan[i];
            ShiftB[i] = ((float)EndingColor[i].b - (float)StartingColor[i].b) / (float)Lifespan[i];
            ShiftA[i] = ((float)EndingColor[i].a - (float)StartingColor[i].a) / (float)Lifespan[i];            
        }
    }

    public int Phase = 0;
    public int Phases;
    public float[] Lifespan;
    public float[] DelayedStart;
    
    public Color[] StartingColor;
    public Color[] EndingColor;
    public float[] ShiftR;
    public float[] ShiftG;
    public float[] ShiftB;
    public float[] ShiftA;

    SpriteRenderer sr;
    
    void Update()
    {
        if (DelayedStart[Phase] > 0)
        {
            DelayedStart[Phase] -= Time.deltaTime;
            return;
        }

        if (Lifespan[Phase] <= 0)
        {
            Phase++;

            if (Phase == Phases)
            {
                sr.color = EndingColor[Phase-1];
                Destroy(this);
            }

            return;
        }

        Lifespan[Phase] -= Time.deltaTime;
        
        Color c = sr.color;
        c = new Color(c.r + ShiftR[Phase]*Time.deltaTime, c.g + ShiftG[Phase] * Time.deltaTime, c.b + ShiftB[Phase] * Time.deltaTime, c.a + ShiftA[Phase] * Time.deltaTime);
        sr.color = c;        
    }
    
}
