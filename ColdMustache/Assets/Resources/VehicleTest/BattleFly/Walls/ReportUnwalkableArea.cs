using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportUnwalkableArea : MonoBehaviour {

    /*
     * 
     * this script should only be used with objects with default scale of 1x1 units
     * (scale % has to match unity units)
     * 
     * if any part is outside the navmap bounds it will crash
     * 
     * 
     */

    // Use this for initialization
    void Start()
    {

        int minX = Mathf.RoundToInt(transform.position.x - transform.lossyScale.x / 2);
        int maxX = Mathf.RoundToInt(transform.position.x + transform.lossyScale.x / 2);
        int minY = Mathf.RoundToInt(transform.position.y - transform.lossyScale.y / 2);
        int maxY = Mathf.RoundToInt(transform.position.y + transform.lossyScale.y / 2);

        for(int x = minX; x < maxX; x++)
        {
            for(int y = minY; y < maxY; y++)
            {
                NavTestStatic.NavArray[x, y] = NavTestStatic.ImpassableTileValue;
            }
        }



    }
       
}
