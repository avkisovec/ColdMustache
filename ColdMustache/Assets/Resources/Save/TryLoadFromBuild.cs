using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TryLoadFromBuild : MonoBehaviour {

	// Use this for initialization
	void Start () {
        /*
        GetComponent<Text>().text = Application.dataPath;
        GetComponent<Text>().text += ";   " + SaverLoader.ReadAFile(Application.dataPath+"/Resources/Save/TryLoad.txt")[0];
        //GetComponent<Text>().text = Application.dataPath;
        */
        TextAsset ta = Resources.Load<TextAsset>("Save/TryLoad");
        GetComponent<Text>().text = ta.text;

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyUp(KeyCode.W))
        {
                /*
                 * 
                 * 
                 * use streamwriter in conjunction with the textasset - load text asset and write into it w sw
                 * 
                 * 
                 * 
                 * 
                 * 
                 * 
                 */
        }

	}
}
