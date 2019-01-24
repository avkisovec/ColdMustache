using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPointer : MonoBehaviour {

    /*
     * on players ship
     * 
     * points to its assigned enemy ship
     * 
     * deletes self if its ship is desrtoyed
     * 
     * 
     */

    public Transform Ship;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(Ship == null)
        {
            Destroy(this.gameObject);
            return;
        }

        transform.localPosition = new Vector3(0, 0, -10);
        Util.RotateTransformToward(transform, Ship);

	}
}
