using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun01_item : InventoryItem {

    public int Ammo = 2;

    public override void CodeBeforeRemoving()
    {
        Ammo = PlayerReference.PlayerObject.GetComponent<Shotgun01>().Ammo;
    }

    public override void CodeAfterEquipping()
    {
        //remove all previously held (=attached components) weapons
        foreach (Weapon w in PlayerReference.PlayerObject.GetComponents<Weapon>())
        {
            Destroy(w);
        }

        PlayerReference.PlayerObject.AddComponent<Shotgun01>().Ammo = Ammo;

        return;
    }
}
