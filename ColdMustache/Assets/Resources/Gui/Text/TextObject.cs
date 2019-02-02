using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextObject : MonoBehaviour
{

    [TextArea]
    public string DefaultText = "";

    public int LineCount = 1;
    public int LineLength = 20;
    
    SpriteRenderer[,] Letters;
    
    void Start()
    {
        if (DefaultText.Contains("\\n"))
        {
            DefaultText = DefaultText.Replace("\\n", "\n");
        }
        Letters = new SpriteRenderer[LineLength, LineCount];

        float OriginX = +7f / 32f / 2f;
        float OriginY = -11f / 32f / 2f;
        //OriginX = 0;
        //OriginY = 0;

        float XShift = (float)7 / (float)32;
        float YShift = (float)-11 / (float)32;

        Color Invisible = new Color(1f, 0.2f, 0.2f, 0f);
        Color Dark = new Color(1f, 0.2f, 0.2f, 0.4f);
        Color Edge = new Color(1, 0.2f, 0.2f, 1);
        Color Final = new Color(0.9f, 0.9f, 0.9f, 1);

        for (int indexY = 0; indexY < LineCount; indexY++)
        {
            for (int indexX = 0; indexX < LineLength; indexX++)
            {
                GameObject go = new GameObject();
                go.transform.parent = transform;
                go.transform.localPosition = new Vector3(OriginX + indexX * XShift, OriginY + indexY * YShift, -0.1f);
                go.transform.localScale = new Vector3(1, 1, 1);
                Letters[indexX, indexY] = go.AddComponent<SpriteRenderer>();
                Letters[indexX, indexY].sprite = AlphabetManager.GetSprite(' ');
            }
        }
        SetText(DefaultText);
    }

    string LastSetText = "no text was set so far";
    public void SetTextIfNeeded(string text)
    {
        if (text != LastSetText)
        {
            LastSetText = text;
            SetText(text);
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
            for (int x = 0; x < LineLength; x++)
            {
                if (y < FormattedText.Count && x < FormattedText[y].Length)
                {
                    Letters[x, y].sprite = AlphabetManager.GetSprite(FormattedText[y][x]);                    
                }
                else
                {
                    Letters[x, y].sprite = AlphabetManager.GetSprite(' ');
                }
            }
        }        
    }


    
}
