using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCrosshair : MonoBehaviour
{
    
    Camera c;

    void Start()
    {
        c = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = c.ScreenToWorldPoint(Input.mousePosition);

        transform.position = new Vector3(pos.x,pos.y,ZIndexManager.Const_GUI);

    }
}
