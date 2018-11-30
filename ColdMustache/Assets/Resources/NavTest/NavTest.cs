using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavTest : MonoBehaviour {

    int ImpassableTileValue = 999999;
    int EmptyTileValue = 888888;
    public int[,] NavArray;

    //WaypointNavigator wn;
    //GameObject enemy;
    //GameObject target;

    // Use this for initialization
    void Start () {
        //enemy = GameObject.Find("EnemyContainer");
        //target = GameObject.Find("Target");

        //wn = GetComponent<WaypointNavigator>();

        Texture2D map = Resources.Load<Texture2D>("NavTest/navmap");
        UnityEngine.Color[] all = map.GetPixels();
        NavArray = new int[map.width, map.height];
        for (int y = 0; y < map.height; y++)
        {
            for(int x = 0; x < map.width; x++)
            {
                if(all[y*map.width + x].r == 0)
                {
                    NavArray[x, y] = ImpassableTileValue;
                }
                else
                {
                    NavArray[x, y] = EmptyTileValue;
                }
            }
        }

        for(int y = 0; y < map.height; y++)
        {
            for(int x = 0; x < map.width; x++)
            {
                if(NavArray[x,y] == ImpassableTileValue)
                {
                    bool ConnectedRight = false;
                    bool ConnectedUp = false;
                    bool ConnectedLeft = false;
                    bool ConnectedDown = false;

                    if(x < map.width-1 && NavArray[x+1,y] == ImpassableTileValue)
                    {
                        ConnectedRight = true;
                    }
                    if (y < map.height - 1 && NavArray[x, y+1] == ImpassableTileValue)
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

                    GameObject concrete = new GameObject();
                    concrete.transform.parent = go.transform;
                    concrete.transform.localPosition = new Vector3(0, 0, 1);
                    concrete.AddComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("NavTest/ConcreteDull320")[y%10*10+x%10];
                }
            }
        }
    }
	
    public bool IsTileEmpty(int X, int Y)
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

    public List<Vector2> FindAPath(int SourceX, int SourceY, int TargetX, int TargetY)
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
                return null;
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

        return WayPoints;
        //wn.WayPoints = WayPoints;
        
    }

    public List<Vector3> CalculateExplosion(int SourceX, int SourceY, int ExpandForce)
    {

        /*
         * this model starts calculation from origin, with some expandforce = pressure
         * the pressure on a given tile is written down, and shared among all nearby tiles that dont have pressure yet
         *
         * result is not very accurate model, but it travels far
         *
         */


        int[,] CurrNavArr = (int[,])NavArray.Clone();

        //int CurrDistance = 0;
        List<Vector2> TilesToExamineNext = new List<Vector2>();
        List<Vector2> TilesCurrentlyBeingExamined = new List<Vector2>();

        List<Vector2> OutputCoordinates = new List<Vector2>(); //only x and y
        List<Vector3> Output = new List<Vector3>(); // x and y are coordinates of explosion, z is expandforce

        TilesToExamineNext.Add(new Vector2(SourceX, SourceY));
        CurrNavArr[SourceX, SourceY] = ExpandForce;

        int AvoidInfiniteCycle = 100;

        while (TilesToExamineNext.Count!=0 && AvoidInfiniteCycle > 0)
        {
            AvoidInfiniteCycle--;

            TilesCurrentlyBeingExamined.Clear();
            foreach (Vector2 v in TilesToExamineNext)
            {
                TilesCurrentlyBeingExamined.Add(v);
            }
            TilesToExamineNext.Clear();
            
            if(ExpandForce < 1)
            {
                break;
            }

            foreach (Vector2 v in TilesCurrentlyBeingExamined)
            {
                if (CurrNavArr[(int)v.x, (int)v.y] > 1)
                {
                    int ExpandableDirections = 0;
                    if (CurrNavArr[(int)v.x + 1, (int)v.y] == EmptyTileValue)
                    {
                        ExpandableDirections++;
                    }
                    if (CurrNavArr[(int)v.x - 1, (int)v.y] == EmptyTileValue)
                    {
                        ExpandableDirections++;
                    }
                    if (CurrNavArr[(int)v.x, (int)v.y + 1] == EmptyTileValue)
                    {
                        ExpandableDirections++;
                    }
                    if (CurrNavArr[(int)v.x, (int)v.y - 1] == EmptyTileValue)
                    {
                        ExpandableDirections++;
                    }

                    if(ExpandableDirections > 0)
                    {
                        
                        if (CurrNavArr[(int)v.x + 1, (int)v.y] == EmptyTileValue)
                        {
                            CurrNavArr[(int)v.x + 1, (int)v.y] = Mathf.FloorToInt(((float)CurrNavArr[(int)v.x, (int)v.y] -0.3f)/ (float)ExpandableDirections);
                            TilesToExamineNext.Add(new Vector2(v.x + 1, v.y));
                            OutputCoordinates.Add(new Vector2(v.x + 1, v.y));
                        }
                        if (CurrNavArr[(int)v.x - 1, (int)v.y] == EmptyTileValue)
                        {
                            CurrNavArr[(int)v.x - 1, (int)v.y] = Mathf.FloorToInt(((float)CurrNavArr[(int)v.x, (int)v.y] -0.3f)/ (float)ExpandableDirections);
                            TilesToExamineNext.Add(new Vector2(v.x - 1, v.y));
                            OutputCoordinates.Add(new Vector2(v.x - 1, v.y));
                        }
                        if (CurrNavArr[(int)v.x, (int)v.y + 1] == EmptyTileValue)
                        {
                            CurrNavArr[(int)v.x, (int)v.y + 1] = Mathf.FloorToInt(((float)CurrNavArr[(int)v.x, (int)v.y] -0.3f)/ (float)ExpandableDirections);
                            TilesToExamineNext.Add(new Vector2(v.x, v.y + 1));
                            OutputCoordinates.Add(new Vector2(v.x, v.y + 1));
                        }
                        if (CurrNavArr[(int)v.x, (int)v.y - 1] == EmptyTileValue)
                        {
                            CurrNavArr[(int)v.x, (int)v.y - 1] = Mathf.FloorToInt(((float)CurrNavArr[(int)v.x, (int)v.y] -0.3f)/ (float)ExpandableDirections);
                            TilesToExamineNext.Add(new Vector2(v.x, v.y - 1));
                            OutputCoordinates.Add(new Vector2(v.x, v.y - 1));
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

        foreach(Vector2 v in OutputCoordinates)
        {
            Output.Add(new Vector3(v.x, v.y, CurrNavArr[(int)v.x, (int)v.y]));
        }
        
        return Output;
    }

    public List<Vector3> CalculateExplosion_Distributor(int SourceX, int SourceY, int ExpandForce)
    {

        /*
         * this model allows for bouncing of pressure waves - this bouncing greatly increases pressures in small areas, but as a result explosion doesnt travel as far
         * 
         * there is a "pressure distributor" - it increases the pressure of a tile its currently "standing in", and spawns new pressure distrubutors to all nearby non-wall tiles
         * and shares its pressure among the new pressure distributors
         * 
         * as a result, explosion will further increase its own pressure, but wont travel far
         * 
         * 
         */



        int[,] CurrNavArr = (int[,])NavArray.Clone();

        //int CurrDistance = 0;
        List<Vector3Int> TilesToExamineNext = new List<Vector3Int>();
        List<Vector3Int> TilesCurrentlyBeingExamined = new List<Vector3Int>();

        List<Vector2Int> OutputCoordinates = new List<Vector2Int>(); //only x and y
        List<Vector3> Output = new List<Vector3>(); // x and y are coordinates of explosion, z is expandforce

        TilesToExamineNext.Add(new Vector3Int(SourceX, SourceY, ExpandForce));
        //CurrNavArr[SourceX, SourceY] = ExpandForce;

        int AvoidInfiniteCycle = 100;

        OutputCoordinates.Add(new Vector2Int( SourceX, SourceY));

        while (TilesToExamineNext.Count != 0 && AvoidInfiniteCycle > 0)
        {
            AvoidInfiniteCycle--;

            TilesCurrentlyBeingExamined.Clear();
            foreach (Vector3Int v in TilesToExamineNext)
            {
                TilesCurrentlyBeingExamined.Add(v);
            }
            TilesToExamineNext.Clear();

            if (ExpandForce < 1)
            {
                break;
            }

            foreach (Vector3Int v in TilesCurrentlyBeingExamined)
            {
                if (v.z > 0)
                {
                    if(CurrNavArr[v.x, v.y] == EmptyTileValue)
                    {
                        CurrNavArr[v.x, v.y] = 0;
                    }
                    CurrNavArr[v.x, v.y] += v.z;


                    int ExpandableDirections = 0;
                    if (CurrNavArr[(int)v.x + 1, (int)v.y] != ImpassableTileValue)
                    {
                        ExpandableDirections++;
                    }
                    if (CurrNavArr[(int)v.x - 1, (int)v.y] != ImpassableTileValue)
                    {
                        ExpandableDirections++;
                    }
                    if (CurrNavArr[(int)v.x, (int)v.y + 1] != ImpassableTileValue)
                    {
                        ExpandableDirections++;
                    }
                    if (CurrNavArr[(int)v.x, (int)v.y - 1] != ImpassableTileValue)
                    {
                        ExpandableDirections++;
                    }

                    if (ExpandableDirections > 0)
                    {
                        int NewDistributorPressure = Mathf.FloorToInt(((float)v.z - 0.3f) / (float)ExpandableDirections);

                        if (CurrNavArr[(int)v.x + 1, (int)v.y] != ImpassableTileValue)
                        {
                            TilesToExamineNext.Add(new Vector3Int(v.x + 1, v.y, NewDistributorPressure));
                            if(!OutputCoordinates.Contains(new Vector2Int(v.x + 1, v.y)) && NewDistributorPressure > 0)
                            {
                                OutputCoordinates.Add(new Vector2Int(v.x + 1, v.y));
                            }
                        }
                        if (CurrNavArr[(int)v.x - 1, (int)v.y] != ImpassableTileValue)
                        {
                            TilesToExamineNext.Add(new Vector3Int(v.x - 1, v.y, NewDistributorPressure));
                            if (!OutputCoordinates.Contains(new Vector2Int(v.x - 1, v.y)) && NewDistributorPressure > 0)
                            {
                                OutputCoordinates.Add(new Vector2Int(v.x - 1, v.y));
                            }
                        }
                        if (CurrNavArr[(int)v.x, (int)v.y + 1] != ImpassableTileValue)
                        {
                            TilesToExamineNext.Add(new Vector3Int(v.x, v.y + 1, NewDistributorPressure));
                            if (!OutputCoordinates.Contains(new Vector2Int(v.x, v.y + 1)) && NewDistributorPressure > 0)
                            {
                                OutputCoordinates.Add(new Vector2Int(v.x, v.y + 1));
                            }
                        }
                        if (CurrNavArr[(int)v.x, (int)v.y - 1] != ImpassableTileValue)
                        {
                            TilesToExamineNext.Add(new Vector3Int(v.x, v.y - 1, NewDistributorPressure));
                            if (!OutputCoordinates.Contains(new Vector2Int(v.x, v.y - 1)) && NewDistributorPressure > 0)
                            {
                                OutputCoordinates.Add(new Vector2Int(v.x, v.y - 1));
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
            Output.Add(new Vector3Int(v.x, v.y, CurrNavArr[(int)v.x, (int)v.y]));
        }

        return Output;
    }

    public List<Vector3> CalculateExplosion_DistributorNoBacksies(int SourceX, int SourceY, int ExpandForce)
    {

        /*
         * based on _Distributor model, but distributors cant go directly back from where they came from
         * this should theoretically increase the spread of the explosion
         * 
         */

        int Right = 0;
        int Up = 90;
        int Left = 180;
        int Down = 270;


        int[,] CurrNavArr = (int[,])NavArray.Clone();

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
                if (v.z > 0)
                {
                    if (CurrNavArr[(int)v.x, (int)v.y] == EmptyTileValue)
                    {
                        CurrNavArr[(int)v.x, (int)v.y] = 0;
                    }
                    CurrNavArr[(int)v.x, (int)v.y] += (int)v.z;


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

                    if (ExpandableDirections > 0)
                    {
                        int NewDistributorPressure = Mathf.FloorToInt(((float)v.z - 0.3f) / (float)ExpandableDirections);
                        if(ExpandableDirections == 1)
                        {
                            NewDistributorPressure -= 2;
                        }

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
            Output.Add(new Vector3Int(v.x, v.y, CurrNavArr[(int)v.x, (int)v.y]));
        }

        return Output;
    }

    /*
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
	}*/
}
