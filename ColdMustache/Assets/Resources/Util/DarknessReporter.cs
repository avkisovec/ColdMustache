using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessReporter : CentrallyUpdatable {

	// Use this for initialization
	void Start () {
        CentralUpdater.Scripts.Add(this);
	}
	
    public override void CentralUpdate()
    {
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
