﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFrag : MonoBehaviour
{
    
    //Age can be negative for delayed activation
    public float Age = 0;

    float MaxAge = 0.4f;

    float AgeToSpreadAt = 0.04f;

    public float MaxSpreadDistance = 10;

    Vector2Int Origin;

    NavTestStatic.AvkisLightNode node = null;

    public Sprite[] sprites;

    SpriteRenderer sr;

    void Start()
    {
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

        Age+=Time.deltaTime;
        if(Age>MaxAge){
            Destroy(gameObject);
            return;
        }
        sr.sprite = sprites[Mathf.RoundToInt(Age/MaxAge*(float)(sprites.Length-1))];
    }

    

    public static void SpawnOriginal(Vector2Int Tile, float MaxSpreadDistance, float Damage=10){
        
        string[] ExplosionAnimPaths = new string[]{
            "Fx/Explosions/64_01",
            "Fx/Explosions/64_02"

        };
        
        foreach(Vector3 v in NavTestStatic.GetExplosionArea(Tile, MaxSpreadDistance))
        {
            GameObject go = new GameObject();            
            go.transform.position = new Vector3(v.x, v.y, -10);
            go.transform.localScale = new Vector3(1.3f,1.3f,1);
            ExplosionFrag e = go.AddComponent<ExplosionFrag>();
            e.Origin = Tile;
            e.Age = -v.z/10;
            e.MaxAge = Random.Range(0.2f,0.6f);
            e.sr = go.AddComponent<SpriteRenderer>();
            e.sr.flipX = Util.Coinflip();
            e.sr.flipY = Util.Coinflip();
            e.sprites = Resources.LoadAll<Sprite>(ExplosionAnimPaths[Random.Range(0,ExplosionAnimPaths.Length)]);
            e.node = NavTestStatic.AvkisLightNodes[0];
            e.MaxSpreadDistance = MaxSpreadDistance;

            go.AddComponent<BoxCollider2D>().isTrigger = true;
            go.GetComponent<BoxCollider2D>().size = new Vector2(1,1);
            go.AddComponent<DamagerInflicterAoE>().ini(Entity.team.Neutral, Damage, 9999, true, 1, -e.Age, DamagerInflicter.WeaponTypes.Explosion  );


            go.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;


            go.AddComponent<DamagerInflicter>().ini(Entity.team.Player, Damage, false, true, 9999, -e.Age, true, DamagerInflicter.WeaponTypes.Explosion);
        }
    }
}
