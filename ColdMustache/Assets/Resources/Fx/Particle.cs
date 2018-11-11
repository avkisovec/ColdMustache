using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.AddComponent<SpriteRenderer>().sprite = sprite;

        if (UseShifts)
        {
            ShiftScale = (EndingScale - StartingScale) / Lifespan;
            ShiftHorizontalWind = (EndingHorizontalWind - StartingHorizontalWind) / Lifespan;
            ShiftVerticalWind = (EndingVerticalWind - StartingVerticalWind) / Lifespan;

            /*
             * TODO possible problem - because of flooring the shift, the difference has to be larger than lifespan for any change to take effect
             * could be solved by flooring during assigning of color and keeping color float untill then - subtle changes would be vissible but it would require more performance
             */

            ShiftR = (EndingColor.r - StartingColor.r) / Lifespan;
            ShiftG = (EndingColor.g - StartingColor.g) / Lifespan;
            ShiftB = (EndingColor.b - StartingColor.b) / Lifespan;
            ShiftA = (EndingColor.a - StartingColor.a) / Lifespan;
        }
        
        CurrScale = StartingScale;
        CurrHorizontalWind = StartingHorizontalWind;
        CurrVerticalWind = StartingVerticalWind;


        transform.localScale = new Vector3(CurrScale, CurrScale);
        GetComponent<SpriteRenderer>().color = StartingColor;

        if (StartAt00)
        {
            transform.localPosition = new Vector3(0, 0);
        }
        if (LeaveParent)
        {
            transform.parent = null;
        }
	}

    public bool UseShifts = false; // if false - ending variables equal starting variables and variables dont change throughout particles lifespan
    public bool LeaveParent = false;

    public bool StartAt00 = true;

    public int Lifespan;

    public Sprite sprite;
    
    
    public Color StartingColor;
    public Color EndingColor;
    public float ShiftR;
    public float ShiftG;
    public float ShiftB;
    public float ShiftA;
    
    float CurrScale;
    public float StartingScale;
    public float EndingScale;
    float ShiftScale;

    float CurrHorizontalWind;
    public float StartingHorizontalWind;
    public float EndingHorizontalWind;
    float ShiftHorizontalWind;

    float CurrVerticalWind;
    public float StartingVerticalWind;
    public float EndingVerticalWind;
    float ShiftVerticalWind;
    

    
	void Update () {

        if(Lifespan <= 0)
        {
            Destroy(gameObject);
            return;
        }
        Lifespan--;



        if (UseShifts)
        {
            CurrScale += ShiftScale;
            CurrHorizontalWind += ShiftHorizontalWind;
            CurrVerticalWind += ShiftVerticalWind;

            transform.localScale = new Vector3(CurrScale, CurrScale);

            Color c = GetComponent<SpriteRenderer>().color;
            c = new Color(c.r + ShiftR, c.g + ShiftG, c.b + ShiftB, c.a + ShiftA);
            GetComponent<SpriteRenderer>().color = c;

        }

        transform.localPosition += new Vector3(CurrHorizontalWind, CurrVerticalWind);
        //transform.position += new Vector3(CurrHorizontalWind, CurrVerticalWind);


    }


    public void Ini_BasicParticle()
    {
        UseShifts = false;
        sprite = Resources.LoadAll<Sprite>("Effects/GoopWhite")[0]; 
        Lifespan = 50;
        StartingScale = 1;
        StartingColor = new Color(1, 1, 1);
        StartingHorizontalWind = Random.Range(-0.01f, 0.01f);
        StartingVerticalWind = Random.Range(-0.01f, 0.01f);
    }

    public void Ini_FireSmoke()
    {
        UseShifts = true;
        LeaveParent = true;
        sprite = Resources.LoadAll<Sprite>("GoopWhite")[0];
        Lifespan = 100;
        StartingScale = 1;
        EndingScale = 10;
        StartingColor = new Color(1, 0.5f, 0, 1);
        EndingColor = new Color(0, 0, 0, 0);
        StartingHorizontalWind = Random.Range(-0.01f, 0.01f);
        EndingHorizontalWind = Random.Range(-0.01f, 0.01f);
        StartingVerticalWind = Random.Range(-0.01f, 0.05f);
        EndingVerticalWind = Random.Range(-0.01f, 0.05f);
    }
}
