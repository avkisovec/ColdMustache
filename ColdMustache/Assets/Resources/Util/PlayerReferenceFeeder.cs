using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReferenceFeeder : MonoBehaviour {

    //this script gives PlayerReference reference to player
    //part of ScriptHolder

	// Use this for initialization
	void Start () {
        PlayerReference.PlayerObject = GameObject.Find("PlayerContainer");
        PlayerReference.PlayerScript = PlayerReference.PlayerObject.GetComponent<Player>();
	}
    
}
