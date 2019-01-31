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

    //if true, it will mark the area in update (so as to not interfere with line drawer)
    public bool OnlyForNPCs = false;

    // Use this for initialization
    void Start()
    {
        if (!OnlyForNPCs)
        {
            DoYourThing();
        }
    }

    private void Update()
    {
        DoYourThing();
    }

    void DoYourThing()
    {
        int minX = Mathf.RoundToInt(transform.position.x - Mathf.Abs(transform.lossyScale.x) / 2);
        int maxX = Mathf.RoundToInt(transform.position.x + Mathf.Abs(transform.lossyScale.x) / 2);
        int minY = Mathf.RoundToInt(transform.position.y - Mathf.Abs(transform.lossyScale.y) / 2);
        int maxY = Mathf.RoundToInt(transform.position.y + Mathf.Abs(transform.lossyScale.y) / 2);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                NavTestStatic.NavArray[x, y] = NavTestStatic.ImpassableTileValue;
            }
        }

        Destroy(GetComponent<ReportUnwalkableArea>());
    }


}
