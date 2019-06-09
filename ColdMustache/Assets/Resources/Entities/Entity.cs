using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    public int UniqueId = -1;
    public float MaxHealth = 10;
    public float Health = 10;
    public enum team {Player, Enemy, Neutral }
    public team Team = team.Neutral;
    public float BaseMoveSpeed = 5;
    public float MoveSpeedSlowModifier = 1;
    public float MoveSpeedBuffModifier = 1;

    public Vector2 LookingToward = new Vector2(1,0);

    protected Rigidbody2D rb;

    public bool UseSpriteManager = true;
    public SpriteManagerBase spriteManager = null;
    
    public bool LookDirectionBasedOnMovement = true;

    public bool IsPlayer = false;

    // Use this for initialization
    void Start () {
        UniqueId = EntityIdDistributor.GetUniqueId();
        rb = GetComponent<Rigidbody2D>();
        if(spriteManager == null && UseSpriteManager)
        {
            spriteManager = GetComponent<SpriteManagerBase>();
        }
	}
	
    public virtual void MoveInDirection(Vector2 MovementVector)
    {
        //fixing some bug when computing/assigning vector 0,0
        if (MovementVector.x != 0 || MovementVector.y != 0)
        {
            //dividing by magnitude to make diagonal travel as fast as horizontal/vertical
            rb.velocity = MovementVector / MovementVector.magnitude * BaseMoveSpeed * MoveSpeedSlowModifier * MoveSpeedBuffModifier;

            if (LookDirectionBasedOnMovement)
            {
                LookingToward = (Vector2)transform.position + MovementVector;
            }
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }


        MoveSpeedSlowModifier = 1;
        MoveSpeedBuffModifier = 1;
    }

    public void StopMoving()
    {
        rb.velocity = new Vector2(0, 0);
    }

    public virtual void TakeDamage (float Damage, DamagerInflicter.WeaponTypes WeaponType = DamagerInflicter.WeaponTypes.Undefined)
    {
        //check invincibility, resistances and stuff
        if (true)
        {
            BloodSpatterSpawner.SpawnSmall(transform.position);
            

            Health -= Damage;

            if (UseSpriteManager)
            {
                spriteManager.TemporaryColor(new Color(1, 0, 0), 0.5f);
            }

            if (IsPlayer)
            {
                ScreenFx.InjuryScreen();
            }

            if(Health <= 0)
            {
                Die(WeaponType);

                BloodSpatterSpawner.SpawnPool(transform.position);

            }
        }
    }

    public virtual void Die(DamagerInflicter.WeaponTypes WeaponType = DamagerInflicter.WeaponTypes.Undefined)
    {
        DeathAnimation da = GetComponent<DeathAnimation>();
        if(da != null) da.Spawn(transform.position);

        if (IsPlayer)
        {
            ScreenFx.DeathScreen(WeaponType);

            //as other scripts often reference player, destroying him would cause issues, so im just moving him away from any danger, hopefully
            //if id just kept him where he "dies", enemies would still attack him which would mess with cursor
            transform.position = new Vector3(NavTestStatic.MapWidth-1, NavTestStatic.MapHeight-1, 0);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

}
