using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTopsDecorator : MonoBehaviour
{

    public bool ENABLED = true;

    public Sprite BaseSprite;

    public string SpritesheetPath;

    int VariationsOfEachSbusprite;

    Sprite[] Sprites;

    // Start is called before the first frame update
    void Start()
    {
        if (!ENABLED) return;

        Generate();

        ENABLED = false;
    }

    void Generate()
    {
        Sprites = Resources.LoadAll<Sprite>(SpritesheetPath);

        VariationsOfEachSbusprite = Sprites.Length/12;


        //gets all wall tops from navteststatic's wall array
        Transform[,] WallTops = new Transform[NavTestStatic.MapWidth, NavTestStatic.MapHeight];

        for(int y = 2; y < NavTestStatic.MapHeight; y++){
            for(int x = 0; x < NavTestStatic.MapWidth; x++){


                //makes sure to only select the tops and not fronts
                //also doesnt select the top that is hidden by a front
                if(NavTestStatic.WallTransformsArray[x,y] != null &&
                NavTestStatic.WallTransformsArray[x, y-2] != null
                ){
                    WallTops[x,y] = NavTestStatic.WallTransformsArray[x,y];
                }
                
            }
        }



        int ChildCount = transform.childCount;
        for(int i = 0; i <ChildCount; i++){
            Transform Child = transform.GetChild(i);
            int x = Mathf.RoundToInt(Child.position.x);
            int y = Mathf.RoundToInt(Child.position.y);

            //skip tiles that arent walltops, or are hidden by a front
            if (WallTops[x, y] == null) continue;

            bool ConnectedRight = false;
            bool ConnectedRightUp = false;
            bool ConnectedUp = false;
            bool ConnectedLeftUp = false;
            bool ConnectedLeft = false;
            bool ConnectedLeftDown = false;
            bool ConnectedDown = false;
            bool ConnectedRightDown = false;
            int Connections = 0;


            //right
            if (IsTileWithinLevelBounds(x + 1, y)) { if (WallTops[x + 1, y] != null) { ConnectedRight = true; Connections++; } }
            //right up
            if (IsTileWithinLevelBounds(x + 1, y + 1)) { if (WallTops[x + 1, y + 1] != null) { ConnectedRightUp = true; Connections++; } }
            //up
            if (IsTileWithinLevelBounds(x, y + 1)) { if (WallTops[x, y + 1] != null) { ConnectedUp = true; Connections++; } }
            //left up
            if (IsTileWithinLevelBounds(x - 1, y + 1)) { if (WallTops[x - 1, y + 1] != null) { ConnectedLeftUp = true; Connections++; } }
            //left
            if (IsTileWithinLevelBounds(x - 1, y)) { if (WallTops[x - 1, y] != null) { ConnectedLeft = true; Connections++; } }
            //left down
            if (IsTileWithinLevelBounds(x - 1, y - 1)) { if (WallTops[x - 1, y - 1] != null) { ConnectedLeftDown = true; Connections++; } }
            //down
            if (IsTileWithinLevelBounds(x, y - 1)) { if (WallTops[x, y - 1] != null) { ConnectedDown = true; Connections++; } }
            //right down
            if (IsTileWithinLevelBounds(x + 1, y - 1)) { if (WallTops[x + 1, y - 1] != null) { ConnectedRightDown = true; Connections++; } }



            #region DestroyingOldDecorations

            SpriteRenderer DamageOverlay = Child.GetComponent<WallSegmentTileIndependent>().DamageOverlay;
            Transform DamageOverlayTr = DamageOverlay.transform;

            int Limit = Child.childCount;

            for(int ii = 0; ii < Limit; ii++){

                if(Child.GetChild(ii) != DamageOverlayTr) Destroy(Child.GetChild(ii).gameObject);

            }

            #endregion



            #region TopRightSubtile

            Sprite SubtileSprite = null;

            if (!ConnectedRight && !ConnectedUp) SubtileSprite = GetSprite(8);
            else if (!ConnectedRight) SubtileSprite = GetSprite(0);
            else if (!ConnectedUp) SubtileSprite = GetSprite(1);
            else if (!ConnectedRightUp) SubtileSprite = GetSprite(4);

            if (SubtileSprite != null)
            {
                GameObject SubTile = new GameObject();
                SubTile.AddComponent<SpriteRenderer>().sprite = SubtileSprite;
                SubTile.transform.parent = WallTops[x, y];
                SubTile.transform.localPosition = new Vector3(0.25f, 0.25f, -0.01f);
            }

            #endregion

            #region TopLeftSubtile

            SubtileSprite = null;

            if (!ConnectedLeft && !ConnectedUp) SubtileSprite = GetSprite(9);
            else if (!ConnectedLeft) SubtileSprite = GetSprite(2);
            else if (!ConnectedUp) SubtileSprite = GetSprite(1);
            else if (!ConnectedLeftUp) SubtileSprite = GetSprite(5);

            if (SubtileSprite != null)
            {
                GameObject SubTile = new GameObject();
                SubTile.AddComponent<SpriteRenderer>().sprite = SubtileSprite;
                SubTile.transform.parent = WallTops[x, y];
                SubTile.transform.localPosition = new Vector3(-0.25f, 0.25f, -0.01f);
            }

            #endregion

            #region DownLeftSubtile

            SubtileSprite = null;

            if (!ConnectedLeft && !ConnectedDown) SubtileSprite = GetSprite(10);
            else if (!ConnectedLeft) SubtileSprite = GetSprite(2);
            else if (!ConnectedDown) SubtileSprite = GetSprite(3);
            else if (!ConnectedLeftDown) SubtileSprite = GetSprite(6);

            if (SubtileSprite != null)
            {
                GameObject SubTile = new GameObject();
                SubTile.AddComponent<SpriteRenderer>().sprite = SubtileSprite;
                SubTile.transform.parent = WallTops[x, y];
                SubTile.transform.localPosition = new Vector3(-0.25f, -0.25f, -0.01f);
            }

            #endregion

            #region  DownRightSubtile

            SubtileSprite = null;

            if (!ConnectedRight && !ConnectedDown) SubtileSprite = GetSprite(11);
            else if (!ConnectedRight) SubtileSprite = GetSprite(0);
            else if (!ConnectedDown) SubtileSprite = GetSprite(3);
            else if (!ConnectedRightDown) SubtileSprite = GetSprite(7);

            if (SubtileSprite != null)
            {
                GameObject SubTile = new GameObject();
                SubTile.AddComponent<SpriteRenderer>().sprite = SubtileSprite;
                SubTile.transform.parent = WallTops[x, y];
                SubTile.transform.localPosition = new Vector3(0.25f, -0.25f, -0.01f);
            }

            #endregion

        }
    }


    Sprite GetSprite(int Rowindex){
        return Sprites[Rowindex*VariationsOfEachSbusprite + Random.Range(0,VariationsOfEachSbusprite)];
    }


    bool IsTileWithinLevelBounds(int x, int y)
    {
        if (x >= 0 &&
            x < NavTestStatic.MapWidth &&
            y >= 0 &&
            y < NavTestStatic.MapHeight
            )
        {
            return true;
        }
        return false;
    }

}
