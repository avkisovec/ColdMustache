using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyframerController : MonoBehaviour
{
    
    /*
    
    this script has several keyframer under control

    it basically just tells them what time it is and its the keyframers job to then based on keyframes rotate into position
        
    
    
     */


    //ratio how quickly will time elapse for the keyframers
    // 1 = normal, 0.5 means animations are 50% slower
    public float TimeSpeed = 1;


    public bool Paused = false;

    public Keyframer[] Keyframers;

    //time all the keyframers have been running for, considering pauses and timespeed
    float ManagedTime = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Paused) return;

        ManagedTime += TimeSpeed * Time.deltaTime;

        //keyframers dont work well with; 2520 because it has many factors
        if(ManagedTime < 0) ManagedTime += 2520;

        for(int i = 0; i < Keyframers.Length; i++){
            Keyframers[i].DoYourThing(ManagedTime);
        }        

    }
}
