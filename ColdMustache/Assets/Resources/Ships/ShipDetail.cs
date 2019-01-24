using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDetail : MonoBehaviour {

    public Sprite sprite;

    GameObject detail;

    public Transform Player;

    public float Distance = 80;

    SpriteRenderer DetailSr;

	// Use this for initialization
	void Start () {

        detail = new GameObject();
        detail.transform.parent = transform;
        detail.AddComponent<StayFixedRotation>();
        DetailSr = detail.AddComponent<SpriteRenderer>();
        DetailSr.sprite = sprite;

        detail.AddComponent<StayFixedSizeSpace>().PixelScale = 0.2f;

	}
	
	// Update is called once per frame
	void Update () {

        detail.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        
        if(Player.position.x > transform.position.x)
        {
            DetailSr.flipX = true;
        }
        else
        {
            DetailSr.flipX = false;
        }

        if(Player.position.y > transform.position.y)
        {
            DetailSr.flipY = true;
        }
        else
        {
            DetailSr.flipY = false;
        }


	}
}
