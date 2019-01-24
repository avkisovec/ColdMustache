using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayFixedSizeSpace : MonoBehaviour {

    Camera c;
    CamControlSpace cControl;

    float OrigRatio = 0;
    Vector3 OrigScale = new Vector3(1, 1, 1);

    public bool PixelPerfectScale = true;
    public float PixelScale = 1;

    // Use this for initialization
    void Start()
    {

        c = Camera.main;
        cControl = c.GetComponent<CamControlSpace>();

        OrigRatio = c.orthographicSize / transform.localScale.y;

    }

    // Update is called once per frame
    void Update()
    {
        if (PixelPerfectScale)
        {
            transform.localScale = OrigScale / cControl.ActualTileScale * PixelScale;
        }
        else
        {
            transform.localScale = OrigScale * (c.orthographicSize / OrigRatio);
        }
    }
}
