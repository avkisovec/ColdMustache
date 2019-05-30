using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZIndexManager : MonoBehaviour {
    /*
        cold mustache - Z index protocol
     
        scenes likely won't be taller than 200 tiles
        they can be as long as they wont, that won't affect the z-index
        
        z -70           (reserved for camera)
        z -50 ... z -60 (reserved for GUI, menus)
        z -40 ... z -50 (reserved for HUD)
        z -30 ... z -40 (reserved for fx and stuff not affected by darkness, such as sparks)
			        also effects like explosion's white flash, injury red screen...
        z -20 ... z -30 (reserved for darkness)
        z -10 ... z -20 (reserved for fx and stuff above eerything but affected by darkness)

	        (10 units margin)

	        for objects:
        z 0	(corresponds to y 0)
        Z 200  (corresponds to y 200)
	        each object can occupy upto -+0.5 from its coordinates (preferably only like +-0.1)

	        (10 units margin)

        z 210 ... z 220 (reserved for floors)

        z 220 ... z 230 (reserved for things below floor - such as water w its ripples and stuff)
    */

    public bool SingleUse = false; //true for immobile objects (walls), false for moving entities

    public enum Types {GUI, HUD, FxAboveDark, Darkness, FxUnderDark, Objects, Floors, UnderFloors};
  
    public Types Type = Types.Objects;

    public const float Const_GUI = -50;
    public const float Const_HUD = -40;
    public const float Const_FxAboveDark = -30;
    public const float Const_Darkness = -20;
    public const float Const_FxUnderDark = -10;
    //value for objects is 0 ... 200, corresponding to their Y position (y0...z0, y200...z200)
    public const float Const_Floors = 210;
    public const float Const_UnderFloors = 220;

    //gets added to the type's default value; 
    //use 0 ... -9 for non-objects (such as ordering GUI) (or just dont use this script if you want to fine-tune)
    //use 0 ... +9 for things below 0 (floors, water...)
    //dont recommend using for objects (entities)
    public float RelativeValue = 0;
    
	void Start () {
        SetZValue();
        if (SingleUse)
        {
            Destroy(this);
            return;
        }
    }
	
	void Update () {
        SetZValue();
	}

    public void SetZValue()
    {
        switch (Type)
        {
            case Types.Objects:
                transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.RoundToInt(transform.position.y) + RelativeValue);
                return;
            case Types.GUI:
                transform.position = new Vector3(transform.position.x, transform.position.y, Const_GUI+RelativeValue);
                return;
            case Types.HUD:
                transform.position = new Vector3(transform.position.x, transform.position.y, Const_HUD + RelativeValue);
                return;
            case Types.FxAboveDark:
                transform.position = new Vector3(transform.position.x, transform.position.y, Const_FxAboveDark + RelativeValue);
                return;
            case Types.Darkness:
                transform.position = new Vector3(transform.position.x, transform.position.y, Const_Darkness + RelativeValue);
                return;
            case Types.FxUnderDark:
                transform.position = new Vector3(transform.position.x, transform.position.y, Const_FxUnderDark + RelativeValue);
                return;
            case Types.Floors:
                transform.position = new Vector3(transform.position.x, transform.position.y, Const_Floors + RelativeValue);
                return;
            case Types.UnderFloors:
                transform.position = new Vector3(transform.position.x, transform.position.y, Const_FxUnderDark + RelativeValue);
                return;
        }
    }
}
