using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper01_item : InventoryItem {

    public int Ammo = 5;
    public Sniper01.ZoomModes Mode = Sniper01.ZoomModes.Zoom4x;

    public override void CodeBeforeRemoving()
    {
        UniversalReference.camControlPixelPerfect.ZoomedInFreecamFactor = UniversalReference.camControlPixelPerfect.DefaultZoomedInFreecamFactor;

        Ammo = UniversalReference.PlayerObject.GetComponent<Sniper01>().Ammo;

        Mode = UniversalReference.PlayerObject.GetComponent<Sniper01>().Mode;
    }

    public override void CodeAfterEquipping()
    {
        //remove all previously held (=attached components) weapons
        foreach (Weapon w in UniversalReference.PlayerObject.GetComponents<Weapon>())
        {
            Destroy(w);
        }

        UniversalReference.PlayerObject.AddComponent<Sniper01>().Ammo = Ammo;
        UniversalReference.PlayerObject.GetComponent<Sniper01>().Mode = Mode;

        return;
    }
}
