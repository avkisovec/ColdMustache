using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SMG01 : Weapon {

    public enum SmgModes { Semi, Full }
    public SmgModes Mode = SmgModes.Full;

    public int MaxAmmo = 10;
    public int Ammo = 10;

    public float BaseCooldownBetweenShots = 0.1f;
    public float CurrCooldownBetweenShots = 0;
    

    public float BaseReloadTime = 1f;
    public float ReloadTimeRemaining = 0;

    float SpecialValueThatIndicatesWeaponHasJustBeenReloaded = -999999;

    public string SpritePath = "Items/Weapons/SMG01";

    public int FramesSincePlayerRequestedShooting = 0; //for the purposes of semi-auto

    public int FramesSincePlayerRequestedAltFire = 0;

    public string AmmoCounterPath = "Canvas/AmmoCounter";
    public Image AmmoCounter;
    public string AmmoSpriteSheetPath = "Gui/Hud/Ammo/Bullets10";
    public Sprite[] AmmoSpriteSheet;

    public string WeaponStatusPath = "Canvas/WeaponStatus";
    public Image WeaponStatus;
    public string StatusFullPath = "Gui/Hud/WeaponStatus/ModeFull";
    public Sprite StatusFullSprite;
    public string StatusSemiPath = "Gui/Hud/WeaponStatus/ModeSemi";
    public Sprite StatusSemiSprite;

    private void Start()
    {
        GetComponent<SpriteManagerGunner>().GunSpriteRenderer.sprite = Resources.Load<Sprite>(SpritePath);

        AmmoCounter = GameObject.Find(AmmoCounterPath).GetComponent<Image>();
        AmmoSpriteSheet = Resources.LoadAll<Sprite>(AmmoSpriteSheetPath);

        DisplayAmmo();

        WeaponStatus = GameObject.Find(WeaponStatusPath).GetComponent<Image>();
        StatusFullSprite = Resources.Load<Sprite>(StatusFullPath);
        StatusSemiSprite = Resources.Load<Sprite>(StatusSemiPath);

        DisplayCorrectStatusImage();
    }

    private void Update()
    {
        if(ReloadTimeRemaining > 0)
        {
            ReloadTimeRemaining -= Time.deltaTime;
        }
        else if(ReloadTimeRemaining != SpecialValueThatIndicatesWeaponHasJustBeenReloaded) //special value, indicates that weapon has just reloaded and reloading should be ignored
        {
            Ammo = MaxAmmo;
            AmmoCounter.sprite = AmmoSpriteSheet[0];
            ReloadTimeRemaining = SpecialValueThatIndicatesWeaponHasJustBeenReloaded;
        }

        if(CurrCooldownBetweenShots > 0)
        {
            CurrCooldownBetweenShots -= Time.deltaTime;
        }

        if (FramesSincePlayerRequestedShooting < 1000)
        {
            FramesSincePlayerRequestedShooting++;
        }
        if(FramesSincePlayerRequestedAltFire < 1000)
        {
            FramesSincePlayerRequestedAltFire++;
        }
    }

    public override void TryShooting(Vector3 Target)
    {
        if(Ammo > 0 && CurrCooldownBetweenShots <= 0)
        {
            if(Mode==SmgModes.Full || ( Mode==SmgModes.Semi && FramesSincePlayerRequestedShooting >= 2))
            {
                Ammo--;
                CurrCooldownBetweenShots = BaseCooldownBetweenShots;

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
                bullet.AddComponent<InflicterSlow>().ini(2, 0.5f, true);
                bullet.gameObject.name = "bullet";

                bullet.AddComponent<DieInSeconds>().Seconds = 5;
                //bullet.gameObject.tag = "PlayerBullet";
                bullet.gameObject.AddComponent<CircleCollider2D>().isTrigger = true;
                //bullet.AddComponent<InflicterSlow>().ini(Entity.team.Enemy, 2, 0.25f);
            }
        }

        FramesSincePlayerRequestedShooting = 0;


        DisplayAmmo();
    }

    public override void TryAltFire()
    {
        if(FramesSincePlayerRequestedAltFire >= 2)
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
        if(Ammo != MaxAmmo && ReloadTimeRemaining <= 0)
        {
            Ammo = 0;

            ReloadTimeRemaining = BaseReloadTime;

            gameObject.AddComponent<StatusSlow>().ini(BaseReloadTime, 0.25f, true);
        }
        DisplayAmmo();
    }

    public void DisplayCorrectStatusImage()
    {
        if(Mode == SmgModes.Full)
        {
            WeaponStatus.sprite = StatusFullSprite;
        }
        else if(Mode == SmgModes.Semi)
        {
            WeaponStatus.sprite = StatusSemiSprite;
        }
        
    }

    public void DisplayAmmo()
    {
        if (Ammo == 0)
        {
            AmmoCounter.sprite = AmmoSpriteSheet[AmmoSpriteSheet.Length - 1];
        }
        else
        {
            AmmoCounter.sprite = AmmoSpriteSheet[AmmoSpriteSheet.Length - 1 - Mathf.RoundToInt(((float)Ammo / (float)MaxAmmo) * (float)(AmmoSpriteSheet.Length - 1))];
        }
    }
}
