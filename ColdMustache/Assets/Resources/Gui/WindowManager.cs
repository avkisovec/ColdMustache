using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    /*
     *  main purpose of this script is to ensure all active windows are on-screen (cannot be dragged outside of bounds)
     *
     *  this script publicly shares the bounds, and if some window is outside of that it will move to be in correct place
     *
     */

    void Start()
    {
        
    }

    
    public static float BoundsMinX = -1;
    public static float BoundsMaxX = -1;
    public static float BoundsMinY = -1;
    public static float BoundsMaxY = -1;

    //how many pixels is the edge of the window window allowed to be from the edge of the screen (how close to being completely out of bounds)
    float Padding = 40;
    void Update()
    {
        Vector3 BotLeftCorner = UniversalReference.MainCamera.ScreenToWorldPoint(new Vector3(Padding, Padding));
        Vector3 TopRightCorner = UniversalReference.MainCamera.ScreenToWorldPoint(new Vector3(Screen.width - Padding, Screen.height - Padding));
        BoundsMinX = BotLeftCorner.x;
        BoundsMaxX = TopRightCorner.x;
        BoundsMinY = BotLeftCorner.y;
        BoundsMaxY = TopRightCorner.y;
    }

    public static Vector3 RequestCorrection(Transform tr){
        float CorrectionX = 0;
        float CorrectionY = 0;

        float TrLeftmostXPoint = tr.position.x + tr.lossyScale.x / 2;
        float TrRightmostXPoint = tr.position.x - tr.lossyScale.x / 2;
        float TrLowermostXPoint = tr.position.y + tr.lossyScale.y / 2;
        float TrUppermostXPoint = tr.position.y - tr.lossyScale.y / 2;

        if(TrLeftmostXPoint < BoundsMinX){
            CorrectionX = BoundsMinX - TrLeftmostXPoint;
        }
        else if(TrRightmostXPoint > BoundsMaxX){
            CorrectionX = BoundsMaxX - TrRightmostXPoint;
        }
        if (TrLowermostXPoint < BoundsMinY)
        {
            CorrectionY = BoundsMinY - TrLowermostXPoint;
        }
        else if (TrUppermostXPoint > BoundsMaxY)
        {
            CorrectionY = BoundsMaxY - TrUppermostXPoint;
        }
        
        return new Vector3(CorrectionX, CorrectionY, 0);


    }

}
