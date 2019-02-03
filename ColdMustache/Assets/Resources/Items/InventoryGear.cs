using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryGear : InventoryBase
{

    public float BaseMaxWeight = 40;
    
    public TextObject WeightText;

    public Transform EquippedWeaponIndicator;
    public Transform EquipppedItemIndicator;

    public bool DeleteMode = false;
    
    // Start is called before the first frame update
    void Start()
    {
        ReloadInventory();
    }

    public void ReloadInventory()
    {
        SlotScripts = SlotScripts.OrderBy(o => o.SlotId).ToList();

        for(int i = 0; i < SlotScripts.Count; i++)
        {
            if (SlotScripts[i].transform.childCount != 0)
            {
                Transform tr = SlotScripts[i].transform.GetChild(0);
                tr.parent = null;
                Destroy(tr.gameObject);
            }
        }

        LoadFromFile();
        ReEquipClothing();

        UpdateWeight();

        EquipFirstAvailableWeapon();        
        EquipFirstAvailableItem();
    }

    public void LoadFromFile()
    {
        foreach (string s in SaverLoader.ReadAFile(Application.dataPath + "/Save/Gear.save"))
        {
            string[] data = s.Split(';');
            if (data.Length >= 2)
            {
                if (data[1] != "EmptySlot")
                {
                    GameObject item = Instantiate(Resources.Load<GameObject>(data[1]) as GameObject);
                    item.transform.parent = SlotScripts[int.Parse(data[0])].transform;
                    item.transform.localPosition = new Vector3(0, 0, -1);
                    item.transform.localScale = new Vector3(1, 1, 1);
                }
            }

        }
    }

    public override void ReportSlotBeingClicked(int id)
    {
        if (DeleteMode)
        {
            int ListId = GetListIdOfSlotWithId(id);
            if (SlotScripts[ListId].transform.childCount != 0)
            {
                //removing it first before destroying it, as destruction needs some time and i need to calculate the new weight without the item
                //if i didnt remove it from parent it would still be included in weight calculation
                Transform tr = SlotScripts[ListId].transform.GetChild(0);
                tr.parent = null;
                Destroy(tr.gameObject);
                UpdateWeight();
            }
        }
        else
        {
            int ListId = GetListIdOfSlotWithId(id);
            if(SlotScripts[ListId].SlotType == InventoryItem.ItemType.WeaponMain)
            {
                if (SlotScripts[ListId].transform.childCount != 0)
                {
                    LastEquippedWeapon.CodeBeforeRemoving();
                    LastEquippedWeapon = SlotScripts[ListId].transform.GetChild(0).GetComponent<InventoryItem>();
                    LastEquippedWeapon.CodeAfterEquipping();
                    EquippedWeaponIndicator.transform.position = new Vector3(LastEquippedWeapon.transform.position.x, LastEquippedWeapon.transform.position.y, transform.position.z-5);
                    return;
                }
            }
            else if (SlotScripts[ListId].SlotType == InventoryItem.ItemType.ActiveItem)
            {
                if (SlotScripts[ListId].transform.childCount != 0)
                {
                    if(LastEquippedItem!=null){
                        LastEquippedItem.CodeBeforeRemoving();
                    }
                    LastEquippedItem = SlotScripts[ListId].transform.GetChild(0).GetComponent<InventoryItem>();
                    LastEquippedItem.CodeAfterEquipping();
                    EquipppedItemIndicator.transform.position = new Vector3(LastEquippedItem.transform.position.x, LastEquippedItem.transform.position.y, transform.position.z - 5);
                    return;
                }
            }
        }
        
    }

    public int GetListIdOfSlotWithId(int slotId)
    {
        for (int i = 0; i < SlotScripts.Count; i++)
        {
            if (SlotScripts[i].SlotId == slotId)
            {
                return i;
            }
        }
        return -1; //means "not found"
    }
    
    public int FindListIdOfFirstEmptyGearSlot(InventoryItem.ItemType type = InventoryItem.ItemType.Undefined)
    {
        for (int i = 0; i < SlotScripts.Count; i++)
        {
            if (SlotScripts[i].SlotType == type && SlotScripts[i].transform.childCount == 0)
            {
                return i;
            }
        }
        return -1;
    }

    public void UpdateWeight()
    {
        float Weight = 0;
        float MaxWeight = BaseMaxWeight;

        foreach (InventorySlot invSlot in SlotScripts)
        {
            if (invSlot.transform.childCount != 0)
            {
                InventoryItem ii = invSlot.transform.GetChild(0).GetComponent<InventoryItem>();
                Weight += ii.Weight;
                MaxWeight += ii.MaxWeightIncrease;
            }
        }

        WeightText.SetText("Weight: " + Weight.ToString().PadLeft(3) + "/" + MaxWeight.ToString().PadRight(3) + " (" + (100f * (float)Weight / (float)MaxWeight).ToString().PadLeft(3).Substring(0, 3).Trim(' ') + "%)");
    }

    InventoryItem LastEquippedWeapon = null;
    void EquipFirstAvailableWeapon()
    {
        for(int i = 0; i < SlotScripts.Count; i++)
        {
            if(SlotScripts[i].SlotType == InventoryItem.ItemType.WeaponMain)
            {
                if(SlotScripts[i].transform.childCount != 0)
                {
                    if (LastEquippedWeapon != null)
                    {
                        LastEquippedWeapon.CodeBeforeRemoving();
                    }
                    LastEquippedWeapon = SlotScripts[i].transform.GetChild(0).GetComponent<InventoryItem>();
                    LastEquippedWeapon.CodeAfterEquipping();                    
                    EquippedWeaponIndicator.transform.position = new Vector3(LastEquippedWeapon.transform.position.x, LastEquippedWeapon.transform.position.y, transform.position.z-5);
                    return;
                }
            }
        }
    }

    InventoryItem LastEquippedItem = null;
    void EquipFirstAvailableItem()
    {
        for (int i = 0; i < SlotScripts.Count; i++)
        {
            if (SlotScripts[i].SlotType == InventoryItem.ItemType.ActiveItem)
            {
                if (SlotScripts[i].transform.childCount != 0)
                {
                    if (LastEquippedItem != null)
                    {
                        LastEquippedItem.CodeBeforeRemoving();
                    }
                    LastEquippedItem = SlotScripts[i].transform.GetChild(0).GetComponent<InventoryItem>();
                    LastEquippedItem.CodeAfterEquipping();
                    EquipppedItemIndicator.transform.position = new Vector3(LastEquippedItem.transform.position.x, LastEquippedItem.transform.position.y, transform.position.z - 5);
                    return;
                }
            }
        }
    }

    //call CodeAfterEquipping on all currently equipped clothes
    public void ReEquipClothing()
    {
        foreach (InventorySlot invs in SlotScripts)
        {
            if (invs.SlotType == InventoryItem.ItemType.ClothingHead ||
                invs.SlotType == InventoryItem.ItemType.ClothingJacket ||
                invs.SlotType == InventoryItem.ItemType.ClothingShirt)
            {
                if (invs.transform.childCount != 0)
                {
                    invs.transform.GetChild(0).GetComponent<ClothingItem>().CodeAfterEquipping();
                }
            }
        }
    }

}
