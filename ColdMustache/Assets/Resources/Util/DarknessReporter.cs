using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessReporter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (
            UniversalReference.PlayerObject.transform.position.x > transform.position.x - (transform.lossyScale.x / 2) &&
            UniversalReference.PlayerObject.transform.position.x < transform.position.x + (transform.lossyScale.x / 2) &&
            UniversalReference.PlayerObject.transform.position.y > transform.position.y - (transform.lossyScale.y / 2) &&
            UniversalReference.PlayerObject.transform.position.y < transform.position.y + (transform.lossyScale.y / 2)
            )
        {
            PlayerBeingInDarknessManager.ReportPlayerBeingInDarkness();
        }
    }
}
