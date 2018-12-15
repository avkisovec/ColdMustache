using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour {

    public enum ItemType { Undefined, ClothingShirt, ClothingJacket, ClothingHead, WeaponSide, WeaponMain };

    public ItemType Type = ItemType.WeaponMain;
    
    public virtual void CodeAfterEquipping()
    {
        
    }

    public virtual void CodeBeforeRemoving()
    {

    }

}
