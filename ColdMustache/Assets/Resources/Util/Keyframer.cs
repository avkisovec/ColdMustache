using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyframer : MonoBehaviour
{

    //how long it takes to go through all the keyframes
    public float Length = 2;

    //pretend time is a bit higer or lower
    public float TimeOffset = 0;

    //the actual keyframes
    //remember the first and last should be the same so it loops back nicely
    //remember to use tricks like 0=360, from 180 to 0 it will move clockwise and from 180 to 360 it will move counter-clockwise but the result is same
    public float[] Angles;




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DoYourThing(float TimeElapsed){

        TimeElapsed += TimeOffset;

        //represents the % ratio the animation is currently at
        //0 - 1
        float Ratio = (TimeElapsed % Length) / Length;

        //0 - n
        float AdjustedRatio = Ratio * (Angles.Length - 1);

        //represents inbetween what two keyframes the animation is
        //0 means between 0 and 1, 3 means between 3 and 4
        int Keyframe = Mathf.FloorToInt(AdjustedRatio);

        //represents how far between the two keyframes the anim is
        //0 - 1
        //0 means exactly the first keyframe within range, 1 (hypothetical) would mean exactly the other keyframe in the range
        float KeyframeRatio = AdjustedRatio - Keyframe;


        float Angle = Angles[Keyframe]*(1-KeyframeRatio) + Angles[Keyframe+1]*(KeyframeRatio);


        transform.localRotation = Quaternion.Euler(0,0,Angle);



    }

}
