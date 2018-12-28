using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : InventoryBase {

    //  SLOTS SCRIPTS ARE INHERITED
    //public List<InventorySlot> SlotsScripts = new List<InventorySlot>();

    public Transform StorageExchangeOverlay;

    // Use this for initialization
    void Start () {
        SlotsScripts = SlotsScripts.OrderBy(o => o.SlotId).ToList();

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            CycleMainWeapons();
        }

	}

    public override void ReportSlotBeingClicked(int id)
    {
        GameObject itemObject;
        InventorySlot SlotScript = SlotsScripts[GetListIdOfSlotWithId(id)];

        //moving to a storage
        if (IsStorageOpen)
        {
            if(SlotScript.SlotType == InventoryItem.ItemType.Everything)
            {
                //non-empty everything slot was clicked
                if (!IsSlotEmpty(GetListIdOfSlotWithId(id)))
                {
                    itemObject = ItemOnSlot(GetSlotWithId(id));
                    OpenStorage.AddItem(itemObject);
                }
                return;

            }  
        }

        //moving to and or from special slots
        else
        {
            switch (SlotScript.SlotType)
            {
                case InventoryItem.ItemType.ClothingShirt: //special slots (equip) -> go to first empty normal slot
                case InventoryItem.ItemType.ClothingJacket:
                case InventoryItem.ItemType.ClothingHead:
                case InventoryItem.ItemType.WeaponSide:
                case InventoryItem.ItemType.WeaponMain:

                    //non-empty special slot was clicked - move item to first empty non-special slot ("everything" slot)
                    if (!IsSlotEmpty(GetListIdOfSlotWithId(id)))
                    {
                        //the item being moved
                        itemObject = ItemOnSlot(GetSlotWithId(id));

                        if (SlotScript.SlotType != InventoryItem.ItemType.WeaponMain || id == LastSelectedWeaponListId)
                        {
                            itemObject.GetComponent<InventoryItem>().CodeBeforeRemoving();
                        }

                        itemObject.transform.parent = SlotsScripts[FindListIdOfFirstEmptySlot(InventoryItem.ItemType.Everything)].transform;

                        itemObject.transform.localPosition = new Vector3(0, 0, -1);

                        //youre just removed selected weapon
                        if (SlotScript.SlotType == InventoryItem.ItemType.WeaponMain && id == LastSelectedWeaponListId)
                        {
                            SelectAvailableWeapon(); //finds some other weapon
                        }
                    }

                    return;

                case InventoryItem.ItemType.Everything: //normal slots (storage) -> go to corresponding special slot (and clear that one)

                    //non-empty everything slot was clicked
                    if (!IsSlotEmpty(GetListIdOfSlotWithId(id)))
                    {
                        //the item being moved
                        itemObject = ItemOnSlot(GetSlotWithId(id));

                        //new thing is a weapon
                        if (itemObject.GetComponent<InventoryItem>().Type == InventoryItem.ItemType.WeaponMain)
                        {
                            InventoryItem.ItemType ObjectType = itemObject.GetComponent<InventoryItem>().Type;

                            //go through every slots of correct type
                            foreach (int listId in GetAllListIdOfSlotType(ObjectType))
                            {
                                if (IsSlotEmpty(listId))
                                {
                                    //empty slot was found, put new thing there (and nothing more)
                                    itemObject.transform.parent = SlotsScripts[listId].transform;
                                    itemObject.transform.localPosition = new Vector3(0, 0, -1);
                                    return;
                                }
                            }

                            //no empty special slot found, gotta remove something first                        
                            int NewSlotListId = FindListIdOfFirstSlot(ObjectType);

                            //if the thing im removing was selected as active weapon
                            //its CodeBeforeRemoving has to be called now, but not from here
                            //its CodeBeforeRemoving will be called during the switch, in ReportSlotBeingClicked

                            //go to the new slot
                            itemObject.transform.parent = SlotsScripts[NewSlotListId].transform;
                            itemObject.transform.localPosition = new Vector3(0, 0, -1);

                            //move the previously equipped item to some empty slot
                            ReportSlotBeingClicked(SlotsScripts[NewSlotListId].SlotId);

                            //previous thing was selected, so new thing is selected aswell
                            if (NewSlotListId == LastSelectedWeaponListId)
                            {
                                CodeAfterEquipping(SlotsScripts[NewSlotListId]);
                            }

                            return;
                        }
                        else
                        {


                            //new thing is not weapon

                            //new object                  
                            InventoryItem.ItemType ObjectType = itemObject.GetComponent<InventoryItem>().Type;

                            foreach (int listId in GetAllListIdOfSlotType(ObjectType))
                            {
                                if (IsSlotEmpty(listId))
                                {
                                    //empty slot was found, put new thing there

                                    itemObject.transform.parent = SlotsScripts[listId].transform;
                                    itemObject.transform.localPosition = new Vector3(0, 0, -1);

                                    itemObject.GetComponent<InventoryItem>().CodeAfterEquipping();

                                    return;
                                }
                            }

                            //no empty special slot found, gotta remove something first

                            //bool WasTheSlotAlreadyOccupied = !IsSlotEmpty(GetListIdOfSlotWithId(ObjectType));

                            int NewSlotListId = FindListIdOfFirstSlot(ObjectType);

                            itemObject.transform.parent = SlotsScripts[NewSlotListId].transform;
                            itemObject.transform.localPosition = new Vector3(0, 0, -1);

                            ReportSlotBeingClicked(SlotsScripts[NewSlotListId].SlotId); //to move the previously equipped item to the now empty slot;

                            itemObject.GetComponent<InventoryItem>().CodeAfterEquipping();
                        }
                    }
                    return;
            }

        }
        
    }

    public void NoWeaponEquipped()
    {
        foreach (Weapon w in UniversalReference.PlayerObject.GetComponents<Weapon>())
        {
            Destroy(w);
        }
        UniversalReference.GunRotator.GunSpriteRenderer.sprite = UniversalReference.WeaponStatus.sprite = UniversalReference.AmmoCounter.sprite = Resources.Load<Sprite>("EmptyPixel");
    }
    
    public int GetListIdOfSlotWithId(int slotId)
    {
        for(int i = 0; i < SlotsScripts.Count; i++)
        {
            if(SlotsScripts[i].SlotId == slotId)
            {
                return i;
            }
        }
        return -1; //means "not found"
    }

    public int[] GetAllListIdOfSlotWithId(int slotId)
    {
        List<int> output = new List<int>();

        for (int i = 0; i < SlotsScripts.Count; i++)
        {
            if(SlotsScripts[i].SlotId == slotId)
            {
                output.Add(i);
            }
        }
        return output.ToArray();
    }

    public int[] GetAllListIdOfSlotType(InventoryItem.ItemType type)
    {
        List<int> output = new List<int>();

        for (int i = 0; i < SlotsScripts.Count; i++)
        {
            if (SlotsScripts[i].SlotType == type)
            {
                output.Add(i);
            }
        }
        return output.ToArray();
    }

    public int[] GetAllListIdOfSlotType_notEmpty(InventoryItem.ItemType type)
    {
        List<int> output = new List<int>();

        for (int i = 0; i < SlotsScripts.Count; i++)
        {
            if (SlotsScripts[i].SlotType == type && !IsSlotEmpty(i))
            {
                output.Add(i);
            }
        }
        return output.ToArray();
    }

    public bool IsSlotEmpty(int ListId)
    {
        if (SlotsScripts[ListId].transform.childCount == 0)
        {
            return true;
        }
        return false;
    }

    public int FindListIdOfFirstEmptySlot(InventoryItem.ItemType type = InventoryItem.ItemType.Undefined)
    {
        for (int i = 0; i < SlotsScripts.Count; i++)
        {
            if (SlotsScripts[i].SlotType == type && IsSlotEmpty(i))
            {
                return i;
            }
        }
        return -1;
    }

    public int FindListIdOfFirstSlot(InventoryItem.ItemType type = InventoryItem.ItemType.Undefined)
    {
        for (int i = 0; i < SlotsScripts.Count; i++)
        {
            if (SlotsScripts[i].SlotType == type)
            {
                return i;
            }
        }
        return -1;
    }

    

    int[] MainWeaponSlotsListIndexes = { 4, 5, 6 };

    int LastSelectedWeaponListId = 4;

    void CycleMainWeapons()
    {
        int SelectedMainWeaponLocalIndex = 0;

        if (LastSelectedWeaponListId == 4)
        {
            SelectedMainWeaponLocalIndex = 0;
        }
        if (LastSelectedWeaponListId == 5)
        {
            SelectedMainWeaponLocalIndex = 1;
        }
        if (LastSelectedWeaponListId == 6)
        {
            SelectedMainWeaponLocalIndex = 2;
        }

        int OccupiedWeaponSlots = 0;

        foreach(int i in MainWeaponSlotsListIndexes)
        {
            if (!IsSlotEmpty(i))
            {
                OccupiedWeaponSlots++;
            }
        }

        //if there are no weapons, cycling has no sense (in fact it will cause infinite cycle)
        if(OccupiedWeaponSlots == 0)
        {
            return;
        }
        if (OccupiedWeaponSlots == 1)
        {
            SelectAvailableWeapon();
            return;
        }

        while (true)
        {
            SelectedMainWeaponLocalIndex++;

            //when you reach end of array, wrap around
            if (SelectedMainWeaponLocalIndex >= MainWeaponSlotsListIndexes.Length)
            {
                SelectedMainWeaponLocalIndex -= MainWeaponSlotsListIndexes.Length;
            }

            //new weapon found - job's done
            if (!IsSlotEmpty(MainWeaponSlotsListIndexes[SelectedMainWeaponLocalIndex]))
            {
                SlotsScripts[ LastSelectedWeaponListId ].transform.GetChild(0).GetComponent<InventoryItem>().CodeBeforeRemoving();
                SlotsScripts[ MainWeaponSlotsListIndexes[ SelectedMainWeaponLocalIndex ] ].transform.GetChild(0).GetComponent<InventoryItem>().CodeAfterEquipping();

                LastSelectedWeaponListId = MainWeaponSlotsListIndexes[SelectedMainWeaponLocalIndex];

                return;
            }
        }
    }
    
    //script is for cases where player unequips his currently held weapon, so he jumps to first one available
    void SelectAvailableWeapon()
    {
        foreach(int i in MainWeaponSlotsListIndexes)
        {
            if (!IsSlotEmpty(i))
            {
                LastSelectedWeaponListId = i;
                CodeAfterEquipping(SlotsScripts[i]);
            }
        }
    }


    InventorySlot GetSlotWithId(int id)
    {
        return SlotsScripts[GetListIdOfSlotWithId(id)];
    }

    GameObject ItemOnSlot(InventorySlot inventorySlot)
    {
        return inventorySlot.transform.GetChild(0).gameObject;
    }

    void CodeBeforeRemoving(InventorySlot inventorySlot)
    {
        inventorySlot.transform.GetChild(0).GetComponent<InventoryItem>().CodeBeforeRemoving();
    }
    void CodeAfterEquipping(InventorySlot inventorySlot)
    {
        inventorySlot.transform.GetChild(0).GetComponent<InventoryItem>().CodeAfterEquipping();
    }


    public void AddItem(GameObject item)
    {
        item.transform.parent = SlotsScripts[FindListIdOfFirstEmptySlot(InventoryItem.ItemType.Everything)].transform;
        item.transform.localPosition = new Vector3(0, 0, -1);
        item.transform.localScale = new Vector3(1, 1, 1);
    }




    //saving loading

    public void LoadItemsFromPaths(string[] paths)
    {
        foreach (string s in paths)
        {
            AddItem(Instantiate(Resources.Load<GameObject>(s) as GameObject));
        }
    }




    //storage 

    public bool IsStorageOpen = false;
    public InventoryStorage OpenStorage = null;
    
    public void UpdateStorageState()
    {
        if(OpenStorage == null)
        {
            IsStorageOpen = false;
            StorageExchangeOverlay.transform.localPosition = new Vector3(StorageExchangeOverlay.transform.localPosition.x, StorageExchangeOverlay.transform.localPosition.y, 10);
        }
        else
        {
            IsStorageOpen = true;
            StorageExchangeOverlay.transform.localPosition = new Vector3(StorageExchangeOverlay.transform.localPosition.x, StorageExchangeOverlay.transform.localPosition.y, -10);
        }
    }


}
