using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleNonNoticably : MonoBehaviour {

    bool WiggleLeft = true;

            //the purpose of this script is to force updates on collisions - objects sometimes dont update collision calls if they stay still for a while and collision status doesnt change
            
	void Update () {
        if (WiggleLeft)
        {
            transform.position += new Vector3(0, 0, 0.001f);
            WiggleLeft = false;
        }
        else
        {
            transform.position += new Vector3(0, 0, -0.001f);
            WiggleLeft = true;
        }
	}
}
