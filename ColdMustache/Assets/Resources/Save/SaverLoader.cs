using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaverLoader : MonoBehaviour {
    
    public static string[] ReadAFile(string path)
    {
        List<string> output = new List<string>();

        StreamReader sr = new StreamReader(path);
        string line = sr.ReadLine();
        while(line != null)
        {
            output.Add(line);

            line = sr.ReadLine();
        }

        sr.Close();

        return output.ToArray(); 
    }

    //file path = desired path (yourApplicationFolder/...)
    //default path = where the source file is stored within Resources (assets/resources/ is implied)
    public static void CheckFileAndCreateIfNeeded(string FilePath, string DefaultPath)
    {
        FilePath = Application.dataPath + "/" + FilePath;
        FileInfo fileInfo = new FileInfo(FilePath);

        if (!fileInfo.Exists)
        {
            Directory.CreateDirectory(fileInfo.Directory.FullName);
            StreamWriter sw = new StreamWriter(FilePath);
            sw.Write(Resources.Load<TextAsset>(DefaultPath).text);
            sw.Close();
        }
    }

    //hard path means Drive:/path - the complete path, not relative to application directory
    //usage - include a filename (drive:/path/path/file) - it can be made up some.thing but it should be there
    public static void CreateHardPathIfNeeded(string FilePath)
    {
        FileInfo fileInfo = new FileInfo(FilePath);
        if (!Directory.Exists(fileInfo.Directory.FullName))
        {
            Directory.CreateDirectory(fileInfo.Directory.FullName);
        }
    }
        
    public static void QuickSave(SpriteManagerGeneric spriteManagerGeneric)
    {
        string FilePath;
        StreamWriter sw;

        //save sprites
        FilePath = Application.dataPath + "/Save/PlayerSprites.save";
        SaverLoader.CreateHardPathIfNeeded(FilePath);
        sw = new StreamWriter(FilePath);
        foreach (string s in spriteManagerGeneric.SaveCurrentAsPreset())
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
            if (invs.transform.childCount == 0)
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
    }

    public static void QuickLoad(SpriteManagerGeneric spriteManagerGeneric)
    {
        if (File.Exists(Application.dataPath + "/Save/PlayerSprites.save"))
        {
            RandomLoaderGeneric.Load(spriteManagerGeneric, Application.dataPath + "/Save/PlayerSprites.save");
        }

        if (File.Exists(Application.dataPath + "/Save/PlayerInventory.save"))
        {
            Inventory inv = UniversalReference.PlayerInventory;
            inv.ForceClearInventory();

            StreamReader sr = new StreamReader(Application.dataPath + "/Save/PlayerInventory.save");
            string line = "";
            while (line != null)
            {
                if (line.Length >= 3)
                {
                    string[] split = line.Split(';');
                    if (split.Length >= 2)
                    {
                        if (split[1] != "EmptySlot")
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
    }

}
