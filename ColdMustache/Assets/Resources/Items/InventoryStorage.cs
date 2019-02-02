using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryStorage : InventoryBase {

    //  SLOTS SCRIPTS ARE INHERITED
    //public List<InventorySlot> SlotsScripts = new List<InventorySlot>();

    public Storage ParentStorageObject;

    // Use this for initialization
    void Start()
    {
        SlotScripts = SlotScripts.OrderBy(o => o.SlotId).ToList();

        LoadItemsFromPaths(ParentStorageObject.ItemPaths);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void ReportSlotBeingClicked(int id)
    {
        GameObject itemObject;
        
        //non-empty special slot was clicked - move item to first empty non-special slot ("everything" slot)
        if (!IsSlotEmpty(GetListIdOfSlotWithId(id)))
        {
            //the item being moved
            itemObject = ItemOnSlot(GetSlotWithId(id));

            UniversalReference.PlayerInventory.AddItem(itemObject);
            
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

    public int[] GetAllListIdOfSlotWithId(int slotId)
    {
        List<int> output = new List<int>();

        for (int i = 0; i < SlotScripts.Count; i++)
        {
            if (SlotScripts[i].SlotId == slotId)
            {
                output.Add(i);
            }
        }
        return output.ToArray();
    }
    

    public bool IsSlotEmpty(int ListId)
    {
        if (SlotScripts[ListId].transform.childCount == 0)
        {
            return true;
        }
        return false;
    }


    public int FindListIdOfFirstEmptySlot()
    {
        for (int i = 0; i < SlotScripts.Count; i++)
        {
            if (IsSlotEmpty(i))
            {
                return i;
            }
        }
        return -1;
    }
    
    


    InventorySlot GetSlotWithId(int id)
    {
        return SlotScripts[GetListIdOfSlotWithId(id)];
    }

    GameObject ItemOnSlot(InventorySlot inventorySlot)
    {
        return inventorySlot.transform.GetChild(0).gameObject;
    }
    

    public void AddItem(GameObject item)
    {
        item.transform.parent = SlotScripts[FindListIdOfFirstEmptySlot()].transform;        
        item.transform.localPosition = new Vector3(0, 0, -1);
        item.transform.localScale = new Vector3(1, 1, 1);
    }

    public void LoadItemsFromPaths(string[] paths)
    {
        foreach (string s in paths)
        {
            AddItem(Instantiate(Resources.Load<GameObject>(s) as GameObject));
        }
    }


    public void SaveBeforeClosing()
    {
        List<string> ItemsRemaining = new List<string>();

        for (int i = 0; i < SlotScripts.Count; i++)
        {
            if (!IsSlotEmpty(i))
            {
                ItemsRemaining.Add(ItemOnSlot(SlotScripts[i]).GetComponent<InventoryItem>().PrefabPath);
            }
        }

        ParentStorageObject.ItemPaths = ItemsRemaining.ToArray();
    }





}
