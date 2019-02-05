using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideStuffWhenMouseIsNotInMenu : MonoBehaviour
{
    /*
     *  this script is originally intended for GearInventory
     *
     *  if a mouse is not in the inventory, the background and slots and stuff will turn transparent
     *  this is so that player can keep it on screen without restricting vision    
     *
     *
     */

    float CurrAlpha = 1;
    float CurrAlphaForMostlyTransparent = 1; //CurrAlpha * 0.8f + 0.2f

    //how quickly it will become opaque/transparent when hovered/not, gain per seconds (higher value means faster)
    public float AlphaGain = 4f;
    public float ALphaLoss = 2f;

    public List<SpriteRenderer> ToBecomeFullyTransparent = new List<SpriteRenderer>();
    public List<SpriteRenderer> ToBecomeMostlyTransparent = new List<SpriteRenderer>(); //mostly means 80%

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(MouseInterceptor.IsMouseHoveringMenu()){
            if(CurrAlpha != 1) CurrAlpha += Time.deltaTime * AlphaGain;
        }
        else{
            if(CurrAlpha != 0) CurrAlpha -= Time.deltaTime * ALphaLoss;
        }
        if(CurrAlpha < 0){
            CurrAlpha = 0;
        }
        else if(CurrAlpha > 1){
            CurrAlpha = 1;
        }
        CurrAlphaForMostlyTransparent = CurrAlpha * 0.8f + 0.2f;

        foreach(SpriteRenderer sr in ToBecomeFullyTransparent){
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, CurrAlpha);
        }
        foreach (SpriteRenderer sr in ToBecomeMostlyTransparent)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, CurrAlphaForMostlyTransparent);
        }
        
    }
}
