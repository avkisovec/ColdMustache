using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeWhenPlayerEnters : MonoBehaviour
{


    /*
    
        if player is located within specified coordinates,
        assigned sprite renderer will slowly become transparent
        (and slowly becoma opaque when player leaves)
    
     */

    public Transform AreaCartesianPixel;

    public SpriteRenderer sr;

    public float ChangeSpeed = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 PlayerPos = new Vector2(UniversalReference.PlayerObject.transform.position.x,
            UniversalReference.PlayerObject.transform.position.y);

        float Alpha = sr.color.a;

        if(PlayerPos.x > AreaCartesianPixel.position.x &&
            PlayerPos.x < AreaCartesianPixel.position.x + AreaCartesianPixel.lossyScale.x &&
            PlayerPos.y > AreaCartesianPixel.position.y &&
            PlayerPos.y < AreaCartesianPixel.position.y + AreaCartesianPixel.lossyScale.y)
        {
            Alpha -= ChangeSpeed * Time.deltaTime;
            if (Alpha < 0) Alpha = 0;
        }
        else{
            Alpha += ChangeSpeed * Time.deltaTime;
            if (Alpha > 1) Alpha = 1;
        }

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Alpha);
    }
}
