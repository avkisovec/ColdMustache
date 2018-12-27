using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramerateManager : MonoBehaviour {


    /*
     * 0,01666 = 1/60 of a second
     * 
     * framerate manager

        FramesToFit60()

        if your game runs on 30 fps, it returns 2
        if your run on 60 fps it returns 1
        if you run on 120 fps it returns 0-1

        if you run on 55-ish it returns 1, sometimes 2

        in code you will have structure like:

        for(int i = 0; i < Frameratemanager.FramesToFit60, i++){
	        ...
        }

        whatever you put into this structure will be called 60 times a second no matter what
     * 
     * 
     * 
     */


    //public List<Vector2> debug = new List<Vector2>();

    public float ExpectedFrameRenderingTime = 1f / 60f;

    public float CumulatedTime = 0;

    public static int FramesRequiredToRenderToKeepUp = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        FramesRequiredToRenderToKeepUp = 0;

        CumulatedTime += Time.deltaTime;

        while(CumulatedTime > ExpectedFrameRenderingTime)
        {
            FramesRequiredToRenderToKeepUp++;
            CumulatedTime -= ExpectedFrameRenderingTime;
        }
        

        /*
        float nu = Time.deltaTime;

        for(int i = 0; i < debug.Count; i++)
        {
            if(debug[i].x == nu)
            {
                debug[i] = new Vector2(debug[i].x, debug[i].y + 1);
                return;
            }
        }
        debug.Add(new Vector2(nu, 0));
        */
	}
}
