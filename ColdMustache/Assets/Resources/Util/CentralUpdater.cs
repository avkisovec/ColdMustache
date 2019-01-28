using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralUpdater : MonoBehaviour {

    /*
     * Update() function is cpu expensive, as it does some checks before each call
     * 
     * for simpler objects that wont be destroyed (such as railcarts and other small background decorative objects)
     * it is faster to update them all centrally from here instead of every individually
     * 
     * also in case of low performance these objects can be update every other or every fourth frame...
     * or possibly disabled entirely on low performance machines
     * 
     */

    public static List<CentrallyUpdatable> Scripts = new List<CentrallyUpdatable>();
    
	void Update () {
        int count = Scripts.Count;
		for(int i = 0; i < count; i++)
        {
            Scripts[i].CentralUpdate();
        }
	}
}
