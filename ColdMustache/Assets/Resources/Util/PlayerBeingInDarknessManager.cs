using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeingInDarknessManager : MonoBehaviour {

    public static int FramesSincePlayerWasInDarkness = 0;

    public static bool IsPlayerInDarkness = false;

    private void Awake()
    {
        FramesSincePlayerWasInDarkness = 0;
        IsPlayerInDarkness = false;
    }

    // Update is called once per frame
    void Update () {
		
        if(FramesSincePlayerWasInDarkness < 1000)
        {
            FramesSincePlayerWasInDarkness++;
        }

        if (FramesSincePlayerWasInDarkness < 2)
        {
            IsPlayerInDarkness = true;
        }
        else
        {
            IsPlayerInDarkness = false;
        }

    }

    public static void ReportPlayerBeingInDarkness()
    {
        FramesSincePlayerWasInDarkness = 0;
    }

    
}
