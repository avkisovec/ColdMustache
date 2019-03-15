using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegAnimBasedOnDeltaPos : MonoBehaviour
{
    Vector3 LastPos;

    public HumanLegsManager LegsManager;
    public Entity e;

    public Vector3 DeltaPos;

    bool EvenFrame = false;

    void Start()
    {
        LastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //there is sometimes problem where deltapos between two frames is 0, so it forces the idle animation; checking only every other frame should fix it (the bug is not visible when going frame by frame)
        EvenFrame = !EvenFrame;
        if(EvenFrame) return;
        

        DeltaPos = transform.position - LastPos;
        LastPos = transform.position;

        //not moving - idle
        if(Mathf.Abs(DeltaPos.x)<0.0001f && Mathf.Abs(DeltaPos.y)<0.0001f){
            LegsManager.RequestIdle(e.LookingToward);
            return;
        }


        if(Mathf.Abs(DeltaPos.x) >= Mathf.Abs(DeltaPos.y)){

            if(DeltaPos.x>0){
                LegsManager.RequestRunRight(e.LookingToward.x > 0);
                return;
            }
            else{
                LegsManager.RequestRunLeft(e.LookingToward.x > 0);
                return;
            }

            
        }
        else
        {
            if(DeltaPos.y > 0)
            {
                LegsManager.RequestRunUp(e.LookingToward.x > 0);
                return;
            }
            else
            {
                LegsManager.RequestRunDown(e.LookingToward.x > 0);
                return;
            }
            
        }


    }
}
