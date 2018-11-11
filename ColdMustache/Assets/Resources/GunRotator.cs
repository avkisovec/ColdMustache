using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotator : MonoBehaviour {

    Transform tr;
    public SpriteRenderer GunSprite;

	// Use this for initialization
	void Start () {
        tr = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	public void AimAtMouse ()
    {
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Util.RotateTransformToward(transform, MouseWorldPos);
        //if aiming left = nothing
        if(MouseWorldPos.x > tr.position.x)
        {
            GunSprite.flipY = false;
        }
        //aiming right = flip the gun
        else
        {
            GunSprite.flipY = true;
        }
	}
}
