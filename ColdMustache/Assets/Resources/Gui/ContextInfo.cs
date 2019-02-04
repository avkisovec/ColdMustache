using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContextInfo : MonoBehaviour
{
    
    int LineCount = 20;
    int LineLength = 20;

    float UnitWidth = 7f / 32f * 20f;
    float BaseUnitHeight = 11f / 32f * 20f;
    float CurrUnitHeight = 11f / 32f * 20f;

    public Transform Container;

    public Transform Background;
    
    SpriteRenderer[,] Letters;

    public int FramesSinceContextMenuWasRequested = 0;

    void Start()
    {
        SelfLink = this;

        Letters = new SpriteRenderer[LineLength, LineCount];

        float OriginX = +7f/32f/2f;
        float OriginY = -11f/32f/2f;
        //OriginX = 0;
        //OriginY = 0;
        
        float XShift = (float)7 / (float)32;
        float YShift = (float)-11 / (float)32;
        
        Color Invisible = new Color(1f, 0.2f, 0.2f, 0f);
        Color Dark = new Color(1f, 0.2f, 0.2f, 0.4f);
        Color Edge = new Color(1, 0.2f, 0.2f, 1);
        Color Final = new Color(0.9f, 0.9f, 0.9f, 1);
        
        for(int indexY = 0; indexY < LineCount; indexY++)
        {
            for (int indexX = 0; indexX < LineLength; indexX++)
            {
                GameObject go = new GameObject();
                go.transform.parent = transform;
                go.transform.localPosition = new Vector3(OriginX + indexX*XShift, OriginY+indexY*YShift, -0.1f);
                Letters[indexX, indexY] = go.AddComponent<SpriteRenderer>();
                Letters[indexX, indexY].sprite = AlphabetManager.GetSprite('$');
            }
        }
                
    }

    string LastSetText = "no text was set so far";
    Vector2 LastMouseScreenPos = new Vector2(-1, -1);
    public void Request(string text)
    {
        FramesSinceContextMenuWasRequested = 0;
        if (text != LastSetText)
        {
            LastSetText = text;
            SetText(text);
        }
        if(Input.mousePosition.x != LastMouseScreenPos.x || Input.mousePosition.y != LastMouseScreenPos.y)
        {
            LastMouseScreenPos = (Vector2)Input.mousePosition;
            SetPosition();
        }
    }

    public void SetPosition()
    {
        //input.mouseposition follows the kartesian system - (0,0) is bottom left and (1920,1080) is top right (depending on resolution of course)

        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Container.position = new Vector3(position.x, position.y, -55);
        
        //mouse is in upper half
        if(Input.mousePosition.y > Screen.height / 2)
        {
            //mouse is left upper corner
            if(Input.mousePosition.x < Screen.width / 2)
            {
                transform.localPosition = new Vector3(0.5f, -0.5f);
            }
            //mouse if right upper corner
            else
            {
                transform.localPosition = new Vector3(-UnitWidth - 0.5f, -0.5f);
            }

        }
        //mouse is in lower half
        else
        {
            //mouse is left lower corner
            if (Input.mousePosition.x < Screen.width / 2)
            {
                transform.localPosition = new Vector3(0.5f, 0.5f + CurrUnitHeight);
            }
            //mouse if right lower corner
            else
            {
                transform.localPosition = new Vector3(-UnitWidth - 0.5f, 0.5f + CurrUnitHeight);
            }
        }



    }
    
    public void SetText(string text)
    {
        List<string> FormattedText = AlphabetManager.BreakText2_Array(text, LineLength);

        Color first = new Color(1f, 1f, 1, 1);
        Color second = new Color(1f, 1f, 0f, 1);
        Color Final = new Color(1, 1, 1, 1);

        for (int y = 0; y < LineCount; y++)
        {
            for(int x = 0; x < LineLength; x++)
            {
                if(y < FormattedText.Count && x < FormattedText[y].Length)
                {
                    Letters[x, y].sprite = AlphabetManager.GetSprite(FormattedText[y][x]);

                    //the text effect is very heavy on processing if player quickly changes what he's hovering over
                    //because it relies on large amout of AddComponent, Update and Destroy which are all heavy
                    
                    TextEffect_ColorWave tecw = Letters[x, y].gameObject.AddComponent<TextEffect_ColorWave>();
                    tecw.Phases = 2;
                    tecw.DelayedStart = new[] { (x + 3f * y) / 50, 0f};
                    tecw.StartingColor = new[] { first, second};
                    tecw.EndingColor = new[] { second, Final,};
                    tecw.Lifespan = new[] { 0.05f, 0.05f};                    
                }
                else
                {
                    Letters[x, y].sprite = AlphabetManager.GetSprite(' ');
                }
                
                
            }
        }

        CurrUnitHeight = 11f / 32f * (float)FormattedText.Count;
        Background.localScale = new Vector3(UnitWidth, CurrUnitHeight, 1);


    }

    // Update is called once per frame
    void Update()
    {
        if(FramesSinceContextMenuWasRequested >= 1)
        {
            Hide();
        }
        FramesSinceContextMenuWasRequested++;
    }

    Vector3 HidingPosition = new Vector3(-999, -999, -999);
    void Hide()
    {
        Container.position = HidingPosition;
    }

    static ContextInfo SelfLink;
    public static void RequestStatic(string text)
    {
        SelfLink.Request(text);
    }
}
