using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower_item : InventoryItem
{
    public override void CodeBeforeRemoving()
    {

        //move bullet origin, dont forget to move it in the first place
        UniversalReference.PlayerBulletsOrigin.localPosition += new Vector3(-0.14f, 0, 0);

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

        //move bullet origin, dont forget to move it back
        UniversalReference.PlayerBulletsOrigin.localPosition += new Vector3(0.14f,0,0);

        UniversalReference.PlayerScript.CurrentlyEquippedWeapon = GetComponent<Weapon>();

        UniversalReference.PlayerScript.CurrentlyEquippedWeapon.CurrentlyActive = true;

        UniversalReference.PlayerScript.CurrentlyEquippedWeapon.OnBecomingActive();


        return;
    }
}