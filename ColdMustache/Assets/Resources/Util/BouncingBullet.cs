using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBullet : MonoBehaviour
{

    public Vector3 LastPos;
    Vector3 DeltaPos;
    public Rigidbody2D rb;

    void Start()
    {
        LastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        //custom "bouncing" - normal colliders dont work, as then it would stop before holes instead of flying over
        //this custom bouncing checks if the nade is about to enter tile that blocks explosions
        //if so, it will flip its velocity on the axis on which it was about enter
        DeltaPos = transform.position - LastPos;
        LastPos = transform.position;

        if (!NavTestStatic.CanExplosionPassThroughTile(Util.Vector2To2Int(new Vector2(LastPos.x + DeltaPos.x, LastPos.y))))
        {
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        }
        if (!NavTestStatic.CanExplosionPassThroughTile(Util.Vector2To2Int(new Vector2(LastPos.x, LastPos.y + DeltaPos.y))))
        {
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
        }
    }
}
