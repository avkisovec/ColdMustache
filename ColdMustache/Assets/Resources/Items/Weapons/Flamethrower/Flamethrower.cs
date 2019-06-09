using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Weapon
{
    //general things - ammo, cooldown, reload, range

    public int MaxAmmo = 40;
    public int Ammo = 40;

    public float BaseCooldownBetweenShots = 0.03f;
    public float CurrCooldownBetweenShots = 0;

    public float BaseReloadTime = 1f;
    public float ReloadTimeRemaining = 0;

    public float FlamesLivespan = 3;
    public float EffectiveRange = 5;
    public float InaccuracyPerShot = 1.2f;

    public float DamagePerBullet = 1;

    public float InaccuracyBase = 1.5f;
    public float InaccuracyImportance = 3f;

    public float ProjectileSpeed = 5;

    //visual things - sprites, icons, colors

    public Sprite sprite = null;

    public Color BulletColorStart = new Color(1f, 1f, 0.5f, 1);
    public Color BulletColorEnd = new Color(0.5f, 0, 0, 1);

    public string AmmoSpriteSheetPath = "Gui/Hud/Ammo/Pressure8";
    public Sprite[] AmmoSpriteSheet;

    public string StatusFullPath = "Gui/Hud/WeaponStatus/ModeFull";
    public Sprite StatusFullSprite;
    public string StatusSemiPath = "Gui/Hud/WeaponStatus/ModeSemi";
    public Sprite StatusSemiSprite;

    public string MuzzleFlashSpritePath = "Fx/SmgMuzzleFlash";

    public string FlameSpriteSheetPath = "Fx/Your/Path/Here";

    Sprite[] FlameSprites;

    //alt fire

    public enum SmgModes { Semi, Full }
    public SmgModes Mode = SmgModes.Full;

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
        FlameSprites = Resources.LoadAll<Sprite>(FlameSpriteSheetPath);

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
            SpawnIgnitionSourceParticle();

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
            if (Mode == SmgModes.Full || (Mode == SmgModes.Semi && FramesSincePlayerRequestedShooting >= 2))
            {
                Ammo--;
                CurrCooldownBetweenShots = BaseCooldownBetweenShots;

                Vector3 Origin = new Vector3(UniversalReference.PlayerBulletsOrigin.position.x, UniversalReference.PlayerBulletsOrigin.position.y, -31);

                //bullet object, position, scale ...
                GameObject bullet = new GameObject();
                bullet.transform.position = Origin;
                bullet.transform.localScale = new Vector3(1f, 1f, 1);
                bullet.gameObject.name = "bullet";

                //bullet physics
                bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                float Inaccuracy = InaccuracyBase + InaccuracyImportance * UniversalReference.PlayerScript.Inaccuracy;
                bullet.GetComponent<Rigidbody2D>().velocity = Util.RotateVector(Util.GetDirectionVectorToward(UniversalReference.PlayerObject.transform, Target), Random.Range(-Inaccuracy, Inaccuracy)) * ProjectileSpeed;
                bullet.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                bullet.gameObject.AddComponent<CircleCollider2D>().isTrigger = true;
                bullet.GetComponent<CircleCollider2D>().radius = 0.6f;

                //bullet visuals
                bullet.AddComponent<SpriteRenderer>();

                SimpleAnimation_codefed anim = bullet.AddComponent<SimpleAnimation_codefed>();

                anim.Duration = FlamesLivespan + 0.02f; //increaed by about a frame, beacuse of problem where it rolls around and shows the first frame the frame it dies
                anim.sprites = FlameSprites;

                //custom mechanics
                bullet.AddComponent<DamagerInflicter_EnvironmentOnly>().ini(Entity.team.Player, DamagePerBullet, true);
                bullet.AddComponent<DamagerInflicterAoE>().ini(Entity.team.Player, DamagePerBullet, 10, true, 1f, 0, DamagerInflicter.WeaponTypes.Fire );
                bullet.AddComponent<DieInSeconds>().Seconds = FlamesLivespan;
                bullet.AddComponent<InflicterSlow>().ini(2, 0.5f, true);


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
        if (FramesSincePlayerRequestedAltFire >= 2)
        {

            if (Mode == SmgModes.Full)
            {
                Mode = SmgModes.Semi;
            }
            else if (Mode == SmgModes.Semi)
            {
                Mode = SmgModes.Full;
            }
        }

        DisplayCorrectStatusImage();
        FramesSincePlayerRequestedAltFire = 0;
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
        if (Mode == SmgModes.Full)
        {
            UniversalReference.WeaponStatus.sprite = StatusFullSprite;
        }
        else if (Mode == SmgModes.Semi)
        {
            UniversalReference.WeaponStatus.sprite = StatusSemiSprite;
        }

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

    public void SpawnIgnitionSourceParticle(){

        if(Util.Coinflip()) return;

        Vector3 Origin = new Vector3(UniversalReference.PlayerBulletsOrigin.position.x, UniversalReference.PlayerBulletsOrigin.position.y, -31);


        GameObject go = new GameObject();
        go.AddComponent<Particle>();
        go.transform.position = Origin;
        Particle p = go.GetComponent<Particle>();
        p.UseShifts = true;
        p.LeaveParent = true;
        p.StartAt00 = false;
        p.sprite = Resources.Load<Sprite>("pixel");
        p.Lifespan = Random.Range(3, 9);
        p.StartingScale = 0.05f;
        p.EndingScale = 0.2f;
        p.StartingColor = new Color(1,1,0.5f,1);
        p.EndingColor = new Color(1,0,0,0);
        p.StartingHorizontalWind = Random.Range(0, 0);
        p.StartingVerticalWind = Random.Range(0, 0.03f);
        p.EndingHorizontalWind = Random.Range(-0.02f, 0.02f);
        p.EndingVerticalWind = Random.Range(0.03f, 0.05f);

    }
}
