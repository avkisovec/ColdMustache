using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NavTestStatic : MonoBehaviour {

    public static int ImpassableTileValue = 999999;
    public static int EmptyTileValue = 888888;

    public static int MapWidth = 130;
    public static int MapHeight = 130;

    public static int[,] NavArray;
    public static int[,] ExplosionNavArray;
    public static int[,] LightNavArray;

    public static void BuildMapFromImage()
    {
        Texture2D map = Resources.Load<Texture2D>("NavTest/navmap");
        UnityEngine.Color[] all = map.GetPixels();
        NavArray = new int[map.width, map.height];
        for (int y = 0; y < map.height; y++)
        {
            for (int x = 0; x < map.width; x++)
            {
                if (all[y * map.width + x].r == 0)
                {
                    NavArray[x, y] = ImpassableTileValue;
                }
                else
                {
                    NavArray[x, y] = EmptyTileValue;
                }
            }
        }

        for (int y = 0; y < map.height; y++)
        {
            for (int x = 0; x < map.width; x++)
            {
                if (NavArray[x, y] == ImpassableTileValue)
                {
                    bool ConnectedRight = false;
                    bool ConnectedUp = false;
                    bool ConnectedLeft = false;
                    bool ConnectedDown = false;

                    if (x < map.width - 1 && NavArray[x + 1, y] == ImpassableTileValue)
                    {
                        ConnectedRight = true;
                    }
                    if (y < map.height - 1 && NavArray[x, y + 1] == ImpassableTileValue)
                    {
                        ConnectedUp = true;
                    }
                    if (x >= 1 && NavArray[x - 1, y] == ImpassableTileValue)
                    {
                        ConnectedLeft = true;
                    }
                    if (y >= 1 && NavArray[x, y - 1] == ImpassableTileValue)
                    {
                        ConnectedDown = true;
                    }

                    GameObject go = new GameObject();
                    go.AddComponent<SpriteRenderer>().sprite = WallSpriteFinder.Find(
                            Resources.LoadAll<Sprite>("Navtest/WallSheetTransparent"),
                            ConnectedRight, ConnectedUp, ConnectedLeft, ConnectedDown);
                    go.transform.position = new Vector3(x, y, -5);
                    go.AddComponent<EnvironmentObject>();
                    go.AddComponent<BoxCollider2D>();

                    GameObject concrete = new GameObject();
                    concrete.transform.parent = go.transform;
                    concrete.transform.localPosition = new Vector3(0, 0, 1);
                    concrete.AddComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("NavTest/ConcreteDull320")[y % 10 * 10 + x % 10];
                }
            }
        }
    }

    public static bool IsTileEmpty(int X, int Y)
    {
        if (NavArray[X, Y] == EmptyTileValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static List<Vector2> FindAPath(int SourceX, int SourceY, int TargetX, int TargetY)
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
            foreach (Vector2 v in TilesToExamineNext)
            {
                TilesCurrentlyBeingExamined.Add(v);
            }
            TilesToExamineNext.Clear();

            if (CurrDistance > 100)
            {
                return null;
            }

            foreach (Vector2 v in TilesCurrentlyBeingExamined)
            {
                if ((int)v.x == TargetX && (int)v.y == TargetY)
                {
                    //found the target
                    Found = true;
                    break;
                }

                if (CurrNavArr[(int)v.x, (int)v.y] == ImpassableTileValue)
                {
                    // impassable tile - ignore
                }
                else
                {
                    if (CurrDistance < CurrNavArr[(int)v.x, (int)v.y])
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

        while ((BackTracker.x != SourceX || BackTracker.y != SourceY) && ToPreventInfiniteCycle > 0)
        {
            ToPreventInfiniteCycle--;

            if (CurrNavArr[(int)BackTracker.x + 1, (int)BackTracker.y] < CurrNavArr[(int)BackTracker.x, (int)BackTracker.y])
            {
                BackTracker = new Vector2(BackTracker.x + 1, BackTracker.y);
                WayPoints.Add(new Vector2(BackTracker.x, BackTracker.y));
                continue;
            }
            if (CurrNavArr[(int)BackTracker.x - 1, (int)BackTracker.y] < CurrNavArr[(int)BackTracker.x, (int)BackTracker.y])
            {
                BackTracker = new Vector2(BackTracker.x - 1, BackTracker.y);
                WayPoints.Add(new Vector2(BackTracker.x, BackTracker.y));
                continue;
            }
            if (CurrNavArr[(int)BackTracker.x, (int)BackTracker.y + 1] < CurrNavArr[(int)BackTracker.x, (int)BackTracker.y])
            {
                BackTracker = new Vector2(BackTracker.x, BackTracker.y + 1);
                WayPoints.Add(new Vector2(BackTracker.x, BackTracker.y));
                continue;
            }
            if (CurrNavArr[(int)BackTracker.x, (int)BackTracker.y - 1] < CurrNavArr[(int)BackTracker.x, (int)BackTracker.y])
            {
                BackTracker = new Vector2(BackTracker.x, BackTracker.y - 1);
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

        return WayPoints;
        //wn.WayPoints = WayPoints;

    }
    
    public static List<Vector2> FindAPath(Vector2Int Source, Vector2Int Target)
    {
        return FindAPath(Source.x, Source.y, Target.x, Target.y);
    }

    public static List<Vector3> CalculateExplosion_DistributorNoBacksiesTileSplit(int SourceX, int SourceY, int ExpandForce)
    {

        /*
         * 
         * splits pressure between tile and new distributors, instead of just giving all to tile and also among distributors
         * 
         */

        int Right = 0;
        int Up = 90;
        int Left = 180;
        int Down = 270;


        int[,] CurrNavArr = (int[,])ExplosionNavArray.Clone();

        //int CurrDistance = 0;
        List<Vector4> TilesToExamineNext = new List<Vector4>();
        List<Vector4> TilesCurrentlyBeingExamined = new List<Vector4>();

        List<Vector2Int> OutputCoordinates = new List<Vector2Int>(); //only x and y
        List<Vector3> Output = new List<Vector3>(); // x and y are coordinates of explosion, z is expandforce

        TilesToExamineNext.Add(new Vector4(SourceX, SourceY, ExpandForce, -1));
        //CurrNavArr[SourceX, SourceY] = ExpandForce;

        int AvoidInfiniteCycle = 100;

        OutputCoordinates.Add(new Vector2Int(SourceX, SourceY));

        while (TilesToExamineNext.Count != 0 && AvoidInfiniteCycle > 0)
        {
            AvoidInfiniteCycle--;

            TilesCurrentlyBeingExamined.Clear();
            foreach (Vector4 v in TilesToExamineNext)
            {
                TilesCurrentlyBeingExamined.Add(v);
            }
            TilesToExamineNext.Clear();

            if (ExpandForce < 1)
            {
                break;
            }

            foreach (Vector4 v in TilesCurrentlyBeingExamined)
            {
                if (CurrNavArr[(int)v.x, (int)v.y] == EmptyTileValue)
                {
                    CurrNavArr[(int)v.x, (int)v.y] = 0;
                }


                int ExpandableDirections = 0;
                if (CurrNavArr[(int)v.x + 1, (int)v.y] != ImpassableTileValue && v.w != Right)
                {
                    ExpandableDirections++;
                }
                if (CurrNavArr[(int)v.x - 1, (int)v.y] != ImpassableTileValue && v.w != Left)
                {
                    ExpandableDirections++;
                }
                if (CurrNavArr[(int)v.x, (int)v.y + 1] != ImpassableTileValue && v.w != Up)
                {
                    ExpandableDirections++;
                }
                if (CurrNavArr[(int)v.x, (int)v.y - 1] != ImpassableTileValue && v.w != Down)
                {
                    ExpandableDirections++;
                }

                ExpandableDirections++; //also splits with ground

                int NewDistributorPressure = Mathf.FloorToInt(((float)v.z) / (float)ExpandableDirections);

                if (NewDistributorPressure > 0)
                {

                    CurrNavArr[(int)v.x, (int)v.y] += NewDistributorPressure;

                    if (ExpandableDirections > 1)
                    {

                        if (CurrNavArr[(int)v.x + 1, (int)v.y] != ImpassableTileValue && v.w != Left)
                        {
                            TilesToExamineNext.Add(new Vector4(v.x + 1, v.y, NewDistributorPressure, Right));
                            if (!OutputCoordinates.Contains(new Vector2Int((int)v.x + 1, (int)v.y)) && NewDistributorPressure > 0)
                            {
                                OutputCoordinates.Add(new Vector2Int((int)v.x + 1, (int)v.y));
                            }
                        }
                        if (CurrNavArr[(int)v.x - 1, (int)v.y] != ImpassableTileValue && v.w != Right)
                        {
                            TilesToExamineNext.Add(new Vector4(v.x - 1, v.y, NewDistributorPressure, Left));
                            if (!OutputCoordinates.Contains(new Vector2Int((int)v.x - 1, (int)v.y)) && NewDistributorPressure > 0)
                            {
                                OutputCoordinates.Add(new Vector2Int((int)v.x - 1, (int)v.y));
                            }
                        }
                        if (CurrNavArr[(int)v.x, (int)v.y + 1] != ImpassableTileValue && v.w != Down)
                        {
                            TilesToExamineNext.Add(new Vector4(v.x, v.y + 1, NewDistributorPressure, Up));
                            if (!OutputCoordinates.Contains(new Vector2Int((int)v.x, (int)v.y + 1)) && NewDistributorPressure > 0)
                            {
                                OutputCoordinates.Add(new Vector2Int((int)v.x, (int)v.y + 1));
                            }
                        }
                        if (CurrNavArr[(int)v.x, (int)v.y - 1] != ImpassableTileValue && v.w != Up)
                        {
                            TilesToExamineNext.Add(new Vector4(v.x, v.y - 1, NewDistributorPressure, Down));
                            if (!OutputCoordinates.Contains(new Vector2Int((int)v.x, (int)v.y - 1)) && NewDistributorPressure > 0)
                            {
                                OutputCoordinates.Add(new Vector2Int((int)v.x, (int)v.y - 1));
                            }
                        }

                        /*
                        if (CurrNavArr[(int)v.x + 1, (int)v.y] == EmptyTileValue)
                        {
                            CurrNavArr[(int)v.x + 1, (int)v.y] = CurrNavArr[(int)v.x, (int)v.y]- 1;
                            TilesToExamineNext.Add(new Vector2(v.x + 1, v.y));
                            OutputCoordinates.Add(new Vector2(v.x + 1, v.y));
                        }
                        if (CurrNavArr[(int)v.x - 1, (int)v.y] == EmptyTileValue)
                        {
                            CurrNavArr[(int)v.x - 1, (int)v.y] = CurrNavArr[(int)v.x, (int)v.y] - 1;
                            TilesToExamineNext.Add(new Vector2(v.x - 1, v.y));
                            OutputCoordinates.Add(new Vector2(v.x - 1, v.y));
                        }
                        if (CurrNavArr[(int)v.x, (int)v.y + 1] == EmptyTileValue)
                        {
                            CurrNavArr[(int)v.x, (int)v.y + 1] = CurrNavArr[(int)v.x, (int)v.y] - 1;
                            TilesToExamineNext.Add(new Vector2(v.x, v.y + 1));
                            OutputCoordinates.Add(new Vector2(v.x, v.y + 1));
                        }
                        if (CurrNavArr[(int)v.x, (int)v.y - 1] == EmptyTileValue)
                        {
                            CurrNavArr[(int)v.x, (int)v.y - 1] = CurrNavArr[(int)v.x, (int)v.y] - 1;
                            TilesToExamineNext.Add(new Vector2(v.x, v.y - 1));
                            OutputCoordinates.Add(new Vector2(v.x, v.y - 1));
                        }
                        */

                    }


                }
            }


        }

        foreach (Vector2Int v in OutputCoordinates)
        {
            if (CurrNavArr[(int)v.x, (int)v.y] != 0)
            {
                Output.Add(new Vector3(v.x, v.y, CurrNavArr[(int)v.x, (int)v.y]));
            }
        }

        return Output;
    }
    
    public static void ExportNavMap()
    {

        SaverLoader.CreateHardPathIfNeeded(Application.dataPath + "/debug/navMapOutput.txt");

        StreamWriter sw = new StreamWriter(Application.dataPath + "/debug/navMapOutput.txt");
        sw.AutoFlush = true;

        for(int y = MapHeight-1; y >= 0; y--)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                if(NavArray[x,y] == ImpassableTileValue)
                {
                    sw.Write("#");
                }
                else
                {
                    sw.Write(".");
                }
            }
            sw.WriteLine();
        }

    }
    
    public static bool CanLightPassThroughTile(Vector2Int tile)
    {
        if (LightNavArray[tile.x, tile.y] == EmptyTileValue)
        {
            return true;
        }
        return false;
    }

    public static bool CheckLineOfSight(Vector2Int Origin, Vector2 Target)
    {
        foreach(Vector2Int v in BresenhamLine(Origin, Target)){
            if (!CanLightPassThroughTile(v))
            {
                return false;
            }
        }
        return true;
    }
        
    /*
     * The following code is adapted version of code from:
     * 
     * https://rosettacode.org/wiki/Bitmap/Bresenham%27s_line_algorithm#C.23
     * 
     * 
     */
    static List<Vector2Int> BresenhamLine(Vector2 Origin, Vector2 Target)
    {
        int x0 = Mathf.RoundToInt(Origin.x);
        int y0 = Mathf.RoundToInt(Origin.y);
        int x1 = Mathf.RoundToInt(Target.x);
        int y1 = Mathf.RoundToInt(Target.y);
        List<Vector2Int> output = new List<Vector2Int>();
        int dx = Mathf.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = Mathf.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = (dx > dy ? dx : -dy) / 2, e2;
        for (;;)
        {
            output.Add(new Vector2Int(x0, y0));
            if (x0 == x1 && y0 == y1) break;
            e2 = err;
            if (e2 > -dx) { err -= dy; x0 += sx; }
            if (e2 < dy) { err += dx; y0 += sy; }
        }
        return output;
    }
    



}
