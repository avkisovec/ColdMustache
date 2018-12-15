using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper01_item : InventoryItem {

    public int Ammo = 5;

    public override void CodeBeforeRemoving()
    {
        Ammo = PlayerReference.PlayerObject.GetComponent<Sniper01>().Ammo;
    }

    public override void CodeAfterEquipping()
    {
        //remove all previously held (=attached components) weapons
        foreach (Weapon w in PlayerReference.PlayerObject.GetComponents<Weapon>())
        {
            Destroy(w);
        }
        
        PlayerReference.PlayerObject.AddComponent<Sniper01>().Ammo = Ammo;
        
        return;
    }
}
