using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour {

    public enum ItemType { Undefined, ClothingShirt, ClothingJacket, ClothingHead, WeaponSide, WeaponMain, Everything };
    //itemtype.everything - used for slots; anything can go into this slot (general inventory slot)

    public ItemType Type = ItemType.WeaponMain;
    
    public virtual void CodeAfterEquipping()
    {
        
    }

    public virtual void CodeBeforeRemoving()
    {

    }
    

}
