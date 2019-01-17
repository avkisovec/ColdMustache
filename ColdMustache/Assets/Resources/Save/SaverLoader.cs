using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaverLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyUp(KeyCode.V)){
            LoadInventory();
        }

        /*
        if (Input.GetKeyUp(KeyCode.X))
        {
            //looks like it doesnt work
            StreamWriter sw = new StreamWriter("Assets/Resources/Save/newFileMate.save");
            sw.WriteLine("eh");
            sw.Close();

        }
        */
	}

    public static void LoadInventory()
    {
        UniversalReference.PlayerInventory.LoadItemsFromPaths(ReadAFile("Assets/Resources/Save/inventory.save"));
    }

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
    public static void CreateHardPathIfNeeded(string FilePath)
    {
        FileInfo fileInfo = new FileInfo(FilePath);
        if (!Directory.Exists(fileInfo.Directory.FullName))
        {
            Directory.CreateDirectory(fileInfo.Directory.FullName);
        }
    }

}
