using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFire : MonoBehaviour
{

    //Age can be negative for delayed activation
    public float Age = 0;

    static float MaxAge = 10f;

    public float MaxSpreadDistance = 10;

    NavTestStatic.AvkisLightNode node = null;

    public Sprite[] sprites;

    SpriteRenderer sr;

    void Start()
    {
        if(!NavTestStatic.IsTileWalkable(Util.Vector3To2Int(transform.position))){
            Destroy(gameObject, 0.6f);
        }
    }

    void Update()
    {
        /*
        //spreading
        if(Age < AgeToSpreadAt && Age + Time.deltaTime > AgeToSpreadAt){    
            foreach (NavTestStatic.AvkisLightNode ChildNode in node.Children)
            {
                if(ChildNode.DistanceFromSource > MaxSpreadDistance){
                    continue;
                }

                Vector2Int ChildTile = Origin+ChildNode.Coordinates;
                if(!NavTestStatic.IsTileWithinBounds(ChildTile) || !NavTestStatic.CanExplosionPassThroughTile(ChildTile)){
                    continue;
                }
                GameObject go = new GameObject();
                go.transform.position = new Vector3(Origin.x+ChildNode.Coordinates.x, Origin.y+ChildNode.Coordinates.y, -10);
                Explosion_AvkisLight e = go.AddComponent<Explosion_AvkisLight>();
                e.Origin = Origin;
                e.sr = go.AddComponent<SpriteRenderer>();
                e.sprites = (Sprite[])sprites.Clone();
                e.node = ChildNode;

            }
            
        }
        */

        //delayed activation check
        //not doing this before spreading as that could cause problems on low framerate machines with explosions that spread quickly (in less than it takes to render a frame)
        if (Age < 0)
        {
            Age += Time.deltaTime;
            return;
        }

        Age += Time.deltaTime;
        if (Age > MaxAge)
        {
            Destroy(gameObject);
            return;
        }
        sr.sprite = sprites[Mathf.RoundToInt(Age / MaxAge * (float)(sprites.Length - 1))];
    }



    public static void SpawnOriginal(Vector2Int Tile, float MaxSpreadDistance)
    {
        foreach (Vector3 v in NavTestStatic.GetExplosionArea(Tile, MaxSpreadDistance))
        {
            GameObject go = new GameObject();
            go.transform.position = new Vector3(v.x, v.y, -10);
            ExplosionFire e = go.AddComponent<ExplosionFire>();
            e.Age = -v.z / 10;
            e.sr = go.AddComponent<SpriteRenderer>();
            e.sr.color = new Color(1,0,0,1);
            e.sprites = Resources.LoadAll<Sprite>("Fx/Explosion");
            e.node = NavTestStatic.AvkisLightNodes[0];
            e.MaxSpreadDistance = MaxSpreadDistance;
           
            go.AddComponent<BoxCollider2D>().isTrigger = true;
            go.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
            go.AddComponent<DamagerInflicterAoE>().ini(Entity.team.Neutral, 3, 0.4f, true, 1, -e.Age, DamagerInflicter.WeaponTypes.Fire);

            //spawning flames
            if(NavTestStatic.IsTileWalkable(Util.Vector3To2Int(v))){
                GameObject Flame = new GameObject();
                Flame.transform.position = new Vector3(v.x, v.y, -10);
                Flame.AddComponent<Fire>().Age = -v.z / 10;
                Flame.AddComponent<SpriteRenderer>();
            }
            
        }
    }
}
