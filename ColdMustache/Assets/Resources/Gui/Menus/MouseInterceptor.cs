using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInterceptor : MonoBehaviour {

    /*
     *  a static script
     *  
     *  when mouse is hovering over some menu/button,
     *  that menu/button reports to this script that the mouse is currently hovering above it
     *  
     *  if you use LMB normally, you shoot
     *  though if you use LMB in a menu, you click on the menu and player shouldnt shoot
     * 
     * 
     */

    private void Awake()
    {
        FramesSinceMouseHoveredOverAMenu = 0;
        MouseBeingIntercepted = false;
    }

    public static int FramesSinceMouseHoveredOverAMenu = 0;
    public static bool MouseBeingIntercepted = false;
    
    public static void ReportAMouseCurrentlyOverAMenu()
    {
        FramesSinceMouseHoveredOverAMenu = 0;
    }
    
    public static bool IsMouseBeingIntercepted()
    {
        return MouseBeingIntercepted;
        /*
        if(FramesSinceMouseHoveredOverAMenu >= 2)
        {
            return false;
        }
        return true;
        */
    }

    public static bool IsMouseAvailable()
    {
        return !MouseBeingIntercepted;
        /*
        if (FramesSinceMouseHoveredOverAMenu >= 2)
        {
            return true;
        }
        return false;
        */
    }

    public static bool IsMouseHoveringMenu()
    {
        if(FramesSinceMouseHoveredOverAMenu >= 2)
        {
            return false;
        }
        return true;
    }

    public static bool IsMouseNotHoveringMenu()
    {
        if (FramesSinceMouseHoveredOverAMenu >= 2)
        {
            return true;
        }
        return false;
    }

    void Update()
    {

        FramesSinceMouseHoveredOverAMenu++;

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            MouseBeingIntercepted = false;
        }
    }

}
