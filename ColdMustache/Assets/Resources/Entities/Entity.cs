using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    public float MaxHealth = 10;
    public float Health = 10;
    public enum team {Player, Enemy, Neutral }
    public team Team = team.Neutral;
    public float BaseMoveSpeed = 5;
    public float MoveSpeedSlowModifier = 1;
    public float MoveSpeedBuffModifier = 1;

    public Vector2 LookingToward = new Vector2(1,0);

    Rigidbody2D rb;

    public bool UseSpriteManager = true;
    public SpriteManagerBase spriteManager = null;
    
    public bool LookDirectionBasedOnMovement = true;

    public bool IsPlayer = false;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        if(spriteManager == null && UseSpriteManager)
        {
            spriteManager = GetComponent<SpriteManagerBase>();
        }
	}
	
    public void MoveInDirection(Vector2 MovementVector)
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

    public void TakeDamage (float Damage, DamagerInflicter.WeaponTypes WeaponType = DamagerInflicter.WeaponTypes.Undefined)
    {
        //check invincibility, resistances and stuff
        if (true)
        {
            GameObject BloodSpatter = new GameObject();
            BloodSpatter.transform.position = new Vector3(transform.position.x, transform.position.y, 209);
            BloodSpatter.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            BloodSpatter.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Fx/BloodSpatter2");
            BloodSpatter.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
            

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
            }
        }
    }

    public void Die(DamagerInflicter.WeaponTypes WeaponType = DamagerInflicter.WeaponTypes.Undefined)
    {
        GetComponent<DeathAnimation>().Spawn(transform.position);
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

    /*
    public void MoveToward(Vector2 Target)
    {
        float CurrMoveSpeed = BaseMoveSpeed;

        
        //fixing some bug when computing/assigning vector 0,0
        if(MovementVector.x != 0 || MovementVector.y != 0)
        {
            //dividing by magnitude to make diagonal travel as fast as horizontal/vertical
            rb.velocity = MovementVector / MovementVector.magnitude * CurrMoveSpeed;
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
        
    }
    */

}
