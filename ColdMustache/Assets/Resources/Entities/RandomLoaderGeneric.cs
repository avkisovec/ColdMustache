using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;

public class RandomLoaderGeneric : MonoBehaviour {

    public bool LoadOnStart = false;
    public SpriteManagerGeneric LoadOnStart_sm;
    public string LoadOnStart_path = "yourPath/yourFile.preset"; //application.datapath is implied
        
    private void Awake()
    {
        if (LoadOnStart)
        {
            Load(LoadOnStart_sm, Application.dataPath + LoadOnStart_path);
        }
    }
    
    public void RerollRandomly()
    {
        Load(LoadOnStart_sm, Application.dataPath + LoadOnStart_path);
    }

    public static void Load(SpriteManagerGeneric spriteManager, string PresetPath)
    {
        List<List<string>> AvailableSpritePaths = new List<List<string>>();
        List<List<Color>> AvailableColors = new List<List<Color>>();

        int Pointer = -1;

        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

        StreamReader sr = new StreamReader(PresetPath);
        string line = sr.ReadLine();
        while (line != null)
        {
            //lines with no or one character will be ignored
            if(line.Length <= 2)
            {

            }
            // if line starts with "//", its a comment and will be ignored
            else if(line[0] == '/' && line[1] == '/')
            {

            }
            //now we are talking about a new slot - [0]...
            else if (line[0] == '[')
            {
                line = line.Trim('[');
                line = line.Trim(']');
                Pointer = int.Parse(line);
                if(AvailableSpritePaths.Count <= Pointer)
                {
                    AvailableSpritePaths.Add(new List<string>());
                    AvailableColors.Add(new List<Color>());
                }
            }
            //line is talking about a color
            else if(line[0] == '#')
            {
                string[] nums_string = line.Trim('#').Split(';');
                float[] nums = new float[nums_string.Length];
                for(int i = 0; i < nums.Length; i++)
                {
                    nums[i] = float.Parse(nums_string[i], nfi);
                }

                float r = nums[0] + Random.Range(-nums[4], nums[4]);
                float g = nums[1] + Random.Range(-nums[4], nums[4]);
                float b = nums[2] + Random.Range(-nums[4], nums[4]);

                AvailableColors[Pointer].Add(new Color(r, g, b, nums[3]));
            }
            //line is talking about a sprite
            else
            {
                AvailableSpritePaths[Pointer].Add(line.Split(';')[0]);
            }


            line = sr.ReadLine();
        }
        sr.Close();
        
        spriteManager.colors = new Color[AvailableColors.Count];
        for(int i = 0; i < spriteManager.colors.Length; i++)
        {
            spriteManager.colors[i] = AvailableColors[i][Random.Range(
                (int)0,
                (int)AvailableColors[i].Count
                )];
        }

        spriteManager.sprites = new Sprite[AvailableSpritePaths.Count][];
        spriteManager.spritePaths = new string[AvailableSpritePaths.Count];
        for (int i = 0; i < spriteManager.sprites.Length; i++)
        {
            spriteManager.spritePaths[i] = AvailableSpritePaths[i][Random.Range(
                (int)0,
                (int)AvailableSpritePaths[i].Count
                )];
        }

        spriteManager.LoadSpritesFromPaths();

        spriteManager.UpdateEverything();

    }
}
