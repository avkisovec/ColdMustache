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
        Ammo = UniversalReference.PlayerObject.GetComponent<SMG01>().Ammo;
        Mode = UniversalReference.PlayerObject.GetComponent<SMG01>().Mode;
    }

    public override void CodeAfterEquipping()
    {
        //remove all previously held (=attached components) weapons
        foreach (Weapon w in UniversalReference.PlayerObject.GetComponents<Weapon>())
        {
            Destroy(w);
        }

        SMG01 smg = UniversalReference.PlayerObject.AddComponent<SMG01>();
        smg.Mode = Mode;
        smg.Ammo = Ammo;
        smg.sprite = sprite;
        smg.BulletColorStart = BulletColorStart;
        smg.BulletColorEnd = BulletColorEnd;

        //UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(player, "Assets/Resources/Items/InventoryItem.cs (38,17)", WeaponScriptName);


        return;
    }
}
