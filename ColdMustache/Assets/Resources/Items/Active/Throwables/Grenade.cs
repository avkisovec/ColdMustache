using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    public enum GrenadeTypes {Frag, Fire}
    public GrenadeTypes GrenadeType = GrenadeTypes.Frag;

    public float Delay = 3;
    public float ExplosionRadius = 5;
    float Age = 0;

    public Vector3 LastPos;
    Vector3 DeltaPos;
    public Rigidbody2D rb;

    

    void Update()
    {


        //custom "bouncing" - normal colliders dont work, as then it would stop before holes instead of flying over
        //this custom bouncing checks if the nade is about to enter tile that blocks explosions
        //if so, it will flip its velocity on the axis on which it was about enter
        DeltaPos = transform.position - LastPos;
        LastPos = transform.position;

        if(!NavTestStatic.CanExplosionPassThroughTile(Util.Vector2To2Int(new Vector2(LastPos.x+DeltaPos.x, LastPos.y))))
        {
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        }
        if (!NavTestStatic.CanExplosionPassThroughTile(Util.Vector2To2Int(new Vector2(LastPos.x, LastPos.y+DeltaPos.y))))
        {
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
        }



        Age += Time.deltaTime;
        if(Age > Delay){
            if(GrenadeType == GrenadeTypes.Frag){
                ExplosionFrag.SpawnOriginal(Util.Vector3To2Int(transform.position), 5);
            }
            else if(GrenadeType == GrenadeTypes.Fire){
                ExplosionFire.SpawnOriginal(Util.Vector3To2Int(transform.position), 3);
            }
            
            Destroy(gameObject);
        }
        
    }
}
