using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WallSegment : MonoBehaviour
{

    /*
    
    wall segment is several wall tiles (1x1) stacked on top of one another in an unbroken line

    the bottommost two will have the front of the wall assigned as a sprite, while the rest will get the top

    this can then dynamically adapt to destruction of tiles
    
     */

    public bool UseChildrenAsTiles = false;
    
    //the sprites for the front of a new wall
    public Sprite[] MainSprites;
    //sprites for the front that gets created by destroying a wall
    public Sprite[] DamagedSprites;
    //sprites for the top sections of walls
    public Sprite[] TopSprites;

    public WallSegmentTile[] Tiles = null;

    //new parent can be null, thats not a problem
    public Transform NewParentForTiles = null;

    public float TileMaxHealth;
    public SpriteRenderer[] spriteRenderers;
    public SpriteRenderer[] DamageOverlaySRs;


    // Start is called before the first frame update
    void Start()
    {
        //initialised by builder that created it
        //ini();
    }

    public void ini(WallSegmentTile[] ToBeTiles = null, bool ChildrenAsTiles = true, bool UseDamagedSprites = false, bool FirstTime = true, Sprite[] MainSprites = null, Sprite[] DamagedSprites = null, Sprite[] TopSprites = null){
        this.UseChildrenAsTiles = ChildrenAsTiles;

        this.MainSprites = MainSprites;
        this.DamagedSprites = DamagedSprites;
        this.TopSprites = TopSprites;

        if(ToBeTiles!=null){
            Tiles = ToBeTiles;
        }
        else if(UseChildrenAsTiles){

            //when i already used them and moved them aside, i cant use them anymore as i dont have any
            //this is important when using builder and then saving from runtime
            UseChildrenAsTiles = false;

            List<WallSegmentTile> Children_TilesScripts = new List<WallSegmentTile>();



            //limit as separate var as childcount gets reduced as children are removed
            //basically replaces transform.childCount
            int Limit = transform.childCount;

            //get all children as a list so they can be sorted accoring to their Y
            List<Transform> ChildrenTransforms = new List<Transform>();

            //filling the list
            for(int i = 0; i < Limit; i++){
                ChildrenTransforms.Add(transform.GetChild(i));
            }

            //sorting itself
            ChildrenTransforms = ChildrenTransforms.OrderByDescending(o => o.position.y).ToList();

            //foreach child (now in sorted list)
            for(int i = 0; i < Limit; i++){

                Transform childTransform = ChildrenTransforms[i];
                childTransform.name = i.ToString();

                //get out of parent, otherwise will get destroyed during the splitting and destruction of this segment
                childTransform.parent = NewParentForTiles;

                Children_TilesScripts.Add(childTransform.GetComponent<WallSegmentTile>());

            }

            Tiles = (WallSegmentTile[]) Children_TilesScripts.ToArray();
            
        }

        if(Tiles.Length < 3){
            for(int i = 0; i < Tiles.Length;i++){
                Tiles[i].Die();
            }
            Destroy(this.gameObject);
            return;
        }

        spriteRenderers = new SpriteRenderer[Tiles.Length];
        //DamageOverlaySRs = new SpriteRenderer[Tiles.Length];

        for (int i = 0; i < Tiles.Length; i++)
        {

            Tiles[i].IndexInSegment = i;
            Tiles[i].Parent = this;
            
            spriteRenderers[i] = Tiles[i].GetComponent<SpriteRenderer>();
        }
        TileMaxHealth = Tiles[0].MaxHealth;

        //MainSprites = Resources.LoadAll<Sprite>("Environment/Physical/Walls/Segment/Wall");

        if(UseDamagedSprites){
            int ChosenSprite = Random.Range(0, this.DamagedSprites.Length/2);
            spriteRenderers[spriteRenderers.Length - 1].sprite = DamagedSprites[DamagedSprites.Length / 2 + ChosenSprite];
            spriteRenderers[spriteRenderers.Length - 2].sprite = DamagedSprites[ChosenSprite];

            //destroy all children - for situations where some decorative object was part of the wall
            Transform UpperSectionOfFrontSide = spriteRenderers[spriteRenderers.Length - 1].transform;
            for(int i = 0; i < UpperSectionOfFrontSide.childCount; i++){
                Destroy(UpperSectionOfFrontSide.GetChild(i).gameObject);
            }

        }
        else{
            int ChosenSprite = Random.Range(0, MainSprites.Length / 2);
            spriteRenderers[spriteRenderers.Length - 1].sprite = MainSprites[MainSprites.Length / 2 + ChosenSprite];
            spriteRenderers[spriteRenderers.Length - 2].sprite = MainSprites[ChosenSprite];
        }

        if(FirstTime){
            for (int i = 0; i < Tiles.Length - 2; i++)
            {
                spriteRenderers[i].sprite = TopSprites[Random.Range(0,TopSprites.Length)];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //public method for damaging tiles
    //this is because of "splashing" damage - if a tile near the end were destroyed and left behind too short of a portion of the wall,
    //the too short portion is destroyed anyway
    //to prevent abusing this, hitting tile near the end damages the end aswell (the damage is split) 
    public void ReportTileTakingDamage(int TileIndex, float Damage){

        int RemainingTilesA = TileIndex;
        int RemainingTilesB = Tiles.Length - 1 - TileIndex;


        //DamageATile(TileIndex, Damage);

        List<int> IndexesToBeDamaged = new List<int>();

        IndexesToBeDamaged.Add(TileIndex);

        if( RemainingTilesA == 1){
            IndexesToBeDamaged.Add(TileIndex - 1);
        }
        else if (RemainingTilesA == 2)
        {
            IndexesToBeDamaged.Add(TileIndex - 1);
            IndexesToBeDamaged.Add(TileIndex - 2);
        }

        if (RemainingTilesB == 1)
        {
            IndexesToBeDamaged.Add(TileIndex + 1);
        }
        else if (RemainingTilesB == 2)
        {
            IndexesToBeDamaged.Add(TileIndex + 1);
            IndexesToBeDamaged.Add(TileIndex + 2);
        }


        
        //Damage = Damage / IndexesToBeDamaged.Count;

        for(int i = 0; i < IndexesToBeDamaged.Count; i++){
            DamageATile(IndexesToBeDamaged[i], Damage);
        }

    }

    //actually damages a tile
    private void DamageATile(int TileIndex, float Damage){

        Tiles[TileIndex].Health -= Damage;

        //tile has health left
        if (Tiles[TileIndex].Health > 0){

            float HealthRatio =  0.75f + (Tiles[TileIndex].Health / TileMaxHealth)*0.25f;
            spriteRenderers[TileIndex].color = new Color(HealthRatio, HealthRatio, HealthRatio, 1);


            return;
        } 



        //tile was destroyed

        Split(TileIndex);

        Tiles[TileIndex].Die();

    }

    //missing tile is the one that just was destroyed
    public void Split(int MissingTile){

        WallSegmentTile[] a = new WallSegmentTile[MissingTile];
        WallSegmentTile[] b = new WallSegmentTile[Tiles.Length-1-MissingTile];

        for(int i = 0; i < Tiles.Length; i++){

            //go to a (section ABOVE the now destroyed tile)
            if(i < MissingTile){
                a[i] = Tiles[i];
            }

            //go to b (section BELOW the now destroyed tile)
            if(i > MissingTile){
                b[i-MissingTile-1] = Tiles[i];
            }

        }

        GameObject goa = new GameObject();
        goa.AddComponent<WallSegment>().ini(a, false, true, false, MainSprites, DamagedSprites, TopSprites);

        

        GameObject gob = new GameObject();
        gob.AddComponent<WallSegment>().ini(b, false, false, false, MainSprites, DamagedSprites, TopSprites);

        Destroy(this.gameObject);



    }


}
