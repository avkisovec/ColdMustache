using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser01_item : InventoryItem {

    public float FullCharge = Laser01.FullCharge;
    public float CurrCharge = Laser01.FullCharge;

    private void Update()
    {
        if (CurrCharge < FullCharge)
        {
            CurrCharge += Time.deltaTime / 2;   //still charges while in inventory, but only at half speed
        }
    }

    public override void CodeBeforeRemoving()
    {
        CurrCharge = UniversalReference.PlayerObject.GetComponent<Laser01>().CurrCharge;
    }

    public override void CodeAfterEquipping()
    {
        //remove all previously held (=attached components) weapons
        foreach (Weapon w in UniversalReference.PlayerObject.GetComponents<Weapon>())
        {
            Destroy(w);
        }

        UniversalReference.PlayerObject.AddComponent<Laser01>().CurrCharge = CurrCharge;

        //UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(player, "Assets/Resources/Items/InventoryItem.cs (38,17)", WeaponScriptName);


        return;
    }
}
