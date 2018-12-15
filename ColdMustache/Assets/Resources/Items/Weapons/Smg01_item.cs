using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smg01_item : InventoryItem {

    public int Ammo = 0;
    public SMG01.SmgModes Mode = SMG01.SmgModes.Full;

    public override void CodeBeforeRemoving()
    {
        Ammo = PlayerReference.PlayerObject.GetComponent<SMG01>().Ammo;
        Mode = PlayerReference.PlayerObject.GetComponent<SMG01>().Mode;
    }

    public override void CodeAfterEquipping()
    {
        //remove all previously held (=attached components) weapons
        foreach (Weapon w in PlayerReference.PlayerObject.GetComponents<Weapon>())
        {
            Destroy(w);
        }

        PlayerReference.PlayerObject.AddComponent<SMG01>().Mode = Mode;
        PlayerReference.PlayerObject.GetComponent<SMG01>().Ammo = Ammo;

        //UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(player, "Assets/Resources/Items/InventoryItem.cs (38,17)", WeaponScriptName);


        return;
    }
}
