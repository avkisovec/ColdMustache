using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavTest : MonoBehaviour {

    int ImpassableTileValue = 999999;
    int EmptyTileValue = 888888;
    int[,] NavArray;

    WaypointNavigator wn;

    GameObject enemy;
    GameObject target;

    // Use this for initialization
    void Start () {
        enemy = GameObject.Find("EnemyContainer");
        target = GameObject.Find("Target");

        wn = GetComponent<WaypointNavigator>();

        Texture2D map = Resources.Load<Texture2D>("NavTest/navmap");
        UnityEngine.Color[] all = map.GetPixels();
        NavArray = new int[map.width, map.height];
        for (int y = 0; y < map.height; y++)
        {
            for(int x = 0; x < map.width; x++)
            {
                if(all[y*map.width + x].r == 0)
                {
                    GameObject go = new GameObject();
                    go.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("pixel");
                    go.transform.position = new Vector3(x, y, 5);
                    NavArray[x, y] = ImpassableTileValue;
                }
                else
                {
                    NavArray[x, y] = EmptyTileValue;
                }
            }
        }
    }
	
    void FindAPath(int SourceX, int SourceY, int TargetX, int TargetY)
    {
        //Debug.Log(SourceX + ", " + TargetY);
        
        int[,] CurrNavArr = (int[,])NavArray.Clone();
        CurrNavArr[SourceX, SourceY] = 0;

        int CurrDistance = 0;
        List<Vector2> TilesToExamineNext = new List<Vector2>();
        List<Vector2> TilesCurrentlyBeingExamined = new List<Vector2>();

        TilesToExamineNext.Add(new Vector2(SourceX + 1, SourceY));
        TilesToExamineNext.Add(new Vector2(SourceX - 1, SourceY));
        TilesToExamineNext.Add(new Vector2(SourceX, SourceY + 1));
        TilesToExamineNext.Add(new Vector2(SourceX, SourceY - 1));

        bool Found = false;

        while (!Found)
        {
            CurrDistance++;

            TilesCurrentlyBeingExamined.Clear();
            foreach(Vector2 v in TilesToExamineNext)
            {
                TilesCurrentlyBeingExamined.Add(v);
            }
            TilesToExamineNext.Clear();

            if (CurrDistance > 100)
            {
                return;
            }

            foreach (Vector2 v in TilesCurrentlyBeingExamined)
            {
                if((int)v.x == TargetX && (int)v.y == TargetY)
                {
                    //found the target
                    Found = true;
                    break;
                }

                if(CurrNavArr[(int)v.x, (int)v.y] == ImpassableTileValue)
                {
                    // impassable tile - ignore
                }
                else
                {
                    if(CurrDistance < CurrNavArr[(int)v.x, (int)v.y])
                    {
                        //either found an empty tile, or a faster path to this tile
                        CurrNavArr[(int)v.x, (int)v.y] = CurrDistance;

                        TilesToExamineNext.Add(new Vector2(v.x + 1, v.y));
                        TilesToExamineNext.Add(new Vector2(v.x - 1, v.y));
                        TilesToExamineNext.Add(new Vector2(v.x, v.y + 1));
                        TilesToExamineNext.Add(new Vector2(v.x, v.y - 1));

                        /*
                        GameObject go = new GameObject();
                        go.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("pixel");
                        go.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.2f);
                        go.transform.position = new Vector3(v.x, v.y, -5);
                        go.name = CurrDistance.ToString();
                        */

                    }
                }
            }


        }

        List<Vector2> WayPoints = new List<Vector2>();

        Vector2 BackTracker = new Vector2(TargetX, TargetY);

        WayPoints.Add(new Vector2(BackTracker.x, BackTracker.y));

        int ToPreventInfiniteCycle = 100;

        while((BackTracker.x != SourceX || BackTracker.y != SourceY) && ToPreventInfiniteCycle > 0)
        {
            ToPreventInfiniteCycle--;
           
            if (CurrNavArr[(int)BackTracker.x+1, (int)BackTracker.y] < CurrNavArr[(int)BackTracker.x, (int)BackTracker.y])
            {
                BackTracker = new Vector2(BackTracker.x+1, BackTracker.y);
                WayPoints.Add(new Vector2(BackTracker.x, BackTracker.y));
                continue;
            }
            if (CurrNavArr[(int)BackTracker.x-1, (int)BackTracker.y] < CurrNavArr[(int)BackTracker.x, (int)BackTracker.y])
            {
                BackTracker = new Vector2(BackTracker.x-1, BackTracker.y);
                WayPoints.Add(new Vector2(BackTracker.x, BackTracker.y));
                continue;
            }
            if (CurrNavArr[(int)BackTracker.x, (int)BackTracker.y+1] < CurrNavArr[(int)BackTracker.x, (int)BackTracker.y])
            {
                BackTracker = new Vector2(BackTracker.x, BackTracker.y+1);
                WayPoints.Add(new Vector2(BackTracker.x, BackTracker.y));
                continue;
            }
            if (CurrNavArr[(int)BackTracker.x, (int)BackTracker.y-1] < CurrNavArr[(int)BackTracker.x, (int)BackTracker.y])
            {
                BackTracker = new Vector2(BackTracker.x, BackTracker.y-1);
                WayPoints.Add(new Vector2(BackTracker.x, BackTracker.y));
                continue;
            }
        }
        /*
        foreach(Vector2 v in WayPoints)
        {
            GameObject go = new GameObject();
            go.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("pixel");
            go.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.8f);
            go.transform.position = new Vector3(v.x, v.y, -5);
        }
        */
        wn.WayPoints = WayPoints;
        
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyUp(KeyCode.F))
        {
            FindAPath(Mathf.RoundToInt(enemy.transform.position.x),
                Mathf.RoundToInt(enemy.transform.position.y),
                Mathf.RoundToInt(target.transform.position.x),
                Mathf.RoundToInt(target.transform.position.y)
                );
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            FindAPath(Mathf.RoundToInt(enemy.transform.position.x),
                Mathf.RoundToInt(enemy.transform.position.y),
                Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x),
                Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y)
                );
        }
	}
}
