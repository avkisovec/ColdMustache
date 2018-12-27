using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun01_item : InventoryItem {

    public int Ammo = 2;

    public Sprite sprite;
    public Color BulletColorStart;
    public Color BulletColorEnd;

    public override void CodeBeforeRemoving()
    {
        Ammo = ((Shotgun01)UniversalReference.PlayerScript.CurrentlyEquippedWeapon).Ammo;

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

        Shotgun01 weapon = UniversalReference.PlayerObject.AddComponent<Shotgun01>();
        weapon.Ammo = Ammo;
        weapon.sprite = sprite;
        weapon.BulletColorStart = BulletColorStart;
        weapon.BulletColorEnd = BulletColorEnd;

        UniversalReference.PlayerScript.CurrentlyEquippedWeapon = weapon;

        return;
    }
}
