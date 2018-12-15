using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiReference : MonoBehaviour {

    public static Image AmmoCounter;
    public static Image WeaponStatus;

	// Use this for initialization
	void Start () {
        AmmoCounter = GameObject.Find("Canvas/AmmoCounter").GetComponent<Image>();
        WeaponStatus = GameObject.Find("Canvas/WeaponStatus").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
