using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMGlike_item : InventoryItem {
    
    public override void CodeBeforeRemoving()
    {
        UniversalReference.PlayerScript.CurrentlyEquippedWeapon.CurrentlyActive = false;

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
   
        UniversalReference.PlayerScript.CurrentlyEquippedWeapon = GetComponent<Weapon>();

        UniversalReference.PlayerScript.CurrentlyEquippedWeapon.CurrentlyActive = true;

        UniversalReference.PlayerScript.CurrentlyEquippedWeapon.OnBecomingActive();


        return;
    }

}
