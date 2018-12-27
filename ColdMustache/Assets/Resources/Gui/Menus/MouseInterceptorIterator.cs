using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInterceptorIterator : MonoBehaviour {
    
    //as the main MouseInterceptor is static, it cant update every frame
    //this script is attached under scriptholder and acts as main mouseInterceptor's update function

        
	void Update () {
        
        MouseInterceptor.FramesSinceMouseHoveredOverAMenu++;
        
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            MouseInterceptor.MouseBeingIntercepted = false;
        }
	}
}
