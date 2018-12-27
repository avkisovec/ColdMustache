using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smg01_item : InventoryItem {

    public int Ammo = 10;
    public SMG01.SmgModes Mode = SMG01.SmgModes.Full;

    public Sprite sprite;
    public Color BulletColorStart;
    public Color BulletColorEnd;

    public override void CodeBeforeRemoving()
    {
        Ammo = ((SMG01)UniversalReference.PlayerScript.CurrentlyEquippedWeapon).Ammo;
        Mode = ((SMG01)UniversalReference.PlayerScript.CurrentlyEquippedWeapon).Mode;
                
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


        SMG01 weapon = UniversalReference.PlayerObject.AddComponent<SMG01>();
        weapon.Mode = Mode;
        weapon.Ammo = Ammo;
        weapon.sprite = sprite;
        weapon.BulletColorStart = BulletColorStart;
        weapon.BulletColorEnd = BulletColorEnd;

        UniversalReference.PlayerScript.CurrentlyEquippedWeapon = weapon;

       
        return;
    }
}
