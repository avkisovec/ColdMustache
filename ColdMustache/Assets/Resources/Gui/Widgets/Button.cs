using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    
    public bool Clicked = false;

    public bool Held = false;

    public bool Hovered = false;

    public Transform PixelToUseAsArea;


    Camera c;

    void Start()
    {
        c = Camera.main;
    }

    void Update()
    {

        //defaults to false on every frame unless later overwritten
        Clicked = false;


        if(Held){

            if(Input.GetKeyUp(KeyCode.Mouse0)){
                Clicked = true;
                Held = false;
            }
        }



        Vector2 MouseWorldPos = c.ScreenToWorldPoint(Input.mousePosition);
        if (MouseWorldPos.x > PixelToUseAsArea.position.x - (PixelToUseAsArea.lossyScale.x / 2) &&
            MouseWorldPos.x < PixelToUseAsArea.position.x + (PixelToUseAsArea.lossyScale.x / 2) &&
            MouseWorldPos.y > PixelToUseAsArea.position.y - (PixelToUseAsArea.lossyScale.y / 2) &&
            MouseWorldPos.y < PixelToUseAsArea.position.y + (PixelToUseAsArea.lossyScale.y / 2)
            )
        {
            //button is currently hovered
            Hovered = true;
        }
        else{
            //button is not hovered, therefore anything else doesnt matter
            Hovered = false;

            //when you stop hovering it invalidates the mousepress
            Held = false;


            return;
        }

        if(Input.GetKeyDown(KeyCode.Mouse0)){

            //only valid if it originated here (if originated during hover, which is true as bcs of the return on not hover)
            Held = true;

        }


    }
}
