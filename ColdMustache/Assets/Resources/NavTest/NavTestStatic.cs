using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavTestStatic : MonoBehaviour {

    public static int ImpassableTileValue = 999999;
    public static int EmptyTileValue = 888888;

    public static int MapWidth = 100;
    public static int MapHeight = 100;

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

    /*
     * the concept of drawing a line
     * 
     * first, a virtual straight line between the points is created
     * 
     * then, you start with the pixel of origin
     * if its light passable, you move onto the next pixel, and repeat until you land on target pixel
     * 
     * you choose the next pixel in a direction that makes it closer to the virtual line
     * if you happen to land exactly on the line, choose a default option
     * 
     * 
     * line formula:
     * y = x * slope + shift
     * 
     * formula i will be using:
     * y = x * slope
     * slope = y/x
     * 
     * 
     */
    public static void DrawLine(Vector3 Origin3, Vector3 Target3)
    {
        Vector2Int Origin = Util.Vector3To2Int(Origin3);
        Vector2Int Target = Util.Vector3To2Int(Target3);

        Vector3 Delta3 = Target3 - Origin3;

        List<Vector2Int> points = new List<Vector2Int>();
        points.Add(Origin);

        Vector2Int DeltaInt = Target - Origin;
        float OriginalSlope = ((float)Delta3.y / ((float)Delta3.x+0.00001f));
        float CurrSlope = 0;

        Vector2Int CurrentTile = Origin;

        int Quadrant = -1;
        if(Delta3.x > 0)
        {
            if(Delta3.y >= 0)
            {
                Quadrant = 0;
                CurrSlope = 1;
                /*
                if(OriginalSlope > 1)
                {
                    CurrentTile += Vector2Int.up;
                }
                else
                {
                    CurrentTile += Vector2Int.right;
                }
                */
            }
            else
            {
                Quadrant = 3;
                CurrSlope = -1;
            }

        }
        else
        {
            if (Delta3.y >= 0)
            {
                Quadrant = 1;
                CurrSlope = -1;
            }
            else
            {
                Quadrant = 2;
                CurrSlope = 1;
            }
        }

        points.Add(CurrentTile);

        Debug.Log(OriginalSlope + " slope, Quadrant: "+ Quadrant);

        //CurrentTile += Vector2Int.up;

        int ToPreventInfiniteCycle = 0;
        while(CurrentTile != Target && ToPreventInfiniteCycle < 100)
        {
            ToPreventInfiniteCycle++;

            //first quadrant (0) - moving up and right
            if (Quadrant == 0)
            {
                Vector2Int CurrDelta = Target - CurrentTile;
                //to prevent dividing by zero
                if(CurrDelta.x == 0)
                {
                    CurrSlope = 999999999;
                }
                else
                {
                    CurrSlope = (float)CurrDelta.y / (float)CurrDelta.x;
                }
                //up
                if(CurrSlope > OriginalSlope)
                {
                    CurrentTile += Vector2Int.up;
                }
                //right
                else
                {
                    CurrentTile += Vector2Int.right;
                }
            }
            //second quadrant - moving up and left
            else if (Quadrant == 1)
            {
                
                Vector2Int CurrDelta = Target - CurrentTile;
                //to prevent dividing by zero
                if (CurrDelta.x == 0)
                {
                    CurrSlope = -999999999;
                }
                else
                {
                    CurrSlope = (float)CurrDelta.y / (float)CurrDelta.x;
                }
                //left
                if (CurrSlope > OriginalSlope || CurrSlope == 0)
                {
                    CurrentTile += Vector2Int.left;
                }
                //up
                else
                {
                    CurrentTile += Vector2Int.up;
                }
            }
            //third quadrant - moving down and left
            else if (Quadrant == 2)
            {
                Vector2Int CurrDelta = Target - CurrentTile;
                //to prevent dividing by zero
                if (CurrDelta.x == 0)
                {
                    CurrSlope = 999999999;
                }
                else
                {
                    CurrSlope = (float)CurrDelta.y / (float)CurrDelta.x;
                }
                //down
                if (CurrSlope > OriginalSlope)
                {
                    CurrentTile += Vector2Int.down;
                }
                //left
                else
                {
                    CurrentTile += Vector2Int.left;
                }
            }
            //fourth quadrant - moving down and right
            else if (Quadrant == 3)
            {
                Vector2Int CurrDelta = Target - CurrentTile;
                //to prevent dividing by zero
                if (CurrDelta.x == 0)
                {
                    CurrSlope = -999999999;
                }
                else
                {
                    CurrSlope = (float)CurrDelta.y / (float)CurrDelta.x;
                    //Debug.Log(CurrSlope);
                }
                //right
                if (CurrSlope > OriginalSlope || CurrSlope == 0)
                {
                    CurrentTile += Vector2Int.right;
                }
                //down
                else
                {
                    CurrentTile += Vector2Int.down;
                }
            }

            points.Add(CurrentTile);
        }

        foreach(Vector2Int v in points)
        {
            GameObject debug = new GameObject();
            debug.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("pixel");
            debug.transform.position = new Vector3(v.x, v.y, -20);
            debug.AddComponent<DieInSeconds>().Seconds = 2;
        }

    }

    public static void SightLine(Vector3 Origin3, Vector3 Target3)
    {
        Vector2Int Origin = Util.Vector3To2Int(Origin3);
        Vector2Int Target = Util.Vector3To2Int(Target3);

        Vector3 Delta3 = Target3 - Origin3;

        List<Vector2Int> points = new List<Vector2Int>();
        points.Add(Origin);

        Vector2Int DeltaInt = Target - Origin;
        float OriginalSlope = ((float)Delta3.y / ((float)Delta3.x + 0.00001f));
        float CurrSlope = 0;

        Vector2Int CurrentTile = Origin;

        int Quadrant = -1;
        if (Delta3.x > 0)
        {
            if (Delta3.y >= 0)
            {
                Quadrant = 0;
                CurrSlope = 1;
                /*
                if(OriginalSlope > 1)
                {
                    CurrentTile += Vector2Int.up;
                }
                else
                {
                    CurrentTile += Vector2Int.right;
                }
                */
            }
            else
            {
                Quadrant = 3;
                CurrSlope = -1;
            }

        }
        else
        {
            if (Delta3.y >= 0)
            {
                Quadrant = 1;
                CurrSlope = -1;
            }
            else
            {
                Quadrant = 2;
                CurrSlope = 1;
            }
        }

        points.Add(CurrentTile);

        Debug.Log(OriginalSlope + " slope, Quadrant: " + Quadrant);

        //CurrentTile += Vector2Int.up;

        int ToPreventInfiniteCycle = 0;
        while (CurrentTile != Target && ToPreventInfiniteCycle < 100)
        {
            ToPreventInfiniteCycle++;

            //first quadrant (0) - moving up and right
            if (Quadrant == 0)
            {
                Vector2Int CurrDelta = Target - CurrentTile;
                //to prevent dividing by zero
                if (CurrDelta.x == 0)
                {
                    CurrSlope = 999999999;
                }
                else
                {
                    CurrSlope = (float)CurrDelta.y / (float)CurrDelta.x;
                }
                //up
                if (CurrSlope > OriginalSlope)
                {
                    CurrentTile += Vector2Int.up;
                }
                //right
                else
                {
                    CurrentTile += Vector2Int.right;
                }
            }
            //second quadrant - moving up and left
            else if (Quadrant == 1)
            {

                Vector2Int CurrDelta = Target - CurrentTile;
                //to prevent dividing by zero
                if (CurrDelta.x == 0)
                {
                    CurrSlope = -999999999;
                }
                else
                {
                    CurrSlope = (float)CurrDelta.y / (float)CurrDelta.x;
                }
                //left
                if (CurrSlope > OriginalSlope || CurrSlope == 0)
                {
                    CurrentTile += Vector2Int.left;
                }
                //up
                else
                {
                    CurrentTile += Vector2Int.up;
                }
            }
            //third quadrant - moving down and left
            else if (Quadrant == 2)
            {
                Vector2Int CurrDelta = Target - CurrentTile;
                //to prevent dividing by zero
                if (CurrDelta.x == 0)
                {
                    CurrSlope = 999999999;
                }
                else
                {
                    CurrSlope = (float)CurrDelta.y / (float)CurrDelta.x;
                }
                //down
                if (CurrSlope > OriginalSlope)
                {
                    CurrentTile += Vector2Int.down;
                }
                //left
                else
                {
                    CurrentTile += Vector2Int.left;
                }
            }
            //fourth quadrant - moving down and right
            else if (Quadrant == 3)
            {
                Vector2Int CurrDelta = Target - CurrentTile;
                //to prevent dividing by zero
                if (CurrDelta.x == 0)
                {
                    CurrSlope = -999999999;
                }
                else
                {
                    CurrSlope = (float)CurrDelta.y / (float)CurrDelta.x;
                    //Debug.Log(CurrSlope);
                }
                //right
                if (CurrSlope > OriginalSlope || CurrSlope == 0)
                {
                    CurrentTile += Vector2Int.right;
                }
                //down
                else
                {
                    CurrentTile += Vector2Int.down;
                }
            }

            if(LightNavArray[CurrentTile.x,CurrentTile.y] == EmptyTileValue)
            {
                points.Add(CurrentTile);
            }
            else
            {
                foreach (Vector2Int v in points)
                {
                    GameObject debug = new GameObject();
                    debug.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("pixel");
                    debug.transform.position = new Vector3(v.x, v.y, -20);
                    debug.AddComponent<DieInSeconds>().Seconds = 2;
                }
                return;
            }
        }

        foreach (Vector2Int v in points)
        {
            GameObject debug = new GameObject();
            debug.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("pixel");
            debug.transform.position = new Vector3(v.x, v.y, -20);
            debug.AddComponent<DieInSeconds>().Seconds = 2;
        }
        return;

    }

    public static void FieldOfView(Vector3 origin)
    {
        int HaveVisionOf = -123456;

        Vector2Int Origin = Util.Vector3To2Int(origin);
        Vector2Int CurrTile = Origin;

        int[,] CurrLightArray = (int[,])LightNavArray.Clone();

        CurrLightArray[CurrTile.x, CurrTile.y] = HaveVisionOf;
        
        
        List<Vector2Int> TilesToExamineNext = new List<Vector2Int>();
        List<Vector2Int> TilesCurrentlyBeingExamined = new List<Vector2Int>();

        TilesToExamineNext.Add(new Vector2Int(CurrTile.x + 1, CurrTile.y));
        TilesToExamineNext.Add(new Vector2Int(CurrTile.x - 1, CurrTile.y));
        TilesToExamineNext.Add(new Vector2Int(CurrTile.x, CurrTile.y + 1));
        TilesToExamineNext.Add(new Vector2Int(CurrTile.x, CurrTile.y - 1));

        int ToPreventInfiniteCycle = 0;

        while (ToPreventInfiniteCycle < 100)
        {
            ToPreventInfiniteCycle++;
            

            TilesCurrentlyBeingExamined.Clear();
            foreach (Vector2Int v in TilesToExamineNext)
            {
                TilesCurrentlyBeingExamined.Add(v);
            }
            TilesToExamineNext.Clear();
            
            foreach (Vector2 v in TilesCurrentlyBeingExamined)
            {
                //projecting a sightline from current tile toward origin
                //if it ends up on impassable tile, stop
                //if it ends up on tile that already has line of sight, spread

                

                Vector2Int LineOrigin = CurrTile;
                Vector2Int LineTarget = Origin;
                                
                Vector2Int DeltaInt = LineTarget - LineOrigin;
                float OriginalSlope = ((float)DeltaInt.y / ((float)DeltaInt.x + 0.00001f));
                float CurrSlope = 0;

                Vector2Int LineCurrentTile = LineOrigin;

                int Quadrant = -1;
                if (DeltaInt.x > 0)
                {
                    if (DeltaInt.y >= 0)
                    {
                        Quadrant = 0;
                        CurrSlope = 1;
                    }
                    else
                    {
                        Quadrant = 3;
                        CurrSlope = -1;
                    }

                }
                else
                {
                    if (DeltaInt.y >= 0)
                    {
                        Quadrant = 1;
                        CurrSlope = -1;
                    }
                    else
                    {
                        Quadrant = 2;
                        CurrSlope = 1;
                    }
                }

                int Line_ToPreventInfiniteCycle = 0;
                
                while (Line_ToPreventInfiniteCycle < 100)
                {
                    Debug.Log(Line_ToPreventInfiniteCycle);

                    Line_ToPreventInfiniteCycle++;

                    //first quadrant (0) - moving up and right
                    if (Quadrant == 0)
                    {
                        Vector2Int CurrDelta = LineTarget - LineCurrentTile;
                        //to prevent dividing by zero
                        if (CurrDelta.x == 0)
                        {
                            CurrSlope = 999999999;
                        }
                        else
                        {
                            CurrSlope = (float)CurrDelta.y / (float)CurrDelta.x;
                        }
                        //up
                        if (CurrSlope > OriginalSlope)
                        {
                            LineCurrentTile += Vector2Int.up;
                        }
                        //right
                        else
                        {
                            LineCurrentTile += Vector2Int.right;
                        }
                    }
                    //second quadrant - moving up and left
                    else if (Quadrant == 1)
                    {

                        Vector2Int CurrDelta = LineTarget - LineCurrentTile;
                        //to prevent dividing by zero
                        if (CurrDelta.x == 0)
                        {
                            CurrSlope = -999999999;
                        }
                        else
                        {
                            CurrSlope = (float)CurrDelta.y / (float)CurrDelta.x;
                        }
                        //left
                        if (CurrSlope > OriginalSlope || CurrSlope == 0)
                        {
                            LineCurrentTile += Vector2Int.left;
                        }
                        //up
                        else
                        {
                            LineCurrentTile += Vector2Int.up;
                        }
                    }
                    //third quadrant - moving down and left
                    else if (Quadrant == 2)
                    {
                        Vector2Int CurrDelta = LineTarget - LineCurrentTile;
                        //to prevent dividing by zero
                        if (CurrDelta.x == 0)
                        {
                            CurrSlope = 999999999;
                        }
                        else
                        {
                            CurrSlope = (float)CurrDelta.y / (float)CurrDelta.x;
                        }
                        //down
                        if (CurrSlope > OriginalSlope)
                        {
                            LineCurrentTile += Vector2Int.down;
                        }
                        //left
                        else
                        {
                            LineCurrentTile += Vector2Int.left;
                        }
                    }
                    //fourth quadrant - moving down and right
                    else if (Quadrant == 3)
                    {
                        Vector2Int CurrDelta = LineTarget - LineCurrentTile;
                        //to prevent dividing by zero
                        if (CurrDelta.x == 0)
                        {
                            CurrSlope = -999999999;
                        }
                        else
                        {
                            CurrSlope = (float)CurrDelta.y / (float)CurrDelta.x;
                            //Debug.Log(CurrSlope);
                        }
                        //right
                        if (CurrSlope > OriginalSlope || CurrSlope == 0)
                        {
                            LineCurrentTile += Vector2Int.right;
                        }
                        //down
                        else
                        {
                            LineCurrentTile += Vector2Int.down;
                        }
                    }

                    if (LightNavArray[LineCurrentTile.x, LineCurrentTile.y] == EmptyTileValue)
                    {
                        ToPreventInfiniteCycle = 1000;
                        continue;
                    }
                    else if(LightNavArray[LineCurrentTile.x, LineCurrentTile.y] == HaveVisionOf)
                    {

                        TilesToExamineNext.Add(new Vector2Int(CurrTile.x + 1, CurrTile.y));
                        TilesToExamineNext.Add(new Vector2Int(CurrTile.x - 1, CurrTile.y));
                        TilesToExamineNext.Add(new Vector2Int(CurrTile.x, CurrTile.y + 1));
                        TilesToExamineNext.Add(new Vector2Int(CurrTile.x, CurrTile.y - 1));


                        GameObject debug = new GameObject();
                        debug.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("pixel");
                        debug.GetComponent<SpriteRenderer>().color = new Color(1,1,0,0.5f);
                        debug.transform.position = new Vector3(CurrTile.x, CurrTile.y, -20);
                        debug.AddComponent<DieInSeconds>().Seconds = 2;

                        ToPreventInfiniteCycle = 1000;
                        continue;
                    }
                    else if(LineCurrentTile == LineTarget)
                    {
                        TilesToExamineNext.Add(new Vector2Int(CurrTile.x + 1, CurrTile.y));
                        TilesToExamineNext.Add(new Vector2Int(CurrTile.x - 1, CurrTile.y));
                        TilesToExamineNext.Add(new Vector2Int(CurrTile.x, CurrTile.y + 1));
                        TilesToExamineNext.Add(new Vector2Int(CurrTile.x, CurrTile.y - 1));


                        GameObject debug = new GameObject();
                        debug.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("pixel");
                        debug.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 0.5f);
                        debug.transform.position = new Vector3(CurrTile.x, CurrTile.y, -20);
                        debug.AddComponent<DieInSeconds>().Seconds = 2;

                        ToPreventInfiniteCycle = 1000;
                        continue;
                    }
                }













            }


        }
        

    }

}
