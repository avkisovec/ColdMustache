using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManagerBase : MonoBehaviour {
        
    public virtual void TemporaryColor(Color clr, float Time)
    {

    }

    //for some reason i had problem accessing the LastDirection in SpriteManagerGeneric directly, so this is a workaround
    public virtual int GetLastDirection()
    {
        return -1;
    }

    public virtual void UpdateIfNeeded(int Direction)
    {

    }

    public virtual void UpdateEverything(int Direction)
    {

    }

    public virtual void LookTowardAngle(float Angle)
    {

    }

}
