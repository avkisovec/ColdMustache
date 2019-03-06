using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;

public class RandomLoaderGenericEnvironment : MonoBehaviour
{
    /*
     *  this script combines the functions of SpriteManagerGeneric and RandomLoaderGeneric
     *
     *  it is intended for random loading of environment objects, that dont need update function
     *
     *
     *  EDIT: it doesnt really have any functionality from SpriteManagerGeneric, it is just a random loader that loads directly into sprite renderers
     *
     *
     */


    public SpriteRenderer[] SpriteRenderers;
    public bool LoadOnAwake = false;
    public string LoadOnStart_path = "yourPath/yourFile.preset"; //application.datapath is implied

    public SpriteRenderer PhSpriteRendererToRemoveOnStart = null;

    private void Awake()
    {
        if (LoadOnAwake)
        {
            Load(Application.dataPath + LoadOnStart_path);
            if(PhSpriteRendererToRemoveOnStart!=null) Destroy(PhSpriteRendererToRemoveOnStart);
        }
    }

    public void Load(string PresetPath)
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
            if (line.Length <= 2)
            {

            }
            // if line starts with "//", its a comment and will be ignored
            else if (line[0] == '/' && line[1] == '/')
            {

            }
            //now we are talking about a new slot - [0]...
            else if (line[0] == '[')
            {
                line = line.Trim('[');
                line = line.Trim(']');
                Pointer = int.Parse(line);
                if (AvailableSpritePaths.Count <= Pointer)
                {
                    AvailableSpritePaths.Add(new List<string>());
                    AvailableColors.Add(new List<Color>());
                }
            }
            //line is talking about a color
            else if (line[0] == '#')
            {
                string[] nums_string = line.Trim('#').Split(';');
                float[] nums = new float[nums_string.Length];
                for (int i = 0; i < nums.Length; i++)
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
                string[] sprite_string = line.Split(';');
                int limit = int.Parse(sprite_string[1]);
                for(int i = 0; i < limit; i++){
                    AvailableSpritePaths[Pointer].Add(sprite_string[0]);
                }
            }


            line = sr.ReadLine();
        }
        sr.Close();

        for (int i = 0; i < SpriteRenderers.Length; i++)
        {
            SpriteRenderers[i].color = AvailableColors[i][Random.Range(
                (int)0,
                (int)AvailableColors[i].Count
                )];
        }

        for (int i = 0; i < SpriteRenderers.Length; i++)
        {
            SpriteRenderers[i].sprite = Resources.Load<Sprite>(AvailableSpritePaths[i][Random.Range(
                (int)0,
                (int)AvailableSpritePaths[i].Count
                )]);
        }
    }
}





