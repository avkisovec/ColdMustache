using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLegacy : MonoBehaviour {

    public float BaseMoveSpeed = 5;

    float AimingMoveSpeedModifier = 0.25f;

    SpriteManagerGunner spriteManager;

    Rigidbody2D rb;

    //after player shoots, he will be briefly stuck in aiming position (making him slower) (in seconds)
    float BaseShootingWindDownDuration = 0.3f;
    float CurrShootingWindDownDuration = 0;

    //duration between shots (in seconds)
    float BaseShootingCooldown = 0.2f;
    float CurrShootingCooldown = 0;

    // Use this for initialization
    void Start()
    {
        spriteManager = GetComponent<SpriteManagerGunner>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float CurrMoveSpeed = BaseMoveSpeed;

        //decreasing active cooldowns
        if (CurrShootingWindDownDuration > 0)
        {
            //when decreasing cooldowns, always use Time.deltaTime (the time it took to render last frame, in seconds) - framerate independent
            CurrShootingWindDownDuration -= Time.deltaTime;
        }
        if (CurrShootingCooldown > 0)
        {
            CurrShootingCooldown -= Time.deltaTime;
        }

        //rotate player toward mouse
        spriteManager.LookAt(MouseWorldPos);


        //when not firing/aiming, hold the gun in resting position
        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1) || CurrShootingWindDownDuration > 0)
        {
            spriteManager.AimGun(MouseWorldPos);
            CurrMoveSpeed *= AimingMoveSpeedModifier;
        }
        else
        {
            spriteManager.HoldGun(MouseWorldPos);
        }

        //movement vector will hold information about direction, speed is added after
        Vector2 MovementVector = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            MovementVector += new Vector2(0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            MovementVector += new Vector2(0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            MovementVector += new Vector2(-1, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            MovementVector += new Vector2(1, 0);
        }

        //fixing some bug when computing/assigning vector 0,0
        if (MovementVector.x != 0 || MovementVector.y != 0)
        {
            //dividing by magnitude to make diagonal travel as fast as horizontal/vertical
            rb.velocity = MovementVector / MovementVector.magnitude * CurrMoveSpeed;
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }

        if (Input.GetKey(KeyCode.Mouse0) && CurrShootingCooldown <= 0)
        {
            CurrShootingWindDownDuration = BaseShootingWindDownDuration;
            Shoot(MouseWorldPos);

            CurrShootingCooldown = BaseShootingCooldown;
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
        bullet.AddComponent<Bullet>();
        bullet.AddComponent<DieInSeconds>().Seconds = 5;
        bullet.gameObject.tag = "PlayerBullet";
        bullet.gameObject.AddComponent<CircleCollider2D>().isTrigger = true;
    }
}
