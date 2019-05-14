using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolRelay_item : InventoryItem
{

    Transform ReadyParticleSource;

    public override void CodeBeforeRemoving()
    {
        Destroy(ReadyParticleSource.gameObject);

        UniversalReference.PlayerScript.CurrentlyEquippedWeapon.CurrentlyActive = false;

        UniversalReference.PlayerScript.CurrentlyEquippedWeapon = null;
        UniversalReference.GunRotator.GunSpriteRenderer.sprite = UniversalReference.WeaponStatus.sprite = UniversalReference.AmmoCounter.sprite = Resources.Load<Sprite>("EmptyPixel");
    }

    public override void CodeAfterEquipping()
    {
        GameObject go = new GameObject();
        ReadyParticleSource = go.transform;

        ReadyParticleSource.parent = UniversalReference.PlayerBulletsOrigin.transform;

        ReadyParticleSource.transform.localPosition = new Vector3(-0.5f,0,0);
        


        //remove all previously held (=attached components) weapons
        foreach (Weapon w in UniversalReference.PlayerObject.GetComponents<Weapon>())
        {
            Destroy(w);
        }

        UniversalReference.PlayerScript.CurrentlyEquippedWeapon = GetComponent<Weapon>();
        ((MolRelay)UniversalReference.PlayerScript.CurrentlyEquippedWeapon).ReadyParticleSource = ReadyParticleSource;

        UniversalReference.PlayerScript.CurrentlyEquippedWeapon.CurrentlyActive = true;

        UniversalReference.PlayerScript.CurrentlyEquippedWeapon.OnBecomingActive();


        return;
    }
}
