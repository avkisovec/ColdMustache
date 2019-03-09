using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lasergun : Weapon
{

    //general things - ammo, cooldown, reload, range

    public float MaxAmmo = 1;
    public float Ammo = 1;

    public float EffectiveRange = 10;

    public float Damage = 1;



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

    public enum SmgModes { Semi, Full }
    public SmgModes Mode = SmgModes.Full;

    //backend stuff


    public override void OnBecomingActive()
    {

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
        if(Ammo < MaxAmmo){
            Ammo += Time.deltaTime;
        }
        if(Ammo > MaxAmmo){
            Ammo = MaxAmmo;
        }

        if(CurrentlyActive) DisplayAmmo();

    }

    public override void TryShooting(Vector3 Target)
    {
        if (Ammo >= MaxAmmo)
        {
            Ammo=0;

            Vector3 Origin = new Vector3(UniversalReference.PlayerBulletsOrigin.position.x, UniversalReference.PlayerBulletsOrigin.position.y, -31);

            Vector2 LaserVector = (Vector2)Target - (Vector2)Origin;

            //laser
            GameObject laser = new GameObject();
            laser.transform.position = Origin;
            laser.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("MidEdgePixel");
            Util.RotateTransformToward(laser.transform, Target);
            laser.transform.localScale = new Vector3(
                NavTestStatic.GetLaserDistance(Origin,(Vector2)Origin+LaserVector/LaserVector.magnitude*EffectiveRange
                )+0.5f,0.4f/32f,-30);

            //laser.AddComponent<DieInSeconds>().Seconds = 0.5f;


            SpriteSheetAnimation lanim = laser.AddComponent<SpriteSheetAnimation>();
            lanim.Sprites = Resources.LoadAll<Sprite>("Fx/LaserCharged_57frames_MidEdge"); //for this animation, damage should start at 70% 
            lanim.LifeSpanInSeconds = 0.5f;
            lanim.Mode = SpriteSheetAnimation.Modes.Destroy;


            laser.AddComponent<DamagerInflicterAoE>().ini(Entity.team.Player, Damage, 0.5f, true, 1, 0.15f, DamagerInflicter.WeaponTypes.Fire);

            laser.AddComponent<BoxCollider2D>().isTrigger = true;
            laser.AddComponent<FxBurnSmoke>();
            laser.AddComponent<WiggleNonNoticably>();



            UniversalReference.PlayerEntity.gameObject.AddComponent<StatusSlow>().ini(0.5f, 0, false);

        }

        DisplayAmmo();
    }

    public override void TryAltFire()
    {
    }

    public override void ForceReload()
    {
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
            UniversalReference.AmmoCounter.sprite = AmmoSpriteSheet[0];
            AmmoStatus.EmptyMag();
        }
        else
        {
            UniversalReference.AmmoCounter.sprite = AmmoSpriteSheet[Mathf.RoundToInt(((float)Ammo / (float)MaxAmmo) * (float)(AmmoSpriteSheet.Length - 1))];
        }
    }
}
