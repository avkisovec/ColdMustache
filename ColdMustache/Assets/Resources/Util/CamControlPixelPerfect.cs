using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CamControlPixelPerfect : MonoBehaviour {

    public Transform Player;
    Vector2 MousePos;

    Camera c;

    public bool FixShake = true;
    
    public int DesiredTileScale = 1;
    public float ActualTileScale = 1;

    public float PixelResolution = 32;

    public float CurrFreecamFactor = 1; //1 is default = both player and cursor on screen; higher numbers can look farther where player is not on screen anymore
    public float UnzoomedFreecamFactor = 1;
    public float ZoomedInFreecamFactor = 3;
    public float DefaultZoomedInFreecamFactor = 3;

    public float MouseScreenX = 0;
    public float MouseScreenY = 0;


    // Use this for initialization
    void Start()
    {
        c = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F2))
        {
            string BaseOutputFileName = Application.dataPath+"/Screenshots/screenshot";
            int ToPreventOverwriting = 0;
            while (File.Exists(BaseOutputFileName + ToPreventOverwriting + ".png"))
            {
                ToPreventOverwriting++;
            }
            SaverLoader.CreateHardPathIfNeeded(BaseOutputFileName + ToPreventOverwriting + ".png");
            ScreenCapture.CaptureScreenshot(BaseOutputFileName + ToPreventOverwriting + ".png", 1);
        }


        MouseScreenX = Input.mousePosition.x;
        MouseScreenY = Input.mousePosition.y;

        if(MouseScreenX < 0)
        {
            MouseScreenX = 0;
        }
        else if(MouseScreenX > c.pixelWidth)
        {
            MouseScreenX = c.pixelWidth;
        }
        if (MouseScreenY < 0)
        {
            MouseScreenY = 0;
        }
        else if (MouseScreenY > c.pixelHeight)
        {
            MouseScreenY = c.pixelHeight;
        }
        Vector2 MouseScreenPos = new Vector2(MouseScreenX, MouseScreenY);
        
        MousePos = UniversalReference.MouseWorldPos;

        /*/
        if (MouseInterceptor.IsMouseBeingIntercepted())
        {
            return;
        }*/

        if (Input.GetKey(KeyCode.Z))
        {
            CurrFreecamFactor = ZoomedInFreecamFactor;
        }
        else
        {
            CurrFreecamFactor = UnzoomedFreecamFactor;
        }

        if (FixShake)
        {
            transform.position = new Vector3(
            Player.position.x + (WorldWidth() / 2 * MouseScreenXRatioRelativeToCenter() * CurrFreecamFactor),
            Player.position.y + (WorldHeight() / 2 * MouseScreenYRatioRelativeToCenter() * CurrFreecamFactor),
            -70);
        }
        else
        {
            Vector2 Goal = (Vector2)Player.position + (MousePos - (Vector2)Player.position) / 2;
            transform.position = new Vector3(Goal.x, Goal.y, -70);
        }

        

        if (Input.mouseScrollDelta.y != 0 && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            DesiredTileScale += (int)Input.mouseScrollDelta.y;

            if (DesiredTileScale > 8)
            {
                DesiredTileScale = 8;
            }
            else if(DesiredTileScale < 0) //original limit was -3, but that was too much
            {
                DesiredTileScale = 0;
            }

            if (DesiredTileScale > 0)
            {
                ActualTileScale = DesiredTileScale;
            }
            else
            {
                ActualTileScale = 1f / (-(DesiredTileScale-2)); // -2 so that 0 (invalid) and -1 (same as 1) get skipped
            }
            
        }
        
        c.orthographicSize = ((float)Screen.height / (PixelResolution * ActualTileScale)) * 0.5f;
            
    }

    //gives 0 in center, -1 on left edge, +1 on right edge (and anything inbetween linearly)
    float MouseScreenXRatioRelativeToCenter()
    {
        return (2 * MouseScreenX / Screen.width - 1);
    }
    
    //gives 0 in center, +1 on top edge, -1 on bottom edge (and anything inbetween linearly)
    float MouseScreenYRatioRelativeToCenter()
    {
        return (2 * MouseScreenY / Screen.height - 1);
    }

    //return how much of the world camera sees, in unity units (therefore in tiles)
    float WorldHeight()
    {
        return (float)Screen.height / PixelResolution / ActualTileScale;
    }
    float WorldWidth()
    {
        return (float)Screen.width / PixelResolution / ActualTileScale;
    }

}
