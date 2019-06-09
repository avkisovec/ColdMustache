using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeLink : MonoBehaviour
{
    /*
    
        used to link one object to another

        if the link target is destroyed, this object is destroyed too

        works only in one direction    
    
     */


    //the target that if is destroyed object with this script is destroyed too
    public Transform Link = null;

    void Update()
    {
        if(Link == null) Destroy(gameObject);
    }
}
