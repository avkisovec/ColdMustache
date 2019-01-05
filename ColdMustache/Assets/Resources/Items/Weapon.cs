using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    //so that update is executed only when the weapon is currently held
    public bool CurrentlyActive = false;


    public virtual void TryShooting(Vector3 Target)
    {

    }

    public virtual void TryAltFire(Vector3 Target)
    {

    }

    public virtual void TryAltFire()
    {

    }

    public virtual void ForceReload()
    {

    }

    public virtual void OnBecomingActive()
    {

    }

}
