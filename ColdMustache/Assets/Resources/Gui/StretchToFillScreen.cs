using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchToFillScreen : MonoBehaviour
{

    /*
    
    
    in unity, display coordinates follow the cartesian system

    [0,0] is the lower right
    
    
     */

    Camera c;
    CamControlPixelPerfect cc;

    // Start is called before the first frame update
    void Start()
    {
        c = Camera.main;
        cc = c.GetComponent<CamControlPixelPerfect>();
    }

    // Update is called once per frame
    void Update()
    {
       
        transform.position = c.ScreenToWorldPoint(new Vector3(0,0));
        transform.position = new Vector3(transform.position.x, transform.position.y, -50);

        transform.localScale = new Vector3(cc.WorldWidth(), cc.WorldHeight(), 1);

    }
}
