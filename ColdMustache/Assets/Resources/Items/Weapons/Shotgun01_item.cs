using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun01_item : InventoryItem {

    public int Ammo = 2;

    public override void CodeBeforeRemoving()
    {
        Ammo = UniversalReference.PlayerObject.GetComponent<Shotgun01>().Ammo;
    }

    public override void CodeAfterEquipping()
    {
        //remove all previously held (=attached components) weapons
        foreach (Weapon w in UniversalReference.PlayerObject.GetComponents<Weapon>())
        {
            Destroy(w);
        }

        UniversalReference.PlayerObject.AddComponent<Shotgun01>().Ammo = Ammo;

        return;
    }
}
