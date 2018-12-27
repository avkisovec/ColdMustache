using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalReferanceIterator : MonoBehaviour {

    //normal universal reference runs before all other scripts

    //this iterator is for cases where you need to run after other scripts

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        UniversalReference.MouseWorldPos = UniversalReference.MainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
