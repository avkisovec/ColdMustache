﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class InventoryArmory : InventoryBase
{

    public Window MyWindow;
    public Window InventoryGearWindow;
    public InventoryGear InventoryGearScript;

    public float BaseMaxWeight = 40;

    public List<InventorySlot> GearSlotScripts = new List<InventorySlot>();

    public TextObject WeightText;

    public Transform SaveAndExitBtn;

    // Start is called before the first frame update
    void Start()
    {
        ReloadArmory();
    }

    public void ReloadArmory(){

        SlotScripts.Clear();

        List<Transform> ToDie = new List<Transform>();
        for(int i = 0; i < transform.childCount; i++){
            ToDie.Add(transform.GetChild(i));
        }
        Transform GearSlots = transform.parent.Find("GearSlots");
        for (int i = 0; i < GearSlots.childCount; i++)
        {
            try{
                ToDie.Add(GearSlots.GetChild(i).GetChild(0));
            }catch{}
        }

        foreach(Transform tr in ToDie){
            tr.parent = null;
            Destroy(tr.gameObject);
        }


        GenerateFromFile();

        SlotScripts = SlotScripts.OrderBy(o => o.SlotId).ToList();

        GearSlotScripts = GearSlotScripts.OrderBy(o => o.SlotId).ToList();

        LoadGearFromFile();

        UpdateWeight();
    }

    public void LoadGearFromFile()
    {
        foreach (string s in SaverLoader.ReadAFile(Application.dataPath + "/Save/Gear.save"))
        {
            string[] data = s.Split(';');
            if (data.Length >= 2)
            {
                if (data[1] != "EmptySlot")
                {
                    GameObject item = Instantiate(Resources.Load<GameObject>(data[1]) as GameObject);
                    item.transform.parent = GearSlotScripts[int.Parse(data[0])].transform;
                    item.transform.localPosition = new Vector3(0, 0, -1);
                    item.transform.localScale = new Vector3(1, 1, 1);
                }
            }

        }
    }

    public void GenerateFromFile()
    {
        float LeftmostX = 8.4f;
        float RightmostX = 14;
        float UpmostY = -2.4f;

        float XShift = 1.2f;
        float Yshift = -1.2f;

        float X = LeftmostX;
        float Y = UpmostY;

        Sprite SlotSprite = Resources.Load<Sprite>("Gui/Menus/InventorySlot");

        foreach (string s in SaverLoader.ReadAFile(Application.dataPath + "/Save/Armory.save"))
        {
            string[] line = s.Split(';');

            GameObject slot = new GameObject();
            slot.transform.parent = transform;
            slot.transform.localPosition = new Vector3(X, Y, -0.1f);
            slot.transform.localScale = new Vector3(1,1,1);
            slot.AddComponent<SpriteRenderer>().sprite = SlotSprite;
            
            InventorySlot slotScript = slot.AddComponent<InventorySlot>();
            slotScript.SlotId = int.Parse(line[0]);
            slotScript.SlotType = InventoryItem.ItemType.Undefined;
            slotScript.ParentInventory = this;
            SlotScripts.Add(slotScript);

            GameObject ItemOnSlot = Instantiate(Resources.Load<GameObject>(line[1]) as GameObject);
            ItemOnSlot.transform.parent = slot.transform;
            ItemOnSlot.transform.localPosition = new Vector3(0, 0, -0.1f);
            ItemOnSlot.transform.localScale = new Vector3(1, 1, 1);
            
            if (X + XShift < RightmostX)
            {
                X += XShift;
            }
            else
            {
                X = LeftmostX;
                Y += Yshift;
            }

        }
    }

    public override void ReportSlotBeingClicked(int id)
    {
        //gear slot was clicked
        if(id < 1000)
        {
            int ListId = GetListIdOfGearSlotWithId(id);
            if(GearSlotScripts[ListId].transform.childCount != 0)
            {
                //removing it first before destroying it, as destruction needs some time and i need to calculate the new weight without the item
                //if i didnt remove it from parent it would still be included in weight calculation
                Transform tr = GearSlotScripts[ListId].transform.GetChild(0);
                tr.parent = null;
                Destroy(tr.gameObject);
                UpdateWeight();
            }
        }
        else
        {
            InventoryItem InvItem = SlotScripts[GetListIdOfSlotWithId(id)].transform.GetChild(0).GetComponent<InventoryItem>();
            int GearSlotListId = FindListIdOfFirstEmptyGearSlot(InvItem.Type);
            if (GearSlotListId != -1)
            {
                GameObject Item = Instantiate(Resources.Load<GameObject>(InvItem.PrefabPath) as GameObject);
                Item.transform.parent = GearSlotScripts[GearSlotListId].transform;
                Item.transform.localPosition = new Vector3(0, 0, -1);
                Item.transform.localScale = new Vector3(1, 1, 1);
                UpdateWeight();
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

    public int GetListIdOfGearSlotWithId(int slotId)
    {
        for (int i = 0; i < GearSlotScripts.Count; i++)
        {
            if (GearSlotScripts[i].SlotId == slotId)
            {
                return i;
            }
        }
        return -1; //means "not found"
    }

    public int FindListIdOfFirstEmptyGearSlot(InventoryItem.ItemType type = InventoryItem.ItemType.Undefined)
    {
        for (int i = 0; i < GearSlotScripts.Count; i++)
        {
            if (GearSlotScripts[i].SlotType == type && GearSlotScripts[i].transform.childCount==0)
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

        foreach (InventorySlot invSlot in GearSlotScripts)
        {
            if (invSlot.transform.childCount != 0)
            {
                InventoryItem ii = invSlot.transform.GetChild(0).GetComponent<InventoryItem>();
                Weight += ii.Weight;
                MaxWeight += ii.MaxWeightIncrease;
            }
        }
        
        WeightText.SetText("Weight: "+Weight.ToString().PadLeft(3)+"/"+MaxWeight.ToString().PadRight(3) + " ("+(100f*(float)Weight/(float)MaxWeight).ToString().PadLeft(3).Substring(0, 3).Trim(' ') + "%)");
    }

    public bool IsWeightUnder100Percent(){
        float Weight = 0;
        float MaxWeight = BaseMaxWeight;

        foreach (InventorySlot invSlot in GearSlotScripts)
        {
            if (invSlot.transform.childCount != 0)
            {
                InventoryItem ii = invSlot.transform.GetChild(0).GetComponent<InventoryItem>();
                Weight += ii.Weight;
                MaxWeight += ii.MaxWeightIncrease;
            }
        }
        if(Weight <= MaxWeight){
            return true;
        }
        else{
            return false;
        }
    }

    private void Update()
    {
        if (MyWindow.AmIActive)
        {
            if (InventoryGearWindow.AmIActive)
            {
                InventoryGearWindow.VisibleCoordinates = InventoryGearWindow.transform.position;
                InventoryGearWindow.transform.position = InventoryGearWindow.HidingCoordinates;
                InventoryGearWindow.AmIActive = false;
            }


            Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetKeyDown(KeyCode.Mouse0) &&
                MouseWorldPos.x > SaveAndExitBtn.position.x - (SaveAndExitBtn.lossyScale.x / 2) &&
                MouseWorldPos.x < SaveAndExitBtn.position.x + (SaveAndExitBtn.lossyScale.x / 2) &&
                MouseWorldPos.y > SaveAndExitBtn.position.y - (SaveAndExitBtn.lossyScale.y / 2) &&
                MouseWorldPos.y < SaveAndExitBtn.position.y + (SaveAndExitBtn.lossyScale.y / 2)
                )
            {
                SaveAndExit();
            }
        }

    }

    public void SaveAndExit()
    {
        //if player has no weapon, he will het the first one (on slot if 22001)
        int EquippedWeapons = 0;
        for(int i = 0; i <GearSlotScripts.Count; i++){
            if(GearSlotScripts[i].SlotType == InventoryItem.ItemType.WeaponMain){
                if(GearSlotScripts[i].transform.childCount!=0){
                    EquippedWeapons++;
                }
            }
        }
        if (EquippedWeapons == 0)
        {
            AlphabetManager.SpawnFloatingText("You need a weapon.", new Vector3(UniversalReference.MouseWorldPos.x, UniversalReference.MouseWorldPos.y, -60));
            return;
        }

        if(!IsWeightUnder100Percent()){
            AlphabetManager.SpawnFloatingText("You are too heavy.", new Vector3(UniversalReference.MouseWorldPos.x, UniversalReference.MouseWorldPos.y, -60));
            return;
        }
        

        StreamWriter sw = new StreamWriter(Application.dataPath + "/Save/Gear.save");

        for(int i = 0; i < GearSlotScripts.Count; i++)
        {
            sw.Write(GearSlotScripts[i].SlotId + ";");

            if (GearSlotScripts[i].transform.childCount != 0)
            {
                sw.Write(GearSlotScripts[i].transform.GetChild(0).GetComponent<InventoryItem>().PrefabPath);
            }
            else
            {
                sw.Write("EmptySlot");
            }
            sw.WriteLine();
        }

        sw.Close();


        InventoryGearScript.ReloadInventory();
        InventoryGearWindow.transform.position = InventoryGearWindow.VisibleCoordinates;
        InventoryGearWindow.AmIActive = true;

        //Destroy(transform.parent.gameObject);
        transform.parent.GetComponent<Window>().Hide();
    }

     
}
