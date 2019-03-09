using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Chain : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<SpriteRenderer>().sprite = sprite;

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
        

        CurrScale = StartingScale;
        CurrHorizontalWind = StartingHorizontalWind;
        CurrVerticalWind = StartingVerticalWind;


        transform.localScale = new Vector3(CurrScale, CurrScale);
        GetComponent<SpriteRenderer>().color = StartingColor;

        if (LeaveParent)
        {
            transform.parent = null;
        }

        //no first one exists -> no particles exist -> become the first and last one
        if(ParticleChainManager.FirstOne == null){
            ParticleChainManager.FirstOne = this;
            ParticleChainManager.LastOne = this;
        }
        //link yourself to last one and become new last one
        else{
            Parent = ParticleChainManager.LastOne;
            Parent.Child = this;
            ParticleChainManager.LastOne = this;
        }

    }
    
    public bool LeaveParent = false;

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


    public Particle_Chain Parent;
    public Particle_Chain Child;




    public void CustomUpdate()
    {
        //death
        if (Lifespan <= 0)
        {
            //i have no parent, therefore im first one -> child becomes new first one
            if(Parent==null){
                ParticleChainManager.FirstOne = Child;
                if(Child!=null) Child.Parent = null;
            }
            else{
                //i had a parent and a child, so ill bridge them
                if(Child != null){
                    Parent.Child = Child;
                    Child.Parent = Parent;
                }
                //i only had a parent, so i had to be the last one - parent is the new last one
                else{
                    Parent.Child = null;
                    ParticleChainManager.LastOne = Parent;
                }

            }

            Destroy(gameObject);
            return;
        }
        Lifespan--;


        CurrScale += ShiftScale;
        CurrHorizontalWind += ShiftHorizontalWind;
        CurrVerticalWind += ShiftVerticalWind;

        transform.localScale = new Vector3(CurrScale, CurrScale);

        Color c = GetComponent<SpriteRenderer>().color;
        c = new Color(c.r + ShiftR, c.g + ShiftG, c.b + ShiftB, c.a + ShiftA);
        GetComponent<SpriteRenderer>().color = c;

        
        transform.localPosition += new Vector3(CurrHorizontalWind, CurrVerticalWind);
        //transform.position += new Vector3(CurrHorizontalWind, CurrVerticalWind);

    }


}
