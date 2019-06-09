using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZIndexManager_ForChildren : MonoBehaviour
{
    /*
            Simplified version of Z-Index manager that organises all children on start
        */

    public bool ENABLED = true;

    public ZIndexManager.Types Type = ZIndexManager.Types.Objects;


    //gets added to the type's default value; 
    //use 0 ... -9 for non-objects (such as ordering GUI) (or just dont use this script if you want to fine-tune)
    //use 0 ... +9 for things below 0 (floors, water...)
    //dont recommend using for objects (entities)
    public float RelativeValue = 0;

    void Start()
    {
        if(!ENABLED) return;
        ENABLED = false;

        SetZValue();
    }

    public void SetZValue()
    {
        int ChildCount = transform.childCount;

        for(int i = 0; i < ChildCount; i++){

            Transform t = transform.GetChild(i);
            switch (Type)
            {
                case ZIndexManager.Types.Objects:
                    t.position = new Vector3(t.position.x, t.position.y, Mathf.RoundToInt(t.position.y) + RelativeValue);
                    break;
                case ZIndexManager.Types.Floors:
                    t.position = new Vector3(t.position.x, t.position.y, ZIndexManager.Const_Floors + RelativeValue + ((t.position.y) / 100));
                    return;
                case ZIndexManager.Types.GUI:
                    t.position = new Vector3(t.position.x, t.position.y, ZIndexManager.Const_GUI + RelativeValue);
                    break;
                case ZIndexManager.Types.HUD:
                    t.position = new Vector3(t.position.x, t.position.y, ZIndexManager.Const_HUD + RelativeValue);
                    break;
                case ZIndexManager.Types.FxAboveDark:
                    t.position = new Vector3(t.position.x, t.position.y, ZIndexManager.Const_FxAboveDark + RelativeValue);
                    break;
                case ZIndexManager.Types.Darkness:
                    t.position = new Vector3(t.position.x, t.position.y, ZIndexManager.Const_Darkness + RelativeValue);
                    break;
                case ZIndexManager.Types.FxUnderDark:
                    t.position = new Vector3(t.position.x, t.position.y, ZIndexManager.Const_FxUnderDark + RelativeValue);
                    break;
                case ZIndexManager.Types.UnderFloors:
                    t.position = new Vector3(t.position.x, t.position.y, ZIndexManager.Const_FxUnderDark + RelativeValue);
                    break;
            }
        }

        }

        
}
