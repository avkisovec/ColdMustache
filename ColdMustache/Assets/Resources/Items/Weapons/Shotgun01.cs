using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shotgun01 : Weapon {

    //general things - ammo, cooldown, reload, range

    public int MaxAmmo = 2;
    public int Ammo = 2;

    public float BaseCooldownBetweenShots = 0.2f;
    public float CurrCooldownBetweenShots = 0;

    public float BaseReloadTime = 0.75f;
    public float ReloadTimeRemaining = 0;

    public float EffectiveRange = 30;
    public float InaccuracyPerShot = 4f;

    public float DamagePerBullet = 1;

    //visual things - sprites, icons, colors

    public Sprite sprite = null;
    public string SpritePath = "Items/Weapons/Shotgun01";

    public string AmmoSpriteSheetPath = "Gui/Hud/Ammo/Shotgun2";
    public Sprite[] AmmoSpriteSheet;

    public Color BulletColorStart = new Color(1f, 1f, 0.5f, 1);
    public Color BulletColorEnd = new Color(0.5f, 0, 0, 1);

    public string MuzzleFlashSpritePath = "Fx/SmgMuzzleFlash";

    //alt fire

    //backend stuff

    float SpecialValueThatIndicatesWeaponHasJustBeenReloaded = -999999;
    
    public int FramesSincePlayerRequestedShooting = 0; //for the purposes of semi-auto
    public int FramesSincePlayerRequestedAltFire = 0;
    


    private void Start()
    {
        //default values
        ReloadTimeRemaining = SpecialValueThatIndicatesWeaponHasJustBeenReloaded; //without this, weapon gets max ammo in its first frame
                                                                                  
        //sprites
        if (sprite != null)
        {
            UniversalReference.GunRotator.GunSpriteRenderer.sprite = sprite;
        }
        else
        {
            UniversalReference.GunRotator.GunSpriteRenderer.sprite = Resources.Load<Sprite>(SpritePath);
        }

        //ammo
        AmmoSpriteSheet = Resources.LoadAll<Sprite>(AmmoSpriteSheetPath);

        DisplayAmmo();

        //status    
        UniversalReference.WeaponStatus.sprite = Resources.Load<Sprite>("EmptyPixel");

        //crosshair
        UniversalReference.crosshair.EffectiveRange = EffectiveRange;
    }

    private void Update()
    {
        if (ReloadTimeRemaining > 0)
        {
            ReloadTimeRemaining -= Time.deltaTime;
        }
        else if (ReloadTimeRemaining != SpecialValueThatIndicatesWeaponHasJustBeenReloaded) //special value, indicates that weapon has just reloaded and reloading should be ignored
        {
            Ammo = MaxAmmo;
            UniversalReference.AmmoCounter.sprite = AmmoSpriteSheet[0];
            ReloadTimeRemaining = SpecialValueThatIndicatesWeaponHasJustBeenReloaded;
        }

        if (CurrCooldownBetweenShots > 0)
        {
            CurrCooldownBetweenShots -= Time.deltaTime;
        }

        if (FramesSincePlayerRequestedShooting < 1000)
        {
            FramesSincePlayerRequestedShooting++;
        }
        if (FramesSincePlayerRequestedAltFire < 1000)
        {
            FramesSincePlayerRequestedAltFire++;
        }
    }

    public override void TryShooting(Vector3 Target)
    {
        if (Ammo > 0 && CurrCooldownBetweenShots <= 0)
        {
            if (FramesSincePlayerRequestedShooting >= 2)
            {
                Ammo--;
                CurrCooldownBetweenShots = BaseCooldownBetweenShots;

                Vector3 Vector = Util.GetDirectionVectorToward(transform, Target);

                ShootProjectile(Util.RotateVector(Vector, 1.5f));
                ShootProjectile(Util.RotateVector(Vector, 4.5f));
                ShootProjectile(Util.RotateVector(Vector, -1.5f));
                ShootProjectile(Util.RotateVector(Vector, -4.5f));
            }
        }

        FramesSincePlayerRequestedShooting = 0;
        
        DisplayAmmo();
    }

    public void ShootProjectile(Vector3 Vector)
    {
        Vector3 Origin = new Vector3(UniversalReference.PlayerBulletsOrigin.position.x, UniversalReference.PlayerBulletsOrigin.position.y, -31);

        //bullet object, position, scale ...
        GameObject bullet = new GameObject();
        bullet.transform.position = Origin;
        bullet.transform.localScale = new Vector3(0.1f, 0.1f, 1);
        bullet.gameObject.name = "bullet";

        //bullet physics
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = Vector * 20;
        bullet.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        bullet.AddComponent<CircleCollider2D>().isTrigger = true;
        bullet.GetComponent<CircleCollider2D>().radius = 1.3f;

        //bullet visuals
        bullet.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("pixel");
        bullet.GetComponent<SpriteRenderer>().color = BulletColorStart;
        bullet.AddComponent<FxBulletTrailCustom>().ini(BulletColorStart, BulletColorEnd);
        bullet.AddComponent<FxBulletExplosionCustom>().ini(BulletColorStart, BulletColorEnd);

        //custom mechanics
        bullet.AddComponent<DamagerInflicter>().ini(Entity.team.Player, DamagePerBullet, true);
        bullet.AddComponent<DieInSeconds>().Seconds = 5;
        bullet.AddComponent<SlowlySlowDown>().EffectiveRange = EffectiveRange;
        bullet.AddComponent<InflicterSlow>().ini(2, 0.5f, true);


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

        //light from muzzle flash
        if (PlayerBeingInDarknessManager.IsPlayerInDarkness)
        {
            GameObject MuzzleFlashLight = new GameObject();
            MuzzleFlashLight.AddComponent<SpriteMask>().sprite = Resources.Load<Sprite>("circle512");
            MuzzleFlashLight.transform.position = transform.position;
            MuzzleFlashLight.transform.localScale = new Vector3(1, 1, 1);
            MuzzleFlashLight.AddComponent<DieInFramesFramerateIndependent>().Frames = 2;
        }

        //increase inaccuracy
        UniversalReference.PlayerScript.RequestInaccuracyIncrease(InaccuracyPerShot);
        if (UniversalReference.PlayerScript.InaccuracyRecoveryBlock < BaseCooldownBetweenShots)
        {
            UniversalReference.PlayerScript.InaccuracyRecoveryBlock += BaseCooldownBetweenShots;
        }

        //display chambering ammo status if applicable
        AmmoStatus.NowChambering(BaseCooldownBetweenShots); 



    }

    public override void TryAltFire(Vector3 Target)
    {
        if(Ammo == 2)
        {
            Ammo = 0;
            CurrCooldownBetweenShots = BaseCooldownBetweenShots;

            Vector3 Vector = Util.GetDirectionVectorToward(transform, Target);

            ShootProjectile(Util.RotateVector(Vector, 1));
            ShootProjectile(Util.RotateVector(Vector, 4));
            ShootProjectile(Util.RotateVector(Vector, 7));
            ShootProjectile(Util.RotateVector(Vector, 10));
            ShootProjectile(Util.RotateVector(Vector, -1));
            ShootProjectile(Util.RotateVector(Vector, -4));
            ShootProjectile(Util.RotateVector(Vector, -7));
            ShootProjectile(Util.RotateVector(Vector, -10));


            DisplayAmmo();
            return;
        }
        if(Ammo == 1)
        {
            TryShooting(Target);
            return;
        }
    }

    public override void ForceReload()
    {
        if (Ammo != MaxAmmo && ReloadTimeRemaining <= 0)
        {
            Ammo = 0;

            ReloadTimeRemaining = BaseReloadTime;

            gameObject.AddComponent<StatusSlow>().ini(BaseReloadTime, 0.25f, true);

            AmmoStatus.NowReloading(BaseReloadTime);
        }
        DisplayAmmo();
    }

    public void DisplayAmmo()
    {
        if (Ammo == 0)
        {
            UniversalReference.AmmoCounter.sprite = AmmoSpriteSheet[AmmoSpriteSheet.Length - 1];
            AmmoStatus.EmptyMag();
        }
        else
        {
            UniversalReference.AmmoCounter.sprite = AmmoSpriteSheet[AmmoSpriteSheet.Length - 1 - Mathf.RoundToInt(((float)Ammo / (float)MaxAmmo) * (float)(AmmoSpriteSheet.Length - 1))];
        }
    }
}
