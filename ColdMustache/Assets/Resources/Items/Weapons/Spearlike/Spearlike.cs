using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spearlike : Weapon {

    //general things - ammo, cooldown, reload, range
    

    public float CooldownBetweenHits = 0.4f;
    

    //the relative potion inside a GunContainer where DamagerInflicter will spanw
    public Vector3 DamagerPosition = new Vector3(1.5f, 0, 0);

    public float DamagePerHit = 1;
    
    //visual things - sprites, icons, colors

    public Sprite SpriteNormal = null;
    public Sprite SpriteExtended = null;

    public float BaseTimeTillRetraction = 0.2f;
    static float SpecialValueIndicatingWeaponsBeenJustRetracted = -999999;
    float CurrTimeTillRetraction = SpecialValueIndicatingWeaponsBeenJustRetracted;

    public float BaseExtensionCooldown = 0.6f;
    float CurrExtensionCooldown = 0;

    //backend stuff 
    DamagerInflicter DamagerInflicterReference;


    public override void OnBecomingActive()
    {
        //default values
        //sprites
        UniversalReference.GunRotator.GunSpriteRenderer.sprite = SpriteNormal;
        
        //crosshair
        UniversalReference.crosshair.EffectiveRange = 1000;


        GameObject DamagerObject = new GameObject();
        DamagerObject.transform.parent = UniversalReference.GunRotator.transform;
        DamagerObject.transform.localPosition = DamagerPosition;
        DamagerObject.AddComponent<CircleCollider2D>().isTrigger = true;
        DamagerObject.GetComponent<CircleCollider2D>().radius = 0.2f;
        DamagerInflicterReference = DamagerObject.AddComponent<DamagerInflicter>();
        DamagerInflicterReference.ini(Entity.team.Player, 0, false, true, 0, 0, false);
        DamagerObject.name = "Damager";
    }

    private void Update()
    {
        if (CurrentlyActive)
        {            
            if (CurrTimeTillRetraction > 0)
            {
                CurrTimeTillRetraction -= Time.deltaTime;
                if(CurrTimeTillRetraction < 0 && CurrTimeTillRetraction != SpecialValueIndicatingWeaponsBeenJustRetracted)
                {
                    CurrTimeTillRetraction = SpecialValueIndicatingWeaponsBeenJustRetracted;
                    Retract();
                }
            }

            if(CurrExtensionCooldown > 0)
            {
                CurrExtensionCooldown -= Time.deltaTime;
            }
        }
    }

    public override void TryShooting(Vector3 Target)
    {
        if(CurrExtensionCooldown <= 0)
        {
            Extend();
        }
    }

    public override void TryAltFire()
    {
    }

    public override void ForceReload()
    {
    }

    public void Extend()
    {
        CurrExtensionCooldown = BaseExtensionCooldown;
        CurrTimeTillRetraction = BaseTimeTillRetraction;
        UniversalReference.GunRotator.GunSpriteRenderer.sprite = SpriteExtended;
        DamagerInflicterReference.ini(Entity.team.Player, DamagePerHit, false, true, CooldownBetweenHits, 0, false);
    }

    public void Retract()
    {
        UniversalReference.GunRotator.GunSpriteRenderer.sprite = SpriteNormal;
        DamagerInflicterReference.ini(Entity.team.Player, 0, false, true, 0, 0, false);
    }
}

