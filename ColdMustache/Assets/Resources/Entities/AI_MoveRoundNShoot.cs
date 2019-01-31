using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_MoveRoundNShoot : AI_Base {


    Entity e;
    Transform target;
    Rigidbody2D targetRb;
    
    WaypointNavigator wn;
        
    public Weapon weapon;

    public GunRotatorHand gunRotatorHand;

    //the direction youre currently looking in - for some reason im not using the entity's LookingAt
    Vector2 AimDirection;

    //when you dont have anything to do, walk a bit and then turn in random direction and walk for random time in that direction
    float WalkTimeRemaining;

    //this is the longest possible reaction time - if player appears behind you (actually its 0.1f faster than stated, assuming i didnt change anything in the code)
    //if player appears in front of you, reaction is instant
    public float ReactionTime = 0.5f;

    //if player appeared behind you, you dont do anything for split second
    float CurrReactionTimeFreeze = 0;

    //if you just seen player in some direction, keep looking there for a while
    float DontChangeDirectionFor = 0; //curr
    float DontChangeDirectionForBase = 5; //base

    //if player often tries to outflank you by appearing behind your back to use the rection time, your patience gets reduced
    //if 0 --> hunting mode - your reaction time is increased and you will pathfind to player instead of walking, if you dont have anyhting to shoot at
    float BasePatience = 2;
    float CurrPatience = 2;
    bool HuntingMode = false;

    //if you see player but are outside of range, move toward him
    public float MaxDistanceToShootFrom = 15;

    // Use this for initialization
    void Start()
    {
        e = GetComponent<Entity>();
        target = UniversalReference.PlayerObject.transform;
        targetRb = target.GetComponent<Rigidbody2D>();
        wn = GetComponent<WaypointNavigator>();
        wn.WayPoints = new List<Vector2>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!HuntingMode)
        {
            if(CurrPatience <= 0)
            {
                AlphabetManager.SpawnFloatingText("I'm coming for you!", new Vector3(transform.position.x, transform.position.y, -35));
                HuntingMode = true;
            }
        }

        //if you are frozen because of your reaction time, return to skip all the rest
        if(CurrReactionTimeFreeze > 0)
        {
            CurrReactionTimeFreeze -= Time.deltaTime;
            return;
        }
        if(DontChangeDirectionFor > 0)
        {
            DontChangeDirectionFor -= Time.deltaTime;
        }
        
        Vector2Int MyPos = Util.Vector3To2Int(transform.position);
        Vector2Int TargetPos = Util.Vector3To2Int(target.position);

        //line of sight toward target (player) - null if obstructed
        List<Vector2Int> LineOfSight = NavTestStatic.GetLineOfSightOptimised(MyPos, TargetPos);
        
        //if you have a vision of player
        if (LineOfSight != null)
        {
            //if you have patience, freeze if hes coming from behind
            if(!HuntingMode)
            {
                CurrReactionTimeFreeze = Mathf.Abs(Vector2.SignedAngle(AimDirection, Util.GetVectorToward(transform, target))) / 180 * ReactionTime;
                //patience reduces more if player attacks from behind
                CurrPatience -= CurrReactionTimeFreeze;
                CurrReactionTimeFreeze -= 0.1f;
                DontChangeDirectionFor = DontChangeDirectionForBase;

                //patience reduces every time player is seen, and also for the duration enemy is reaction frozen                
            }

            
            //aim toward player
            AimDirection = Util.GetVectorToward(transform, target);

            //rotate gun toward player
            if (gunRotatorHand != null) gunRotatorHand.AimGun((Vector2)transform.position + AimDirection);

            //if frozen, stop
            if (CurrReactionTimeFreeze > 0)
            {
                return;
            }

            float DistanceToTarget = (transform.position - target.position).magnitude;

            if(DistanceToTarget < MaxDistanceToShootFrom)
            {
                e.StopMoving();
                //50-50 chance to either shoot at player current position
                if (Util.Coinflip())
                {
                    weapon.TryShooting(target.position);
                }
                //or predict his movement
                else
                {
                    weapon.TryShooting(
                        (Vector2)target.position +
                        (targetRb.velocity * (DistanceToTarget / 20))
                        );
                }
            }
            else
            {
                e.MoveInDirection(Util.GetVectorToward(transform, target));
            }

                      
        }

        //you dont see player
        else
        {
            //you are out of patience - pathfind toward him
            if (HuntingMode)
            {
                if(wn.WayPoints.Count <= 1)
                {
                    List<Vector2> path = NavTestStatic.FindAPath(MyPos, TargetPos);
                    if (path != null)
                    {
                        wn.WayPoints = path;
                    }
                }
                if(wn.WayPoints.Count != 0)
                {
                    AimDirection = Util.GetVectorToward(transform, wn.WayPoints[wn.WayPoints.Count - 1]);
                }

                if (gunRotatorHand != null)
                {
                    gunRotatorHand.AimGun(Util.RotateVector(Vector2.right, e.spriteManager.GetLastDirection()));
                }
            }

            //you still have patience - walk around normally
            else
            {
                if (WalkTimeRemaining >= 0)
                {
                    WalkTimeRemaining -= Time.deltaTime;
                    e.MoveInDirection(AimDirection);
                }
                else if (DontChangeDirectionFor <= 0)
                {
                    WalkTimeRemaining = Random.Range(0.5f, 5);
                    AimDirection = Util.RotateVector(Vector2.right, Random.Range(0, 360));
                }
                if (gunRotatorHand != null)
                {
                    gunRotatorHand.AimGun((Vector2)transform.position + AimDirection);
                }
            }

        }
        e.spriteManager.LookTowardAngle(Vector2.SignedAngle(Vector2.right, AimDirection));
        
    }    

}
