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

    public Vector2 LookingToward = new Vector2(1,0);

    Rigidbody2D rb;

    public bool UseSpriteManager = true;
    public SpriteManagerBase AnySprtMng = null;
    
    public bool LookDirectionBasedOnMovement = true;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        if(AnySprtMng == null && UseSpriteManager)
        {
            AnySprtMng = GetComponent<SpriteManagerBase>();
        }
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
            rb.velocity = MovementVector / MovementVector.magnitude * BaseMoveSpeed * MoveSpeedSlowModifier;

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
    }

    public void TakeDamage (float Damage)
    {
        //check invincibility, resistances and stuff
        if (true)
        {


            GameObject BloodSpatter = new GameObject();
            BloodSpatter.transform.position = new Vector3(transform.position.x, transform.position.y, 209);
            BloodSpatter.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            BloodSpatter.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Fx/BloodSpatter");



            Health -= Damage;

            if (UseSpriteManager)
            {
                AnySprtMng.TemporaryColor(new Color(1, 0, 0), 0.5f);
            }

            if(Health <= 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
        GetComponent<DeathAnimation>().Spawn(transform.position);
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
