using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTurret_rocket : ShipTurret {

    public float CurrCooldown = 0;
    public float BaseCooldown = 1f;

    public Entity entity;

    public bool RelativeVelocity = false;
    public Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CurrCooldown > 0)
        {
            CurrCooldown -= Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.R))
        {
            TryShooting(Vector2.zero);
        }
    }

    public override void TryShooting(Vector2 Target)
    {
        if (CurrCooldown <= 0)
        {
            Shoot(Target);
            CurrCooldown = BaseCooldown;
        }
    }

    public void Shoot(Vector2 Target)
    {
        //bullet object, position, scale ...
        GameObject bullet = new GameObject();
        bullet.transform.position = transform.position;
        bullet.transform.localScale = new Vector3(1f, 1f, 1);

        GameObject BulletLightCircle = new GameObject();
        BulletLightCircle.transform.parent = bullet.transform;
        BulletLightCircle.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Fx/Goop256");
        BulletLightCircle.GetComponent<SpriteRenderer>().color = new Color(1, 0.6f, 0, 1);
        BulletLightCircle.transform.localPosition = new Vector3(0, 0, 0.1f);
        BulletLightCircle.transform.localScale = new Vector3(24, 24, 1);

        LightFlicker blf = BulletLightCircle.AddComponent<LightFlicker>();
        blf.AlphaMax = 1;
        blf.AlphaMin = 0.5f;
        blf.ColorA = new Color(1, 1, 0, 1);
        blf.ColorB = new Color(1, 0.8f, 0, 1);


        Util.RotateTransformToward(bullet.transform, Target);

        //bullet physics
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        if (RelativeVelocity)
        {
            bullet.GetComponent<Rigidbody2D>().velocity = rb.velocity;
        }
        else
        {
        }

        

        bullet.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        bullet.gameObject.AddComponent<CircleCollider2D>().isTrigger = true;
        bullet.GetComponent<CircleCollider2D>().radius = 3f;

        //bullet visuals
        bullet.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Fx/Goop256");
        bullet.GetComponent<SpriteRenderer>().color = new Color(1, 0.6f, 0, 0.5f);
        bullet.AddComponent<FxShipRailgunTrail>();


        if(ListOfEnemyShips.list.Count >= 1)
        {
            HomingRocket hr = bullet.AddComponent<HomingRocket>();
            hr.Target = ListOfEnemyShips.list[0];
        }

        //custom mechanics
        bullet.AddComponent<DamagerInflicter>().ini(entity.Team, 1, true);
        bullet.AddComponent<DieInSeconds>().Seconds = 10;

        bullet.AddComponent<FxSpaceExplosionCustom>().ini(new Color(1, 0.6f, 0f, 1), new Color(1, 0.6f, 0, 0f), 1.5f, 10, 1, 16);


        /*
        //muzzle flash (visual effect)
        GameObject Muzzleflash = new GameObject();
        Muzzleflash.transform.parent = UniversalReference.GunRotator.GunSpriteRenderer.transform;
        Muzzleflash.transform.localPosition = new Vector3(0, 0, 0); //split into two to set local position 0 0, ant then z -31 according to z-index protocol
        Muzzleflash.transform.position = new Vector3(Muzzleflash.transform.position.x, Muzzleflash.transform.position.y, -31);
        Muzzleflash.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        Muzzleflash.transform.localScale = new Vector3(1, 1, 1);
        Muzzleflash.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(MuzzleFlashSpritePath);
        Muzzleflash.GetComponent<SpriteRenderer>().color = BulletColorStart;
        Muzzleflash.AddComponent<DieIn>().Frames = 0; //original: 0
        */

        /*
        //light from muzzle flash
        if (PlayerBeingInDarknessManager.IsPlayerInDarkness)
        {
            GameObject MuzzleFlashLight = new GameObject();
            MuzzleFlashLight.AddComponent<SpriteMask>().sprite = Resources.Load<Sprite>("circle512");
            MuzzleFlashLight.transform.position = transform.position;
            MuzzleFlashLight.transform.localScale = new Vector3(1, 1, 1);
            MuzzleFlashLight.AddComponent<DieInFramesFramerateIndependent>().Frames = 2;
        }
        */

        /*
        //increase inaccuracy
        UniversalReference.PlayerScript.RequestInaccuracyIncrease(InaccuracyPerShot);
        if (UniversalReference.PlayerScript.InaccuracyRecoveryBlock < BaseCooldownBetweenShots)
        {
            UniversalReference.PlayerScript.InaccuracyRecoveryBlock += BaseCooldownBetweenShots;
        }
        */
    }
}
