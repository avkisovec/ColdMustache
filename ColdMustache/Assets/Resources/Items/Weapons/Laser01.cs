using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Laser01 : Weapon {
    
    public static float FullCharge = 2f;
    public float CurrCharge = 2f;
    
    public string SpritePath = "Items/Weapons/Laser01";
    
    public string AmmoSpriteSheetPath = "Gui/Hud/Ammo/Charger10";
    public Sprite[] AmmoSpriteSheet;
    

    private void Start()
    {
        UniversalReference.GunRotator.GunSpriteRenderer.sprite = Resources.Load<Sprite>(SpritePath);
        
        AmmoSpriteSheet = Resources.LoadAll<Sprite>(AmmoSpriteSheetPath);

        UniversalReference.WeaponStatus.sprite = Resources.Load<Sprite>("EmptyPixel");

        DisplayAmmo();
    }

    private void Update()
    {
        if(CurrCharge < FullCharge)
        {
            CurrCharge += Time.deltaTime;
        }
        DisplayAmmo();
    }

    public override void TryShooting(Vector3 Target)
    {
        if (CurrCharge >= FullCharge)
        {
            CurrCharge = 0;

            GameObject laser = new GameObject();
            laser.AddComponent<SpriteRenderer>();
            laser.transform.localScale = new Vector3(1,0.4f,1);
            Laser l = laser.AddComponent<Laser>();
            l.Origin = transform.position;
            l.End = Target;

            SpriteSheetAnimation lanim = laser.AddComponent<SpriteSheetAnimation>();
            lanim.Sprites = Resources.LoadAll<Sprite>("Fx/LaserCharged_57frames2"); //for this animation, damage should start at 70% 
            lanim.LifeSpanInSeconds = 0.5f;
            lanim.Mode = SpriteSheetAnimation.Modes.Destroy;

            GameObject dmg = new GameObject();
            dmg.transform.parent = laser.transform;
            dmg.transform.localScale = new Vector3(1f / 32f, 1, 1);
            //dmg.AddComponent<InflicterSlow>();
            dmg.AddComponent<DamagerInflicter>().ini(Entity.team.Player, 1, false, true, 1, 0);
            dmg.AddComponent<BoxCollider2D>().isTrigger = true;
            dmg.AddComponent<FxBurnSmoke>();
            dmg.AddComponent<WiggleNonNoticably>();

            gameObject.AddComponent<StatusSlow>().ini(0.5f, 0, false);

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
    }

    public void DisplayAmmo()
    {
        if (CurrCharge >= FullCharge)
        {
            UniversalReference.AmmoCounter.sprite = AmmoSpriteSheet[0];
        }
        else
        {
            UniversalReference.AmmoCounter.sprite = AmmoSpriteSheet[Mathf.RoundToInt(((float)CurrCharge / (float)FullCharge) * (float)(AmmoSpriteSheet.Length - 1))];
        }
    }
}
