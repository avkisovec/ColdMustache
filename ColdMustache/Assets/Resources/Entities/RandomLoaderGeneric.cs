﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RandomLoaderGeneric : MonoBehaviour {

    public bool LoadOnStart = false;
    public SpriteManagerGeneric LoadOnStart_sm;
    public string LoadOnStart_path = "Assets/Resources/yourPath/yourFile.preset";
        
    private void Awake()
    {
        if (LoadOnStart)
        {
            Load(LoadOnStart_sm, LoadOnStart_path);
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load(LoadOnStart_sm, LoadOnStart_path);
        }
	}

    public static void Load(SpriteManagerGeneric spriteManager, string PresetPath)
    {
        List<List<Sprite[]>> AvailableSprites = new List<List<Sprite[]>>();
        List<List<Color>> AvailableColors = new List<List<Color>>();

        int Pointer = -1;

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
                if(AvailableSprites.Count <= Pointer)
                {
                    AvailableSprites.Add(new List<Sprite[]>());
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
                    nums[i] = float.Parse(nums_string[i]);
                }

                float r = nums[0] + Random.Range(-nums[4], nums[4]);
                float g = nums[1] + Random.Range(-nums[4], nums[4]);
                float b = nums[2] + Random.Range(-nums[4], nums[4]);

                AvailableColors[Pointer].Add(new Color(r, g, b, nums[3]));
            }
            //line is talking about a sprite
            else
            {
                AvailableSprites[Pointer].Add(Resources.LoadAll<Sprite>(line.Split(';')[0]));
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

        spriteManager.sprites = new Sprite[AvailableSprites.Count][];
        for (int i = 0; i < spriteManager.sprites.Length; i++)
        {
            spriteManager.sprites[i] = AvailableSprites[i][Random.Range(
                (int)0,
                (int)AvailableSprites[i].Count
                )];
        }

        spriteManager.UpdateEverything();

    }
}