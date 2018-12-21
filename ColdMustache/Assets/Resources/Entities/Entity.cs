using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    public float MaxHealth = 10;
    public float Health = 10;
    public enum team {Player, Enemy, Neutral }
    public team Team = team.Neutral;
    public float BaseMoveSpeed = 5;
    public float CurrMoveSpeed = 5;

    public Vector2 LookingToward = new Vector2(1,0);

    Rigidbody2D rb;

    SpriteManager AnySprtMng;

    public bool LookDirectionBasedOnMovement = true;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        AnySprtMng = GetComponent<SpriteManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MoveInDirection(Vector2 MovementVector)
    {
        //fixing some bug when computing/assigning vector 0,0
        if (MovementVector.x != 0 || MovementVector.y != 0)
        {
            //dividing by magnitude to make diagonal travel as fast as horizontal/vertical
            rb.velocity = MovementVector / MovementVector.magnitude * CurrMoveSpeed;

            if (LookDirectionBasedOnMovement)
            {
                LookingToward = (Vector2)transform.position + MovementVector;
            }
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }


        CurrMoveSpeed = BaseMoveSpeed;
    }

    public void TakeDamage (int Damage)
    {
        //check invincibility, resistances and stuff
        if (true)
        {
            Health -= Damage;
            AnySprtMng.FlashInjuredColor();
            if(Health <= 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        GetComponent<DeathAnimation>().Spawn(transform.position);
        Destroy(this.gameObject);
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
