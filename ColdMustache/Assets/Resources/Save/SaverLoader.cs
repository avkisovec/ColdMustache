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

	}

    public static void LoadInventory()
    {
        foreach(string s in ReadAFile("Assets/Resources/Save/inventory.save"))
        {
            Debug.Log(s);
            UniversalReference.PlayerInventory.AddItem( Instantiate(Resources.Load<GameObject>(s) as GameObject) );
        }
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

}
