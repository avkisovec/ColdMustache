using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NavTestStatic : MonoBehaviour {

    #region Declarations

    public const int ImpassableTileValue = 999999;
    public const int EmptyTileValue = 888888;

    public static int MapWidth = 200;
    public static int MapHeight = 200;

    //the maximum radius AvkisLight and explosions using it can have
    public static int MaxLightAndExplosionDistance = 25;

    public static int[,] NavArray;
    public static int[,] ExplosionNavArray;
    public static int[,] LightNavArray;

    #endregion

    #region Building

    void Start()
    {

        //set up an empty nav array (all tiles considered empty/walkable)
        NavTestStatic.NavArray = new int[NavTestStatic.MapWidth, NavTestStatic.MapHeight];
        for (int y = 0; y < NavTestStatic.MapWidth; y++)
        {
            for (int x = 0; x < NavTestStatic.MapHeight; x++)
            {
                NavTestStatic.NavArray[x, y] = NavTestStatic.EmptyTileValue;
            }
        }

        //set up the other arrays
        //explosion nav array and light array are clones of nav array, therefore empty on default
        NavTestStatic.ExplosionNavArray = (int[,])NavTestStatic.NavArray.Clone();
        NavTestStatic.LightNavArray = (int[,])NavTestStatic.NavArray.Clone();

        //go through all existing environment objects in scene, and if they block particular thing, note in in the array
        foreach (EnvironmentObject eo in GameObject.FindObjectsOfType<EnvironmentObject>())
        {
            if (!eo.Nav_Walkable)
            {
                NavTestStatic.NavArray[Mathf.RoundToInt(eo.transform.position.x), Mathf.RoundToInt(eo.transform.position.y)] = NavTestStatic.ImpassableTileValue;
            }
            if (!eo.Nav_ExplosionCanPass)
            {
                NavTestStatic.ExplosionNavArray[Mathf.RoundToInt(eo.transform.position.x), Mathf.RoundToInt(eo.transform.position.y)] = NavTestStatic.ImpassableTileValue;
            }
            if (!eo.Nav_LightCanPass)
            {
                NavTestStatic.LightNavArray[Mathf.RoundToInt(eo.transform.position.x), Mathf.RoundToInt(eo.transform.position.y)] = NavTestStatic.ImpassableTileValue;
            }
        }


        NavTestStatic.AvkisLight_build(MaxLightAndExplosionDistance);
    }

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

    #endregion

    #region Debug

    public static void ExportNavMap()
    {

        SaverLoader.CreateHardPathIfNeeded(Application.dataPath + "/debug/navMapOutput.txt");

        StreamWriter sw = new StreamWriter(Application.dataPath + "/debug/navMapOutput.txt");
        sw.AutoFlush = true;

        for (int y = MapHeight - 1; y >= 0; y--)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                if (NavArray[x, y] == ImpassableTileValue)
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

    #endregion

    #region General

    public static bool IsTileWithinBounds(Vector2Int Tile)
    {
        if (Tile.x >= 0 && Tile.y >= 0 && Tile.x < MapWidth && Tile.y < MapHeight)
        {
            return true;
        }
        return false;
    }

    #endregion

    #region Navigation

    public static List<Vector2> FindAPath(int SourceX, int SourceY, int TargetX, int TargetY, int MaxDistance = 20)
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

            if(CurrDistance > MaxDistance)
            {
                return null;
            }

            TilesCurrentlyBeingExamined.Clear();
            foreach (Vector2 v in TilesToExamineNext)
            {
                TilesCurrentlyBeingExamined.Add(v);
            }
            TilesToExamineNext.Clear();
            

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
    
    

    public static bool IsTileWalkable(int X, int Y)
    {
        if (NavArray[X, Y] == EmptyTileValue)
        {
            return true;
        }
        return false;
    }

    public static bool IsTileWalkable(Vector2Int tile)
    {
        if (NavArray[tile.x, tile.y] == EmptyTileValue)
        {
            return true;
        }
        return false;
    }

    public static bool IsPathWalkable(List<Vector2Int> Path)
    {
        foreach(Vector2Int tile in Path)
        {
            if (!IsTileWalkable(tile))
            {
                return false;
            }
        }
        return true;
    }

    #endregion

    #region Light

    public static bool CanLightPassThroughTile(int x, int y)
    {
        if (LightNavArray[x, y] == EmptyTileValue)
        {
            return true;
        }
        return false;
    }

    public static bool CanLightPassThroughTile(Vector2Int tile)
    {
        if (LightNavArray[tile.x, tile.y] == EmptyTileValue)
        {
            return true;
        }
        return false;
    }

    public static bool CanLightPassThroughPath(List<Vector2Int> Path)
    {
        foreach (Vector2Int tile in Path)
        {
            if (!CanLightPassThroughTile(tile))
            {
                return false;
            }
        }
        return true;
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

    //returns null if the line is obstructed, returns the path is the line of sight is clear
    public static List<Vector2Int> GetLineOfSightOptimised(Vector2 Origin, Vector2 Target)
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
            if (CanLightPassThroughTile(x0, y0))
            {
                output.Add(new Vector2Int(x0, y0));
            }
            else
            {
                return null;
            }
            if (x0 == x1 && y0 == y1) break;
            e2 = err;
            if (e2 > -dx) { err -= dy; x0 += sx; }
            if (e2 < dy) { err += dx; y0 += sy; }
        }
        return output;
    }

    #endregion

    #region Explosion

    public static bool CanExplosionPassThroughTile(Vector2Int tile)
    {
        if (ExplosionNavArray[tile.x, tile.y] == EmptyTileValue)
        {
            return true;
        }
        return false;
    }

    //RETURNS a vector 3, X and Y are TILE COORDINATES and Z is DISTANCE FROM SOURCE
    public static List<Vector3> GetExplosionArea(Vector2Int Source, float MaxDistance){
        List<Vector3>Output = new List<Vector3>();

        AvkisLight_cast_explosion_recursion(Source, AvkisLightNodes[0], MaxDistance, Output);

        return Output;
    }

    static void AvkisLight_cast_explosion_recursion(Vector2Int Source, AvkisLightNode node, float MaxDistance, List<Vector3> Output)
    {
        if (IsTileWithinBounds(Source + node.Coordinates))
        {
            Output.Add(new Vector3(Source.x + node.Coordinates.x, Source.y + node.Coordinates.y, node.DistanceFromSource));
            if (CanExplosionPassThroughTile(Source + node.Coordinates) && node.DistanceFromSource < MaxDistance)
            {
                foreach (AvkisLightNode child in node.Children)
                {
                    AvkisLight_cast_explosion_recursion(Source, child, MaxDistance, Output);
                }

            }
        }
    }

    #endregion

    #region AvkisLight

    /*
     * HOW AVKISLIGHT WORKS
     * 
     * instead of calculating light passes at runtime (like bresenham light does),
     * AvkisLight builds a map of Nodes (the map is built on game load, and cached)
     * 
     * the node map starts with root node, which represents the root tile (the tile light is calculated from)
     * every node has its children - these represents neighboring tiles to which light will spread from current node/tile
     * every node is accessible in exactly one way, from one parent node (therefore during calculation every node/tile will be calculated only once, and if light cannot reach it, the children won't ever be calculated)
     * 
     * the actual light calculation is then very simple - check if light can pass through a node, and then recursively keep checking children
     * 
     * AvkisLight has huge advantage over Bresenham light, because it calculates each tile only once - this means way faster calculation at runtime
     * also if a light cannot pass through a node, it automatically cannot pass through any children - they wont be calculated
     * the downside is having to build the map on load, but thats not a problem - its worth spending some miliseconds during loading to save them on every frame after
     * 
     * this algorithm is way more efficient if there are many tiles to calculate, as it will always calculate every visible tile only once
     * 
     * 
     * usage:
     * somewhere on load/start:
     *      NavTestStatic.AvkisLight_build(MaxRadius);
     *      
     * then when you need:
     *      foreach(Vector2Int VisibleTile in NavTestStatic.AvkisLight_cast(PlayerPosition))
     *      {
     *          //do stuff with visible tiles
     *      }
     * 
     * 
     */

    public static List<AvkisLightNode> AvkisLightNodes;

    public static void AvkisLight_build(int MaxRadius)
    {
        List<Vector2Int> Output = new List<Vector2Int>();
        AvkisLightNodes = new List<AvkisLightNode>();

        //adding root node
        AvkisLightNodes.Add(new AvkisLightNode(new Vector2Int(0, 0)));

        for(int Radius = 1; Radius < MaxRadius; Radius++)
        {
            foreach(Vector2Int CirclePoint in BresenhamCircle_unrestricted(new Vector2Int(0,0), Radius))
            {                
                //if node with identical coordinates already exists, skip to next one
                //i belive its not possible, so you brobably can delete this check
                //update: bresenham's midpoint circle will never overlap, it rather leaves gaps; but you should keep this check if you use different circle algorithm
                foreach (AvkisLightNode node in AvkisLightNodes)
                {
                    if (node.Coordinates == CirclePoint)
                    {
                        //collision found - current circle shares a point with some previous circle
                        goto NextCirclePoint;
                    }
                }
                
                //casting a line from circlepoint toward center
                List<Vector2Int> Line = BresenhamLine(CirclePoint, new Vector2(0, 0));
                
                AvkisLightNode found = null;
                foreach (AvkisLightNode node in AvkisLightNodes)
                {
                    if (node.Coordinates == Line[1])
                    {
                        found = node;
                        break;
                    }
                }

                //if parent node was found immediatelly, no need to skip a gap
                if (found != null)
                {
                    AvkisLightNode nu = new AvkisLightNode(CirclePoint, Radius);
                    AvkisLightNodes.Add(nu);
                    found.AddChild(nu);
                }
                //skipping a gap (putting a node into the gap)
                else
                {
                    AvkisLightNode nu = new AvkisLightNode(CirclePoint, Radius);
                    AvkisLightNode GapFiller = new AvkisLightNode(Line[1], Radius);
                    foreach (AvkisLightNode node in AvkisLightNodes)
                    {
                        if (node.Coordinates == Line[2])
                        {
                            found = node;
                            break;
                        }
                    }
                    AvkisLightNodes.Add(nu);
                    AvkisLightNodes.Add(GapFiller);
                    found.AddChild(GapFiller);
                    GapFiller.AddChild(nu);
                }

                NextCirclePoint:;
            }
        }
        
    }

    public static List<Vector2Int> AvkisLight_cast(Vector2Int Source)
    {        
        List<Vector2Int> Output = new List<Vector2Int>();

        AvkisLight_cast_recursion(Source, AvkisLightNodes[0], Output);

        return Output;
    }

    static void AvkisLight_cast_recursion(Vector2Int Source, AvkisLightNode node, List<Vector2Int> Output)
    {
        if(IsTileWithinBounds(Source + node.Coordinates))
        {
            Output.Add(Source + node.Coordinates);
            if (CanLightPassThroughTile(Source + node.Coordinates))
            {                
                foreach(AvkisLightNode child in node.Children)
                {
                    AvkisLight_cast_recursion(Source, child, Output);
                }

            }
        }
    }

    public class AvkisLightNode
    {
        public Vector2Int Coordinates;
        public AvkisLightNode[] Children;

        public float DistanceFromSource = -1;

        public AvkisLightNode(Vector2Int Coordinates, float DistanceFromSource = -1)
        {
            this.Coordinates = Coordinates;
            this.DistanceFromSource = DistanceFromSource;
            Children = new AvkisLightNode[0];
        }

        public void AddChild(AvkisLightNode Child)
        {
            AvkisLightNode[] ChildrenOriginal = (AvkisLightNode[])Children.Clone();
            Children = new AvkisLightNode[ChildrenOriginal.Length + 1];
            for (int i = 0; i < ChildrenOriginal.Length; i++)
            {
                Children[i] = ChildrenOriginal[i];
            }
            Children[Children.Length - 1] = Child;
        }

    }

    #endregion

    #region Bresenham

    public static List<Vector2Int> BresenhamLight(Vector2Int Source, int radius)
    {
        List<Vector2Int> output = new List<Vector2Int>();
        
        foreach(Vector2Int CirclePoint in BresenhamCircle_unrestricted(Source, radius))
        {
            foreach (Vector2Int v in BresenhamLine(Source, CirclePoint))
            {
                if (IsTileWithinBounds(v) && CanLightPassThroughTile(v))
                {
                    if (!output.Contains(v))
                    {
                        output.Add(v);
                    }
                    continue;
                }
                else
                {
                    break;
                }
            }
        }
        return output;
    }

    /*
     * The following code (BresenhamLine() and BresenhamCircle()) is adapted version of code from:
     * https://rosettacode.org/wiki/Bitmap/Bresenham%27s_line_algorithm#C.23
     * and
     * https://rosettacode.org/wiki/Bitmap/Midpoint_circle_algorithm#C.23
     * respectively.
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
            
    public static List<Vector2Int> BresenhamCircle(Vector2Int Origin, int radius)
    {
        List<Vector2Int> output = new List<Vector2Int>();
        int centerX = Origin.x;
        int centerY = Origin.y;
        int d = (5 - radius * 4) / 4;
        int x = 0;
        int y = radius;

        do
        {
            // ensure index is in range before setting (depends on your image implementation)
            // in this case we check if the pixel location is within the bounds of the image before setting the pixel
            if (centerX + x >= 0 && centerX + x < MapWidth && centerY + y >= 0 && centerY + y < MapHeight) output.Add(new Vector2Int(centerX + x, centerY + y));
            if (centerX + x >= 0 && centerX + x < MapWidth && centerY - y >= 0 && centerY - y < MapHeight) output.Add(new Vector2Int(centerX + x, centerY - y));
            if (centerX - x >= 0 && centerX - x < MapWidth && centerY + y >= 0 && centerY + y < MapHeight) output.Add(new Vector2Int(centerX - x, centerY + y));
            if (centerX - x >= 0 && centerX - x < MapWidth && centerY - y >= 0 && centerY - y < MapHeight) output.Add(new Vector2Int(centerX - x, centerY - y));
            if (centerX + y >= 0 && centerX + y < MapWidth && centerY + x >= 0 && centerY + x < MapHeight) output.Add(new Vector2Int(centerX + y, centerY + x));
            if (centerX + y >= 0 && centerX + y < MapWidth && centerY - x >= 0 && centerY - x < MapHeight) output.Add(new Vector2Int(centerX + y, centerY - x));
            if (centerX - y >= 0 && centerX - y < MapWidth && centerY + x >= 0 && centerY + x < MapHeight) output.Add(new Vector2Int(centerX - y, centerY + x));
            if (centerX - y >= 0 && centerX - y < MapWidth && centerY - x >= 0 && centerY - x < MapHeight) output.Add(new Vector2Int(centerX - y, centerY - x));
            if (d < 0)
            {
                d += 2 * x + 1;
            }
            else
            {
                d += 2 * (x - y) + 1;
                y--;
            }
            x++;
        } while (x <= y);
        return output;
    }

    //this version doesnt check if its within bounds
    public static List<Vector2Int> BresenhamCircle_unrestricted(Vector2Int Origin, int radius)
    {
        List<Vector2Int> output = new List<Vector2Int>();
        int centerX = Origin.x;
        int centerY = Origin.y;
        int d = (5 - radius * 4) / 4;
        int x = 0;
        int y = radius;

        do
        {
            // ensure index is in range before setting (depends on your image implementation)
            // in this case we check if the pixel location is within the bounds of the image before setting the pixel
            output.Add(new Vector2Int(centerX + x, centerY + y));
            output.Add(new Vector2Int(centerX + x, centerY - y));
            output.Add(new Vector2Int(centerX - x, centerY + y));
            output.Add(new Vector2Int(centerX - x, centerY - y));
            output.Add(new Vector2Int(centerX + y, centerY + x));
            output.Add(new Vector2Int(centerX + y, centerY - x));
            output.Add(new Vector2Int(centerX - y, centerY + x));
            output.Add(new Vector2Int(centerX - y, centerY - x));
            if (d < 0)
            {
                d += 2 * x + 1;
            }
            else
            {
                d += 2 * (x - y) + 1;
                y--;
            }
            x++;
        } while (x <= y);
        return output;
    }

    #endregion


}
