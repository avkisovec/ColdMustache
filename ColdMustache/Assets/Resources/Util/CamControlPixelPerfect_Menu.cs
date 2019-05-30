using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CamControlPixelPerfect_Menu : MonoBehaviour
{

    /*
    
    simplified version of camcontrol p p, this one doesnt move   
    would have called it "static" but in programming context that word has a slightly different meaning
    
     */

    Camera c;

    public float ActualTileScale = 1;

    public float PixelResolution = 32;
    

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
            string BaseOutputFileName = Application.dataPath + "/Screenshots/screenshot";
            int ToPreventOverwriting = 0;
            while (File.Exists(BaseOutputFileName + ToPreventOverwriting + ".png"))
            {
                ToPreventOverwriting++;
            }
            SaverLoader.CreateHardPathIfNeeded(BaseOutputFileName + ToPreventOverwriting + ".png");
            ScreenCapture.CaptureScreenshot(BaseOutputFileName + ToPreventOverwriting + ".png", 1);
        }


        c.orthographicSize = ((float)Screen.height / (PixelResolution * ActualTileScale)) * 0.5f;

    }
}