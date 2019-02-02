using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour {

    //when you save this object, it is saved as this path; from this path, it will be loaded later
    public string PrefabPath;

    public enum ItemType { Undefined, ClothingShirt, ClothingJacket, ClothingHead, WeaponSide, WeaponMain, Everything, ActiveItem };
    //itemtype.everything - used for slots; anything can go into this slot (general inventory slot)

    public ItemType Type = ItemType.WeaponMain;

    //how much this item weighs
    public float Weight = 1;
    //how much this affects you max weight (things like exoskeletons or deep pockets can help you carry more)
    public float MaxWeightIncrease = 0;

    [TextArea]
    public string HoverDescription = "";
    
    public virtual void CodeAfterEquipping()
    {
        
    }

    public virtual void CodeBeforeRemoving()
    {

    }

    private void Update()
    {
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (MouseWorldPos.x > transform.position.x - (transform.lossyScale.x / 2) &&
            MouseWorldPos.x < transform.position.x + (transform.lossyScale.x / 2) &&
            MouseWorldPos.y > transform.position.y - (transform.lossyScale.y / 2) &&
            MouseWorldPos.y < transform.position.y + (transform.lossyScale.y / 2)
            )
        {
            ContextInfo.RequestStatic(HoverDescription);
        }
    }


}
