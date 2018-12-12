using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour {

    public Transform Player;
    Vector2 MousePos;

    public bool PixelPerfect;

    public int PixelSize = 1;
    float ActualPixelSize = 1;

    public float PixelResolution = 32;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ScreenCapture.CaptureScreenshot("screen.png", 4);
        }

        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 Goal = (Vector2)Player.position + (MousePos - (Vector2)Player.position) / 2;

        if((Goal - (Vector2)transform.position).magnitude > 0.1)
        {
            transform.position += ((Vector3)Goal - transform.position) / 10;
        }
        
        transform.position = new Vector3(transform.position.x, transform.position.y, -50);

        if(Input.mouseScrollDelta.y!= 0)
        {
            float factor = 1 - Input.mouseScrollDelta.y/10;

            PixelSize += (int)Input.mouseScrollDelta.y;
            if(PixelSize > 0)
            {
                ActualPixelSize = PixelSize;

                if(PixelSize > 15)
                {
                    PixelSize = 15;
                }

            }
            else
            {
                switch (PixelSize)
                {
                    case 0:
                        ActualPixelSize = 0.5f;
                        break;
                    case -1:
                        ActualPixelSize = 0.25f;
                        break;
                    default:
                        ActualPixelSize = 0.125f;
                        PixelSize = -2;
                        break;
                }
            }

            if (GetComponent<Camera>().orthographicSize*factor > 2 && GetComponent<Camera>().orthographicSize*factor < 30)
            {
                GetComponent<Camera>().orthographicSize *= factor;
            }
        }

        if (PixelPerfect)
        {
            //Orthographic size = ((Vert Resolution)/(PPUScale * PPU)) * 0.5
            //GetComponent<Camera>().orthographicSize = ((float)Screen.height / 32f) / PixelSize;
            GetComponent<Camera>().orthographicSize = ((float)Screen.height / (PixelResolution * ActualPixelSize)) * 0.5f;

            transform.position = (Vector3)Goal;
            transform.position = new Vector3(transform.position.x, transform.position.y, -50);


        }
        
        
	}
    
}
