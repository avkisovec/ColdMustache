using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntPlayerVisually : MonoBehaviour {

    //if you have line of sight with player - go to him; if not, go to last known location

    public enum EnemyMode { NoTarget, VisibleTarget, TargetDisappeared };

    public Transform Player;

    public EnemyMode Mode = EnemyMode.NoTarget;

    LineOfSight sightline;

    bool WasLineClearPreviousFrame = false;

    Vector2 PlayerLastKnownPos = new Vector2(0, 0);

    int BaseShootCooldown = 60;
    int CurrShootCooldown = 0;

    Rigidbody2D rb;

    Entity e;

    // Use this for initialization
    void Start()
    {

        GameObject go = new GameObject();
        go.AddComponent<LineOfSight>().Source = transform;
        go.GetComponent<LineOfSight>().Target = Player;
        sightline = go.GetComponent<LineOfSight>();
        rb = GetComponent<Rigidbody2D>();

        e = GetComponent<Entity>();

    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("LastKnownPos").transform.position = PlayerLastKnownPos;

        if (CurrShootCooldown > 0)
        {
            CurrShootCooldown--;
        }

        if (sightline.IsLineClear)
        {
            WasLineClearPreviousFrame = true;
            PlayerLastKnownPos = Player.position;
            Mode = EnemyMode.VisibleTarget;
            
            e.MoveInDirection(Util.GetDirectionVectorToward(transform, PlayerLastKnownPos));
            
        }
        else if (!sightline.IsLineClear && WasLineClearPreviousFrame)
        {
            //enemy just disappeared in this frame
            Mode = EnemyMode.TargetDisappeared;

            e.MoveInDirection(Util.GetDirectionVectorToward(transform, PlayerLastKnownPos));
            WasLineClearPreviousFrame = false;
        }
        else if(Mode == EnemyMode.TargetDisappeared)
        {
            if (((Vector2)transform.position - PlayerLastKnownPos).magnitude > 0.3f)
            {
                e.MoveInDirection(Util.GetDirectionVectorToward(transform, PlayerLastKnownPos));
            }
            else
            {
                e.MoveInDirection(new Vector2(0,0));
            }
        }
    

        /*
        if (Mode == EnemyMode.TargetDisappeared && rb.velocity.magnitude < 0.1f)
        {
        }
        */
        if (Mode == EnemyMode.NoTarget)
        {

        }
        else if (Mode == EnemyMode.VisibleTarget)
        {
            //ShootIfPossible();
        }
        else if (Mode == EnemyMode.TargetDisappeared)
        {
            //ShootIfPossible();
        }



    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            rb.velocity = new Vector2(0, 0);
            rb.angularVelocity = 0;
        }
    }

}
