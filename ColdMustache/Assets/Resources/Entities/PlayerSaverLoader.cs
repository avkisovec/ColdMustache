using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerSaverLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //quicksave
        if (Input.GetKeyUp(KeyCode.F5))
        {
            string FilePath;
            StreamWriter sw;

            //save sprites
            FilePath = Application.dataPath+"/Save/PlayerSprites.save";
            SaverLoader.CreateHardPathIfNeeded(FilePath);
            sw = new StreamWriter(FilePath);
            foreach(string s in GetComponent<SpriteManagerGeneric>().SaveCurrentAsPreset())
            {
                sw.WriteLine(s);
            }
            sw.Close();

            //save inventory
            FilePath = Application.dataPath + "/Save/PlayerInventory.save";
            SaverLoader.CreateHardPathIfNeeded(FilePath);
            sw = new StreamWriter(FilePath);
            Inventory inventory = UniversalReference.PlayerInventory;
            foreach (InventorySlot invs in inventory.SlotsScripts)
            {
                sw.Write(invs.SlotId + ";");

                //this slot has no child
                if(invs.transform.childCount == 0)
                {
                    sw.Write("EmptySlot");
                }
                else
                {
                    sw.Write(invs.transform.GetChild(0).GetComponent<InventoryItem>().PrefabPath);
                }

                sw.WriteLine();
            }
            sw.Close();

            //Debug.Log("Quicksave Complete.");
        }

        //quickload
        if (Input.GetKeyUp(KeyCode.F6))
        {
            if(File.Exists(Application.dataPath + "/Save/PlayerSprites.save"))
            {
                RandomLoaderGeneric.Load(GetComponent<SpriteManagerGeneric>(), Application.dataPath + "/Save/PlayerSprites.save");
            }

            if (File.Exists(Application.dataPath + "/Save/PlayerInventory.save"))
            {
                Inventory inv = UniversalReference.PlayerInventory;
                inv.ForceClearInventory();

                StreamReader sr = new StreamReader(Application.dataPath + "/Save/PlayerInventory.save");
                string line = "";
                while(line != null)
                {
                    if(line.Length >= 3)
                    {
                        string[] split = line.Split(';');
                        if(split.Length >= 2)
                        {
                            if(split[1] != "EmptySlot")
                            {
                                GameObject item = Instantiate(Resources.Load<GameObject>(split[1]) as GameObject);
                                inv.AddItemToSpecificSlot(item, int.Parse(split[0]));
                            }
                        }
                    }

                    line = sr.ReadLine();
                }

                inv.ReEquipClothing();

            }

            //Debug.Log("Quickload Complete.");
        }

	}
}
