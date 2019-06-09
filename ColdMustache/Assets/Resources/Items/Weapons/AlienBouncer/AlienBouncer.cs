﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBouncer : Weapon
{
    //general things - ammo, cooldown, reload, range

    public int MaxAmmo = 4;
    public int Ammo = 4;

    public float BaseCooldownBetweenShots = 0.1f;
    public float CurrCooldownBetweenShots = 0;

    public float BaseReloadTime = 1f;
    public float ReloadTimeRemaining = 0;

    public float EffectiveRange = 10;
    public float InaccuracyPerShot = 1.2f;

    public float DamagePerBullet = 1;

    public float InaccuracyBase = 1.5f;
    public float InaccuracyImportance = 3f;

    public float ProjectileSpeed = 20;

    //visual things - sprites, icons, colors

    public Sprite sprite = null;

    public Color BulletColorStart = new Color(1f, 1f, 0.5f, 1);
    public Color BulletColorEnd = new Color(0.5f, 0, 0, 1);

    public string AmmoSpriteSheetPath = "Gui/Hud/Ammo/Bullets10";
    public Sprite[] AmmoSpriteSheet;

    public string StatusFullPath = "Gui/Hud/WeaponStatus/ModeFull";
    public Sprite StatusFullSprite;
    public string StatusSemiPath = "Gui/Hud/WeaponStatus/ModeSemi";
    public Sprite StatusSemiSprite;

    public string MuzzleFlashSpritePath = "Fx/SmgMuzzleFlash";

    //alt fire

    //backend stuff

    float SpecialValueThatIndicatesWeaponHasJustBeenReloaded = -999999;

    public int FramesSincePlayerRequestedShooting = 0; //for the purposes of semi-auto
    public int FramesSincePlayerRequestedAltFire = 0;


    public override void OnBecomingActive()
    {
        //default values
        ReloadTimeRemaining = SpecialValueThatIndicatesWeaponHasJustBeenReloaded; //without this, weapon gets max ammo in its first frame

        //sprites
        UniversalReference.GunRotator.GunSpriteRenderer.sprite = sprite;

        //ammo
        AmmoSpriteSheet = Resources.LoadAll<Sprite>(AmmoSpriteSheetPath);

        DisplayAmmo();

        //status
        StatusFullSprite = Resources.Load<Sprite>(StatusFullPath);
        StatusSemiSprite = Resources.Load<Sprite>(StatusSemiPath);

        AmmoStatus.SetEmptySpriteAndNormalColor();

        DisplayCorrectStatusImage();

        //crosshair
        UniversalReference.crosshair.EffectiveRange = EffectiveRange;
    }

    private void Update()
    {
        if (CurrentlyActive)
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
    }

    public override void TryShooting(Vector3 Target)
    {
        if (Ammo > 0 && CurrCooldownBetweenShots <= 0)
        {
            if (FramesSincePlayerRequestedShooting >= 2) //semi-auto
            {
                Ammo--;
                CurrCooldownBetweenShots = BaseCooldownBetweenShots;

                Vector3 Origin = new Vector3(UniversalReference.PlayerBulletsOrigin.position.x, UniversalReference.PlayerBulletsOrigin.position.y, -31);

                //bullet object, position, scale ...
                GameObject bullet = new GameObject();
                bullet.transform.position = Origin;
                bullet.transform.localScale = new Vector3(0.1f, 0.1f, 1);
                bullet.gameObject.name = "bullet";

                //bullet physics
                bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                float Inaccuracy = InaccuracyBase + InaccuracyImportance * UniversalReference.PlayerScript.Inaccuracy;
                bullet.GetComponent<Rigidbody2D>().velocity = Util.RotateVector(Util.GetDirectionVectorToward(UniversalReference.PlayerObject.transform, Target), Random.Range(-Inaccuracy, Inaccuracy)) * ProjectileSpeed;
                bullet.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                bullet.gameObject.AddComponent<CircleCollider2D>().isTrigger = true;
                bullet.GetComponent<CircleCollider2D>().radius = 1.3f;

                //bullet visuals
                bullet.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("pixel");
                bullet.GetComponent<SpriteRenderer>().color = BulletColorStart;
                bullet.AddComponent<FxBulletTrailCustom>().ini(BulletColorStart, BulletColorEnd);
                bullet.AddComponent<FxBulletExplosionCustom>().ini(BulletColorStart, BulletColorEnd);

                //custom mechanics
                bullet.AddComponent<DamagerInflicter>().ini(Entity.team.Player, DamagePerBullet, true);
                bullet.GetComponent<DamagerInflicter>().AttackWalls = false;
                bullet.AddComponent<DieInSeconds>().Seconds = 5;
                bullet.AddComponent<SlowlySlowDown>().TimeUntilStartSlowingDown = EffectiveRange/ProjectileSpeed;
                bullet.AddComponent<InflicterSlow>().ini(2, 0.5f, true);

                bullet.AddComponent<BouncingBullet>().rb = bullet.GetComponent<Rigidbody2D>();

                //increase inaccuracy
                UniversalReference.PlayerScript.RequestInaccuracyIncrease(InaccuracyPerShot);
                if (UniversalReference.PlayerScript.InaccuracyRecoveryBlock < BaseCooldownBetweenShots)
                {
                    UniversalReference.PlayerScript.InaccuracyRecoveryBlock += BaseCooldownBetweenShots;
                }

                //display chambering ammo status if applicable
                //AmmoStatus.NowChambering(BaseCooldownBetweenShots); //since chambering is so fast in smg, i dont need to show it

            }
        }

        FramesSincePlayerRequestedShooting = 0;


        DisplayAmmo();
    }

    public override void TryAltFire()
    {
    }

    public override void ForceReload()
    {
        if (Ammo != MaxAmmo && ReloadTimeRemaining <= 0)
        {
            Ammo = 0;

            ReloadTimeRemaining = BaseReloadTime;

            UniversalReference.PlayerObject.AddComponent<StatusSlow>().ini(BaseReloadTime, 0.25f, true);

            AmmoStatus.NowReloading(BaseReloadTime);
        }
        DisplayAmmo();

    }

    public void DisplayCorrectStatusImage()
    {
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