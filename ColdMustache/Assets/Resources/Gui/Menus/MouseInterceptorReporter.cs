using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInterceptorReporter : MonoBehaviour {

    //this script is attached to some menu/button
    //reports when a mouse is hovering it (therefore the mouse shouldnt do normal world things like shooting)
    
	void Update () {
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.Mouse0) &&
            MouseWorldPos.x > transform.position.x - (transform.lossyScale.x / 2) &&
            MouseWorldPos.x < transform.position.x + (transform.lossyScale.x / 2) &&
            MouseWorldPos.y > transform.position.y - (transform.lossyScale.y / 2) &&
            MouseWorldPos.y < transform.position.y + (transform.lossyScale.y / 2)       
            )
        {
            //MouseInterceptor.ReportAMouseCurrentlyOverAMenu();
            MouseInterceptor.MouseBeingIntercepted = true;
        }
    }
}
