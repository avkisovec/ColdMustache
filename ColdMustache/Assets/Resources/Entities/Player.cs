using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
    //after player shoots, he will be briefly stuck in aiming position (making him slower) (in seconds)
    float BaseShootingWindDownDuration = 0.3f;
    float CurrShootingWindDownDuration = 0;
    //duration between shots (in seconds)
    float BaseShootingCooldown = 0.2f;
    float CurrShootingCooldown = 0;

    Entity entity;

    SpriteManagerGunner smg;

	// Use this for initialization
	void Start () {
        entity = GetComponent<Entity>();
        smg = GetComponent<SpriteManagerGunner>();
	}
	
	// Update is called once per frame
	void Update () {
        

        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        entity.LookingToward = MouseWorldPos;
        


        //decreasing active cooldowns
        if(CurrShootingWindDownDuration > 0)
        {
            //when decreasing cooldowns, always use Time.deltaTime (the time it took to render last frame, in seconds) - framerate independent
            CurrShootingWindDownDuration -= Time.deltaTime;
        }
        if(CurrShootingCooldown > 0)
        {
            CurrShootingCooldown -= Time.deltaTime;
        }
        
        //when not firing/aiming, hold the gun in resting position
        if(Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1) || CurrShootingWindDownDuration > 0)
        {
            smg.ActivelyAiming = true;
        }
        else
        {
            smg.ActivelyAiming = false;
        }
        

        //movement vector will hold information about direction, speed is added after
        Vector2 MovementVector = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            MovementVector += new Vector2(0,1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            MovementVector += new Vector2(0,-1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            MovementVector += new Vector2(-1,0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            MovementVector += new Vector2(1, 0);
        }

        entity.MoveInDirection(MovementVector);



        //test purposes - can delete
        

        if (Input.GetKeyUp(KeyCode.Mouse2))
        {
            GameObject laser = new GameObject();
            laser.AddComponent<SpriteRenderer>();
            Laser l = laser.AddComponent<Laser>();
            l.Origin = transform.position;
            l.End = MouseWorldPos;

            SpriteSheetAnimation lanim = laser.AddComponent<SpriteSheetAnimation>();
            lanim.Sprites = Resources.LoadAll<Sprite>("Fx/LaserCharged_57frames2"); //for this animation, damage should start at 70% 
            lanim.LifeSpanInSeconds = 1f;
            lanim.Mode = SpriteSheetAnimation.Modes.Destroy;

            GameObject dmg = new GameObject();
            dmg.transform.parent = laser.transform;
            dmg.transform.localScale = new Vector3(1f/32f, 1, 1);
            //dmg.AddComponent<InflicterSlow>();
            dmg.AddComponent<DamagerInflicter>().ini(Entity.team.Player, 1, false, true, 1, 0.7f);
            dmg.AddComponent<BoxCollider2D>().isTrigger = true;
            dmg.AddComponent<FxBurnSmoke>();
            dmg.AddComponent<WiggleNonNoticably>();
        }


        if (Input.GetKeyUp(KeyCode.B)) {

            foreach (Vector3 v in GameObject.Find("ScriptHolder").GetComponent<NavTest>().CalculateExplosion(
                Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x),
                Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y),
                20
                )){

                GameObject blast = new GameObject();
                blast.transform.position = new Vector3(v.x, v.y, -5);
                blast.AddComponent<SpriteRenderer>();

                SpriteSheetAnimation banim = blast.AddComponent<SpriteSheetAnimation>();
                banim.Sprites = Resources.LoadAll<Sprite>("Fx/Explosion"); 
                banim.LifeSpanInSeconds = 1f;
                banim.Mode = SpriteSheetAnimation.Modes.Destroy;

                blast.GetComponent<SpriteRenderer>().color = new Color(1,v.z/10,0);
            }

        }
        if (Input.GetKeyUp(KeyCode.N))
        {

            foreach (Vector3 v in GameObject.Find("ScriptHolder").GetComponent<NavTest>().CalculateExplosion_Distributor(
                Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x),
                Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y),
                100
                ))
            {

                GameObject blast = new GameObject();
                blast.transform.position = new Vector3(v.x, v.y, -5);
                blast.AddComponent<SpriteRenderer>();

                SpriteSheetAnimation banim = blast.AddComponent<SpriteSheetAnimation>();
                banim.Sprites = Resources.LoadAll<Sprite>("Fx/Explosion");
                banim.LifeSpanInSeconds = 2.5f;
                banim.Mode = SpriteSheetAnimation.Modes.Destroy;

                blast.GetComponent<SpriteRenderer>().color = new Color(1, v.z / 50, 0);
                blast.name = v.z.ToString();
            }

        }

        //end of test stuff


        if (Input.GetKey(KeyCode.Mouse0) && CurrShootingCooldown <= 0)
        {
            CurrShootingWindDownDuration = BaseShootingWindDownDuration;
            Shoot(MouseWorldPos);

            CurrShootingCooldown = BaseShootingCooldown;

            gameObject.AddComponent<StatusSlow>().ini(BaseShootingCooldown, 0.5f, false);  //slow yourself for the duration of the cooldown
        }

    }

    void Shoot(Vector2 Target)
    {
        GameObject bullet = new GameObject();
        bullet.transform.position = transform.position;
        bullet.transform.localScale = new Vector3(0.1f, 0.1f, 1);
        bullet.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("pixel");
        bullet.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0);
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = Util.GetDirectionVectorToward(transform, Target) * 20;
        bullet.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        bullet.AddComponent<FxBulletTrail>();
        bullet.AddComponent<FxBulletExplosion>();

        bullet.AddComponent<DamagerInflicter>().ini(Entity.team.Player, 1, true);
        bullet.AddComponent<InflicterSlow>().ini(2,0.5f,true);
        bullet.gameObject.name = "bullet";

        bullet.AddComponent<DieInSeconds>().Seconds = 5;
        //bullet.gameObject.tag = "PlayerBullet";
        bullet.gameObject.AddComponent<CircleCollider2D>().isTrigger = true;
        //bullet.AddComponent<InflicterSlow>().ini(Entity.team.Enemy, 2, 0.25f);
    }
}
