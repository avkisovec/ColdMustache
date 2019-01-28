using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPointerSpawner : MonoBehaviour {

    /*
     * spawns pointer for this ship
     * 
     */

    public Transform PlayerShip;

	// Use this for initialization
	void Start () {

        GameObject pointer = new GameObject();
        pointer.transform.parent = PlayerShip;

        pointer.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Ships/IconEnemyPointer");

        pointer.AddComponent<EnemyPointer>().Ship = transform;

        pointer.AddComponent<StayFixedSizeSpace>().PixelScale = 2;
        

	}
	
}
