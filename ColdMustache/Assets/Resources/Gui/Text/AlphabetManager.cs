using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphabetManager : MonoBehaviour {

    /*
     * based on standard ASCII table (printable characters only)
     * 
     * everything from:
     * (dec) 32 - " ", space (here indexed as [0])
     * to:
     * (dec) 126 - "~", tilde (here indexed as [94])
     * 
     * 
     */

    public static Sprite[] Alphabet;
    public string AlphabetPath;

    // Use this for initialization
    static bool HasBeenSetup = false;
	void Start () {
        if(HasBeenSetup)
        {
            return;
        }
        HasBeenSetup = true;

        Alphabet = Resources.LoadAll<Sprite>(AlphabetPath);
	}
	
    public static Sprite GetSprite(char c)
    {
        int CharValue = (int)c - 32;
        if(CharValue>=0 && CharValue <= 94)
        {
            return Alphabet[CharValue];
        }
        return null;
    }

    public static List<Sprite> GetSprites(string s)
    {
        List<Sprite> output = new List<Sprite>();
        foreach(char c in s)
        {
            output.Add(GetSprite(c));
        }
        return output;
    }

    public static string BreakText(string text, int MaxLineWidth)
    {
        string[] words = text.Split(' ');
        string output = "";

        int CurrLenght = 0;
        
        for(int i = 0; i < words.Length; i++)
        {

            //if next word is not longer than the entire line
            if(words[i].Length < MaxLineWidth)
            {
                //if its possible to fit the word onto current line (+1 to account for space)
                if(CurrLenght + 1 + words[i].Length <= MaxLineWidth)
                {
                    output += " " + words[i];
                    CurrLenght += 1 + words[i].Length;
                    continue;
                }
                //it wont fit, need a newline
                else
                {
                    output += "\n" + words[i];
                    CurrLenght = words[i].Length;
                    continue;
                }
            }

        }

        //inserting space before every word makes the first character be always a space,this solves it
        output = output.TrimStart(' ');

        return output;
    }

    public static void SpawnFloatingText(string text, Vector3 position)
    {
        GameObject container = new GameObject();
        container.transform.position = position;
        container.AddComponent<StayFixedSize>().PixelScale = 2;
        container.AddComponent<DieInSeconds>().Seconds = 1.5f;
        container.AddComponent<SlowlyMove>().PositionChange = new Vector3(0.03f, 0.03f);

        List<Sprite> sprites = GetSprites(text);
        for (int i = 0; i < sprites.Count; i++)
        {
            GameObject go = new GameObject();
            go.transform.parent = container.transform;
            go.transform.localPosition = new Vector3(((float)i * (float)7 / (float)32), 0, 0);
            go.AddComponent<SpriteRenderer>().sprite = sprites[i];
        }


    }
    
}
