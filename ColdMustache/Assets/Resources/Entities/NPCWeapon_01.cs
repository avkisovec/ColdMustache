using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWeapon_01 : Weapon {


    //general things - ammo, cooldown, reload, range

    public int MaxAmmo = 10;
    public int Ammo = 10;

    public float BaseCooldownBetweenShots = 0.1f;
    public float CurrCooldownBetweenShots = 0;

    float BaseReloadTime = 4f;
    float ReloadTimeRemaining = 0;

    public float EffectiveRange = 10;

    public float DamagePerBullet = 1;

    public float Inaccuracy = 1f;

    public float BulletVelocity = 20;

    //visual things - sprites, icons, colors

    public Sprite sprite = null;

    public Color BulletColorStart = new Color(1f, 1f, 0.5f, 1);
    public Color BulletColorEnd = new Color(0.5f, 0, 0, 1);

    public Entity entity;
    
    //backend stuff

    float SpecialValueThatIndicatesWeaponHasJustBeenReloaded = -999999;
        
    private void Update()
    {
        if (ReloadTimeRemaining > 0)
        {
            ReloadTimeRemaining -= Time.deltaTime;
        }
        else if (ReloadTimeRemaining != SpecialValueThatIndicatesWeaponHasJustBeenReloaded) //special value, indicates that weapon has just reloaded and reloading should be ignored
        {
            Ammo = MaxAmmo;
            ReloadTimeRemaining = SpecialValueThatIndicatesWeaponHasJustBeenReloaded;
        }
        else if (Ammo <= 0)
        {
            ForceReload();
        }

        if (CurrCooldownBetweenShots > 0)
        {
            CurrCooldownBetweenShots -= Time.deltaTime;
        }     
        
    }

    public override void TryShooting(Vector3 Target)
    {
        if (Ammo > 0 && CurrCooldownBetweenShots <= 0)
        {
            Ammo--;
            CurrCooldownBetweenShots = BaseCooldownBetweenShots;
            
            //bullet object, position, scale ...
            GameObject bullet = new GameObject();
            bullet.transform.position = transform.position;
            bullet.transform.localScale = new Vector3(0.1f, 0.1f, 1);
            bullet.gameObject.name = "bullet";

            //bullet physics
            bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
            bullet.GetComponent<Rigidbody2D>().velocity = Util.RotateVector(Util.GetDirectionVectorToward(transform, Target), Random.Range(-Inaccuracy, Inaccuracy)) * BulletVelocity;
            bullet.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            bullet.gameObject.AddComponent<CircleCollider2D>().isTrigger = true;
            bullet.GetComponent<CircleCollider2D>().radius = 1.3f;

            //bullet visuals
            bullet.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("pixel");
            bullet.GetComponent<SpriteRenderer>().color = BulletColorStart;
            bullet.AddComponent<FxBulletTrailCustom>().ini(BulletColorStart, BulletColorEnd);
            bullet.AddComponent<FxBulletExplosionCustom>().ini(BulletColorStart, BulletColorEnd);

            //custom mechanics
            bullet.AddComponent<DamagerInflicter>().ini(entity.Team, DamagePerBullet, true);
            bullet.AddComponent<DieInSeconds>().Seconds = 5;
            bullet.AddComponent<SlowlySlowDown>().EffectiveRange = EffectiveRange;
            bullet.AddComponent<InflicterSlow>().ini(2, 0.5f, true);
                        
        }
    }
    
    public override void ForceReload()
    {
        if (Ammo != MaxAmmo && ReloadTimeRemaining <= 0)
        {
            Ammo = 0;

            ReloadTimeRemaining = BaseReloadTime;
                        
            AlphabetManager.SpawnFloatingText("Reloading", new Vector3(transform.position.x, transform.position.y, -35));

        }
    }    
}
