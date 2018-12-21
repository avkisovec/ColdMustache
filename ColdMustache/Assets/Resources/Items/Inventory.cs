using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public List<InventorySlot> Slots = new List<InventorySlot>();
    
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void ReportSlotBeingClicked(int id)
    {
        GameObject itemObject;
        switch (id)
        {
            case -11: //special slots (equip) -> go to first empty normal slot
            case -12:
            case -13:
            case -21:
            case -22:
                if (!IsSlotEmpty(GetListIdOfSlotWithId(id)))
                {
                    itemObject = Slots[GetListIdOfSlotWithId(id)].gameObject.transform.GetChild(0).gameObject;
                    itemObject.GetComponent<InventoryItem>().CodeBeforeRemoving();
                    itemObject.transform.parent = Slots[FindFirstEmptyNormalSlot()].transform;
                    itemObject.transform.localPosition = new Vector3(0, 0, -1);
                }

                if (IsSlotEmpty(GetListIdOfSlotWithId(-22)))
                {
                    NoWeaponEquipped();
                }

                return;
            default: //normal slots (storage) -> go to corresponding special slot (and clear that one)
                if (!IsSlotEmpty(GetListIdOfSlotWithId(id)))
                {
                    itemObject = Slots[GetListIdOfSlotWithId(id)].gameObject.transform.GetChild(0).gameObject;
                    
                    int SpecialSlotId = GetSpecialSlotId(itemObject.GetComponent<InventoryItem>().Type);

                    bool WasTheSlotAlreadyOccupied = !IsSlotEmpty(GetListIdOfSlotWithId(SpecialSlotId));

                    itemObject.transform.parent = Slots[GetListIdOfSlotWithId(SpecialSlotId)].transform;
                    itemObject.transform.localPosition = new Vector3(0, 0, -1);

                    if (WasTheSlotAlreadyOccupied)
                    {
                        ReportSlotBeingClicked(SpecialSlotId); //to move the previously equipped item to the now empty slot;
                    }

                    itemObject.GetComponent<InventoryItem>().CodeAfterEquipping();
                }
                return;
        }
    }

    public void NoWeaponEquipped()
    {
        foreach (Weapon w in PlayerReference.PlayerObject.GetComponents<Weapon>())
        {
            Destroy(w);
        }
        PlayerReference.GunRotator.GunSpriteRenderer.sprite = GuiReference.WeaponStatus.sprite = GuiReference.AmmoCounter.sprite = Resources.Load<Sprite>("EmptyPixel");
    }

    public int GetSpecialSlotId(InventoryItem.ItemType type)
    {
        switch (type)
        {
            case InventoryItem.ItemType.ClothingShirt:
                return -11;
            case InventoryItem.ItemType.ClothingJacket:
                return -12;
            case InventoryItem.ItemType.ClothingHead:
                return -13;
            case InventoryItem.ItemType.WeaponSide:
                return -21;
            case InventoryItem.ItemType.WeaponMain:
                return -22;
        }
        
        return -1;
    }

    public int GetListIdOfSlotWithId(int slotId)
    {
        for(int i = 0; i < Slots.Count; i++)
        {
            if(Slots[i].SlotID == slotId)
            {
                return i;
            }
        }
        return -1; //means "not found"
    }

    public bool IsSlotEmpty(int ListId)
    {
        if (Slots[ListId].transform.childCount == 0)
        {
            return true;
        }
        return false;
    }

    public int FindFirstEmptyNormalSlot()
    {
        for (int i = 0; i < 100; i++)
        {
            int ListId = GetListIdOfSlotWithId(i);
            if (ListId != 0)
            {
                if(Slots[ListId].SlotID >= 0 && IsSlotEmpty(ListId))
                {
                    return ListId;
                }
            }
        }
        return -1;
    }

}
