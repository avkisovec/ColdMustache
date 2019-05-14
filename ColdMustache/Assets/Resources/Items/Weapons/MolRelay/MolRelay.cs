using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolRelay : Weapon
{

    //general things - ammo, cooldown, reload, range

    public int MaxAmmo = 10;
    public int Ammo = 10;

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

    public Transform ReadyParticleSource;

    //alt fire

    public float AbilityCooldownBase = 5;
    float AbilityCooldownCurr = 0;

    public float AbilityRange = 5;

    //backend stuff

    float SpecialValueThatIndicatesWeaponHasJustBeenReloaded = -999999;

    public int FramesSincePlayerRequestedShooting = 0; //for the purposes of semi-auto


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

        if(AbilityCooldownCurr > 0) AbilityCooldownCurr -= Time.deltaTime;

        if (CurrentlyActive)
        {

            if (AbilityCooldownCurr <= 0){
                SpawnReadyParticles();
            }

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
        }
    }

    public override void TryShooting(Vector3 Target)
    {
        if (Ammo > 0 && CurrCooldownBetweenShots <= 0)
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
                bullet.AddComponent<DieInSeconds>().Seconds = 5;
                bullet.AddComponent<SlowlySlowDown>().TimeUntilStartSlowingDown = EffectiveRange / ProjectileSpeed;
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
                //AmmoStatus.NowChambering(BaseCooldownBetweenShots); //since chambering is so fast in smg, i dont need to show it

            
        }

        FramesSincePlayerRequestedShooting = 0;


        DisplayAmmo();
    }

    public override void TryAltFire()
    {




        if (AbilityCooldownCurr <= 0)
        {
            AbilityCooldownCurr = AbilityCooldownBase;

            Vector2 Target= Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 Origin = UniversalReference.PlayerObject.transform.position;

            Vector2 LaserVector = Target - (Vector2)Origin;

            //laser
            GameObject laser = new GameObject();
            laser.transform.position = Origin;
            laser.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("MidEdgePixel");
            Util.RotateTransformToward(laser.transform, Target);

            float LaserDistance = NavTestStatic.GetLaserDistance(Origin, (Vector2)Origin + LaserVector / LaserVector.magnitude * AbilityRange);

            laser.transform.localScale = new Vector3(
                LaserDistance, 0.4f / 32f, -30
                );

            Vector2 PosDelta = Util.GetDirectionVectorToward(UniversalReference.PlayerObject.transform, Target);

            PosDelta *= LaserDistance;


            //particles on current player pos
            SpawnTeleportParticles(UniversalReference.PlayerObject.transform.position + new Vector3(0, 0, -10));

            //move player
            UniversalReference.PlayerObject.transform.position += (Vector3)PosDelta;

            //particles on new player pos
            SpawnTeleportParticles(UniversalReference.PlayerObject.transform.position + new Vector3(0, 0, -10));

            SpriteSheetAnimation lanim = laser.AddComponent<SpriteSheetAnimation>();
            lanim.Sprites = Resources.LoadAll<Sprite>("Fx/TransportBeam");
            lanim.LifeSpanInSeconds = 0.5f;
            lanim.Mode = SpriteSheetAnimation.Modes.Destroy;


            //laser.AddComponent<WiggleNonNoticably>();


            //stops player movement briefly
            UniversalReference.PlayerEntity.gameObject.AddComponent<StatusSlow>().ini(0.15f, 0, false);

        }





        DisplayCorrectStatusImage();
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
        UniversalReference.WeaponStatus.sprite = null;

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

    //particles signalling that the ability is ready
    public void SpawnReadyParticles(){


        if (Util.Coinflip()) return;

        Vector3 Origin = ReadyParticleSource.position;


        GameObject go = new GameObject();
        go.AddComponent<Particle>();
        go.transform.position = Origin;
        Particle p = go.GetComponent<Particle>();
        p.UseShifts = true;
        p.LeaveParent = true;
        p.StartAt00 = false;
        p.sprite = Resources.Load<Sprite>("pixel");
        p.Lifespan = Random.Range(5, 30);
        p.StartingScale = 0.10f;
        p.EndingScale = 0.25f;
        p.StartingColor = BulletColorStart;
        p.EndingColor = new Color(BulletColorEnd.r, BulletColorEnd.g, BulletColorEnd.b, 0);
        p.StartingHorizontalWind = Random.Range(-0.03f, 0.03f);
        p.StartingVerticalWind = Random.Range(-0.03f, 0.03f);
        p.EndingHorizontalWind = Random.Range(-0.03f, 0.03f);
        p.EndingVerticalWind = Random.Range(-0.03f, 0.03f);


    }

    public void SpawnTeleportParticles(Vector3 Pos)
    {

        for(int i = 0; i < 30; i++){

            GameObject go = new GameObject();
            go.AddComponent<Particle>();
            go.transform.position = Pos;
            Particle p = go.GetComponent<Particle>();
            p.UseShifts = true;
            p.LeaveParent = true;
            p.StartAt00 = false;
            p.sprite = Resources.Load<Sprite>("pixel");
            p.Lifespan = Random.Range(50, 140);
            p.StartingScale = 0.35f;
            p.EndingScale = 0.55f;
            p.StartingColor = BulletColorStart;
            p.EndingColor = new Color(BulletColorEnd.r, BulletColorEnd.g, BulletColorEnd.b, 0);
            p.StartingHorizontalWind = Random.Range(-0.03f, 0.03f);
            p.StartingVerticalWind = Random.Range(-0.03f, 0.03f);
            p.EndingHorizontalWind = Random.Range(-0.01f, 0.01f);
            p.EndingVerticalWind = Random.Range(-0.01f, 0.01f);

        }


    }



}
