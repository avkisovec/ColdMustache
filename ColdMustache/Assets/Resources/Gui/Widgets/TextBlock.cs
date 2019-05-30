using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBlock : MonoBehaviour
{
    
    /*
    
        just a simple container object with sprite renderers inside
    
    
    
     */



    void Start()
    {
        
    }
/*
    // Update is called once per frame
    void Update()
    {
        
    }
    */

    public void ini(int Width, int Height, string Text, float LetterWidth = 7f/32f, float LetterHeight = 11f/32f){


        Color Invisible = new Color(1f, 0.2f, 0.2f, 0f);
        Color Dark = new Color(1f, 0.2f, 0.2f, 0.4f);
        Color Edge = new Color(1, 0.2f, 0.2f, 1);
        Color Final = new Color(0.9f, 0.9f, 0.9f, 1);
        
        float X = 0;
        float Y = 0;

        for (int i = 0; i < Text.Length; i++)
        {
            if (Text[i] == '\n')
            {
                X = 0;
                Y -= (float)11 / (float)32;
                continue;
            }
            GameObject go = new GameObject();
            go.transform.parent = transform;
            go.transform.localPosition = new Vector3(X, Y, 0);
            go.AddComponent<SpriteRenderer>().sprite = AlphabetManager.GetSprite(Text[i]);

            go.GetComponent<SpriteRenderer>().color = Dark;
            TextEffect_ColorWave tecw = go.AddComponent<TextEffect_ColorWave>();
            tecw.Phases = 3;
            tecw.DelayedStart = new[] { (X - 3f * Y) / 600, 0f, 0f }; // /6 -> /600 to make it way faster for testing
            tecw.StartingColor = new[] { Invisible, Dark, Edge };
            tecw.EndingColor = new[] { Dark, Edge, Final, };
            tecw.Lifespan = new[] { 0.6f, 0.4f, 0.2f, };

            X += (float)7 / (float)32;
        }



    }


}
