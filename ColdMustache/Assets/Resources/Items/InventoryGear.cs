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
        UniversalReference.PlayerSpriteManager.ResetChosenSprites(new int[]{1,3,7}); //1 shirt 3 coat 7 headwear
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
                    item.transform.localPosition = new Vector3(0, 0, -0.01f);
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

    void Update(){

        if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)){
            if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
                TryEquippingItem(8);
            }
            else{
                TryEquippingWeapon(3);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                TryEquippingItem(9);
            }
            else
            {
                TryEquippingWeapon(4);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                TryEquippingItem(10);
            }
            else
            {
                TryEquippingWeapon(5);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                TryEquippingItem(11);
            }
            else
            {
                TryEquippingWeapon(6);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                TryEquippingItem(12);
            }
            else
            {
                TryEquippingWeapon(7);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            TryEquippingItem(13);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
        {
            TryEquippingItem(14);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
        {
            TryEquippingItem(15);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
        {
            TryEquippingItem(16);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            TryEquippingItem(17);
        }

        if(Input.GetKeyDown(KeyCode.Tab)){
            if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
                CycleItem();
            }
            else{
                CycleWeapon();
            }
        }

        if(!(Input.GetKey(KeyCode.LeftControl)||Input.GetKey(KeyCode.RightControl))){
            if (Input.mouseScrollDelta.y != 0)
            {
                //negative value - scrolling down
                if (Input.mouseScrollDelta.y < 0)
                {
                    int limit = Mathf.RoundToInt(Mathf.Abs(Input.mouseScrollDelta.y));
                    for (int i = 0; i < limit; i++)
                    {
                        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) CycleItem();
                        else CycleWeapon();
                    }
                }
                else
                {
                    int limit = Mathf.RoundToInt(Input.mouseScrollDelta.y);
                    for (int i = 0; i < limit; i++)
                    {
                        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ReverseCycleItem();
                        else ReverseCycleWeapon();
                    }
                }
            }
        }
        

    }

    /*
     *  weapons 1-5 ... [3]-[7]
     *
     *  items 1-10 ... [8]-[17]
     *
     */

    int[] WeaponSlotIndexes = new int[]{3,4,5,6,7};
    int LastEquippedWeaponSlotIndex = -1;
    InventoryItem LastEquippedWeapon = null;
    void EquipFirstAvailableWeapon()
    {
        foreach (int i in WeaponSlotIndexes)
        {
            if (SlotScripts[i].transform.childCount != 0)
            {
                ForceEquipWeapon(i);
                return;
            }
        }
    }
    void TryEquippingWeapon(int index)
    {
        if (SlotScripts[index].transform.childCount != 0)
        {
            ForceEquipWeapon(index);
            return;
        }
    }
    void CycleWeapon(){
        if(EquippedWeaponsCount()<=1){
            return;
        }

        int CurrSlot = LastEquippedWeaponSlotIndex;
        for(int PreventInfiniteCycle = 0; PreventInfiniteCycle < 5; PreventInfiniteCycle++){
            CurrSlot = WeaponSlotIndexIterator(CurrSlot);
            if (SlotScripts[CurrSlot].transform.childCount != 0)
            {
                ForceEquipWeapon(CurrSlot);
                return;
            }
        }

    }
    
    void ReverseCycleWeapon()
    {
        if (EquippedWeaponsCount() <= 1)
        {
            return;
        }

        int CurrSlot = LastEquippedWeaponSlotIndex;
        for (int PreventInfiniteCycle = 0; PreventInfiniteCycle < 5; PreventInfiniteCycle++)
        {
            CurrSlot = ReverseWeaponSlotIndexIterator(CurrSlot);
            if (SlotScripts[CurrSlot].transform.childCount != 0)
            {
                ForceEquipWeapon(CurrSlot);
                return;
            }
        }

    }
    int WeaponSlotIndexIterator(int i){
        if(i<7) return i+1;
        else return 3;
    }
    int ReverseWeaponSlotIndexIterator(int i)
    {
        if (i >3) return i - 1;
        else return 7;
    }
    void ForceEquipWeapon(int SlotId){
        if (LastEquippedWeapon != null)
        {
            LastEquippedWeapon.CodeBeforeRemoving();
        }
        LastEquippedWeaponSlotIndex = SlotId;
        LastEquippedWeapon = SlotScripts[SlotId].transform.GetChild(0).GetComponent<InventoryItem>();
        LastEquippedWeapon.CodeAfterEquipping();
        EquippedWeaponIndicator.transform.position = new Vector3(LastEquippedWeapon.transform.position.x, LastEquippedWeapon.transform.position.y, transform.position.z - 0.03f);
        return;
    }
    int EquippedWeaponsCount(){
        int output = 0;
        foreach (int i in WeaponSlotIndexes)
        {
            if (SlotScripts[i].transform.childCount != 0)
            {
                output++;
            }
        }
        return output;
    }

    int[] ItemSlotIndexes = new int[]{8,9,10,11,12,13,14,15,16,17};
    int LastEquippedItemSlotIndex = -1;
    InventoryItem LastEquippedItem = null;
    void EquipFirstAvailableItem()
    {
        for (int i = 0; i < SlotScripts.Count; i++)
        {
            if (SlotScripts[i].SlotType == InventoryItem.ItemType.ActiveItem)
            {
                if (SlotScripts[i].transform.childCount != 0)
                {
                    ForceEquipItem(i);
                    return;
                }
            }
        }
    }
    
    void TryEquippingItem(int index)
    {
        if (SlotScripts[index].transform.childCount != 0)
        {
            ForceEquipItem(index);
            return;
        }
    }

    void CycleItem()
    {
        if (EquippedItemsCount() <= 1)
        {
            return;
        }

        int CurrSlot = LastEquippedItemSlotIndex;
        for (int PreventInfiniteCycle = 0; PreventInfiniteCycle < 10; PreventInfiniteCycle++)
        {
            CurrSlot = ItemSlotIndexIterator(CurrSlot);
            if (SlotScripts[CurrSlot].transform.childCount != 0)
            {
                ForceEquipItem(CurrSlot);
                return;
            }
        }

    }
    void ReverseCycleItem()
    {
        if (EquippedItemsCount() <= 1)
        {
            return;
        }

        int CurrSlot = LastEquippedItemSlotIndex;
        for (int PreventInfiniteCycle = 0; PreventInfiniteCycle < 10; PreventInfiniteCycle++)
        {
            CurrSlot = ReverseItemSlotIndexIterator(CurrSlot);
            if (SlotScripts[CurrSlot].transform.childCount != 0)
            {
                ForceEquipItem(CurrSlot);
                return;
            }
        }

    }
    int ItemSlotIndexIterator(int i)
    {
        if (i < 17) return i + 1;        
        else return 8;
    }
    int ReverseItemSlotIndexIterator(int i)
    {
        if (i >8) return i - 1;
        else return 17;
    }
        
    void ForceEquipItem(int SlotId)
    {
        if (LastEquippedItem != null)
        {
            LastEquippedItem.CodeBeforeRemoving();
        }
        LastEquippedItemSlotIndex = SlotId;
        LastEquippedItem = SlotScripts[SlotId].transform.GetChild(0).GetComponent<InventoryItem>();
        LastEquippedItem.CodeAfterEquipping();
        EquipppedItemIndicator.transform.position = new Vector3(LastEquippedItem.transform.position.x, LastEquippedItem.transform.position.y, transform.position.z - 0.03f);
        return;
    }

    int EquippedItemsCount()
    {
        int output = 0;
        foreach (int i in ItemSlotIndexes)
        {
            if (SlotScripts[i].transform.childCount != 0)
            {
                output++;
            }
        }
        return output;
    }


    public void RefillAllActiveItems(){
        foreach(int i in ItemSlotIndexes){
            if(SlotScripts[ItemSlotIndexes[i]].transform.childCount!=0){
                SlotScripts[ItemSlotIndexes[i]].transform.GetChild(0).GetComponent<ActiveItem>().Refill();
            }
        }
    }

}
