using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper01_item : InventoryItem {

    public int Ammo = 5;
    public Sniper01.ZoomModes Mode = Sniper01.ZoomModes.Zoom4x;

    public Sprite sprite;
    public Color BulletColorStart;
    public Color BulletColorEnd;

    public override void CodeBeforeRemoving()
    {
        UniversalReference.camControlPixelPerfect.ZoomedInFreecamFactor = UniversalReference.camControlPixelPerfect.DefaultZoomedInFreecamFactor;

        Ammo = ((Sniper01)UniversalReference.PlayerScript.CurrentlyEquippedWeapon).Ammo;
        Mode = ((Sniper01)UniversalReference.PlayerScript.CurrentlyEquippedWeapon).Mode;

        foreach (Weapon w in UniversalReference.PlayerObject.GetComponents<Weapon>())
        {
            Destroy(w);
        }
        UniversalReference.PlayerScript.CurrentlyEquippedWeapon = null;
        UniversalReference.GunRotator.GunSpriteRenderer.sprite = UniversalReference.WeaponStatus.sprite = UniversalReference.AmmoCounter.sprite = Resources.Load<Sprite>("EmptyPixel");

    }

    public override void CodeAfterEquipping()
    {
        //remove all previously held (=attached components) weapons
        foreach (Weapon w in UniversalReference.PlayerObject.GetComponents<Weapon>())
        {
            Destroy(w);
        }

        Sniper01 weapon = UniversalReference.PlayerObject.AddComponent<Sniper01>();
        weapon.Mode = Mode;
        weapon.Ammo = Ammo;
        weapon.sprite = sprite;
        weapon.BulletColorStart = BulletColorStart;
        weapon.BulletColorEnd = BulletColorEnd;

        UniversalReference.PlayerScript.CurrentlyEquippedWeapon = weapon;

        return;
    }
}
