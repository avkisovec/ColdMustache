using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgManagerBasedOnHoveredBtn : MonoBehaviour
{
    
    public Button01[] Buttons;
    public SpriteRenderer[] Srs;

    public float AlphaChengePerSecond = 1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < Buttons.Length; i++){

            float Alpha = Srs[i].color.a;

            if(Buttons[i].IsHovered) Alpha += Time.deltaTime*AlphaChengePerSecond;
            else Alpha -= Time.deltaTime * AlphaChengePerSecond;

            if(Alpha > 1 ) Alpha = 1;
            else if(Alpha < 0) Alpha = 0;

            Srs[i].color = new Color(Srs[i].color.r, Srs[i].color.g, Srs[i].color.b, Alpha);

        }
    }
}
