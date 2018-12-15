﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shotgun01 : Weapon {

    public int MaxAmmo = 2;
    public int Ammo = 2;

    public float BaseCooldownBetweenShots = 0.2f;
    public float CurrCooldownBetweenShots = 0;


    public float BaseReloadTime = 0.75f;
    public float ReloadTimeRemaining = 0;

    float SpecialValueThatIndicatesWeaponHasJustBeenReloaded = -999999;

    public string SpritePath = "Items/Weapons/Shotgun01";

    public int FramesSincePlayerRequestedShooting = 0; //for the purposes of semi-auto

    public int FramesSincePlayerRequestedAltFire = 0;


    public string AmmoSpriteSheetPath = "Gui/Hud/Ammo/Shotgun2";
    public Sprite[] AmmoSpriteSheet;

    private void Start()
    {
        ReloadTimeRemaining = SpecialValueThatIndicatesWeaponHasJustBeenReloaded; //without this, weapon gets max ammo in its first frame

        GetComponent<SpriteManagerGunner>().GunSpriteRenderer.sprite = Resources.Load<Sprite>(SpritePath);

        AmmoSpriteSheet = Resources.LoadAll<Sprite>(AmmoSpriteSheetPath);

        DisplayAmmo();

        GuiReference.WeaponStatus.sprite = Resources.Load<Sprite>("EmptyPixel");
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
            GuiReference.AmmoCounter.sprite = AmmoSpriteSheet[0];
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
        GameObject bullet = new GameObject();
        bullet.transform.position = transform.position;
        bullet.transform.localScale = new Vector3(0.1f, 0.1f, 1);
        bullet.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("pixel");
        bullet.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0);
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = Vector * 20;
        bullet.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        bullet.AddComponent<FxBulletTrail>();
        bullet.AddComponent<FxBulletExplosion>();

        bullet.AddComponent<DamagerInflicter>().ini(Entity.team.Player, 3, true);
        bullet.AddComponent<InflicterSlow>().ini(2, 0.5f, true);
        bullet.gameObject.name = "bullet";

        bullet.AddComponent<DieInSeconds>().Seconds = 5;
        //bullet.gameObject.tag = "PlayerBullet";
        bullet.gameObject.AddComponent<CircleCollider2D>().isTrigger = true;
        bullet.GetComponent<CircleCollider2D>().radius = 1.3f;
        //bullet.AddComponent<InflicterSlow>().ini(Entity.team.Enemy, 2, 0.25f);

        gameObject.AddComponent<StatusSlow>().ini(BaseCooldownBetweenShots, 0.5f, true);
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
        }
        DisplayAmmo();
    }

    public void DisplayAmmo()
    {
        if (Ammo == 0)
        {
            GuiReference.AmmoCounter.sprite = AmmoSpriteSheet[AmmoSpriteSheet.Length - 1];
        }
        else
        {
            GuiReference.AmmoCounter.sprite = AmmoSpriteSheet[AmmoSpriteSheet.Length - 1 - Mathf.RoundToInt(((float)Ammo / (float)MaxAmmo) * (float)(AmmoSpriteSheet.Length - 1))];
        }
    }
}