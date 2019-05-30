using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCurtain : MonoBehaviour
{


    float BlockCountX = 15;
    float BlockCountY = 15;

    SpriteRenderer[,] Blocks;

    float[,] Ages;
    float[,] Speeds;

    float BaseSpeed = 2;

    bool IsUpdateNeeded = true;

    void Start()
    {
        Blocks = new SpriteRenderer[(int)BlockCountX, (int)BlockCountY];
        Ages = new float[(int)BlockCountX, (int)BlockCountY];
        Speeds = new float[(int)BlockCountX, (int)BlockCountY];

        float BlockScaleX = 1 / BlockCountX;
        float BlockScaleY = 1 / BlockCountY;

        Sprite sprite = Resources.Load<Sprite>("CartesianPixel");

        for(int y = 0; y < BlockCountY; y++){
            for(int x = 0; x < BlockCountX; x++){
                
                GameObject go = new GameObject();
                go.transform.parent = transform;
                go.transform.localPosition = new Vector3(BlockScaleX*x, BlockScaleY*y,0);
                go.transform.localScale = new Vector3(BlockScaleX, BlockScaleY,1);

                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = sprite;
                Blocks[x,y] = sr;

                Ages[x,y] = 2;
                Speeds[x,y] = 0;

            }
        }


    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.B)){
            TurnBlack();
        }
        if(Input.GetKeyUp(KeyCode.N)){
            TurnTransparent();
        }
        

        if(!IsUpdateNeeded) return;

        Render();

        for (int y = 0; y < BlockCountY; y++)
        {
            for (int x = 0; x < BlockCountX; x++)
            {
                Ages[x, y] += Time.deltaTime * Speeds[x,y];
            }
        }
    }

    public void Render(){

        IsUpdateNeeded = false;
        
        for (int y = 0; y < BlockCountY; y++)
        {
            for (int x = 0; x < BlockCountX; x++)
            {

                float f = Ages[x,y];

                /*
                if (f < 0) { Blocks[x, y].color = Color.black; continue; }
                if (f > 1) { Blocks[x, y].color = new Color(0,0,0,0); continue; }
                Blocks[x, y].color = new Color(0, 0, 0, 1-f);
                continue;
                */

                //0.1764706,0.2666667,0.4431373   <- my dark blue im using as main color in menus

                if (Speeds[x, y] > 0 && f < 1) IsUpdateNeeded = true;
                if (Speeds[x, y] < 0 && f > 0) IsUpdateNeeded = true;

                if (f < 0) { Blocks[x, y].color = Color.black; continue; }
                if (f > 1) { Blocks[x, y].color = new Color(0.1764706f, 0.2666667f, 0.4431373f, 0); continue; }
                Blocks[x, y].color = new Color(0.1764706f*f, 0.2666667f*f, 0.4431373f*f, 1 - f);
                continue;

            }
        }

    }

    public void TurnBlack(){

        IsUpdateNeeded = true;

        for (int y = 0; y < BlockCountY; y++)
        {
            for (int x = 0; x < BlockCountX; x++)
            {
                Ages[x, y] = (x + (BlockCountY-y)) * 0.05f + Random.Range(1f, 1.3f);
                Speeds[x,y] = -BaseSpeed * Random.Range(0.75f,1.5f);

            }
        }

    }

    public void TurnTransparent(){

        IsUpdateNeeded = true;

        for (int y = 0; y < BlockCountY; y++)
        {
            for (int x = 0; x < BlockCountX; x++)
            {
                Ages[x, y] = (x+(BlockCountY-y))*-0.05f + Random.Range(-0.3f, 0f);
                Speeds[x, y] = BaseSpeed * Random.Range(0.75f, 1.5f);

            }
        }

    }

}
