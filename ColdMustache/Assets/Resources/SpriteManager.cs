using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour {

    //public enum LookDirections { Up, Down, Left, Right }

    public Sprite Front;
    public Sprite Side;
    public Sprite Back;

    public SpriteRenderer SpriteRenderer;
    
	// Use this for initialization
	void Start ()
    {
        SpriteRenderer.sprite = Front;
    }
	
	public void LookAt(Vector3 Target)
    {
        Vector2 Delta = (Vector2)Target - (Vector2)transform.position;

        //if target is farther "up/down" than "left/right", choose between up/down
        if (Mathf.Abs(Delta.y) > Mathf.Abs(Delta.x))
        {
            SpriteRenderer.flipX = false;
            //up (back is visible)
            if (Delta.y > 0)
            {
                SpriteRenderer.sprite = Back;
                
            }
            //down (front is visible)
            else
            {
                SpriteRenderer.sprite = Front;

            }
        }
        //target is on one of your sides - choose between left/right
        else
        {
            //side (toward right = dafault)
            if (Delta.x > 0)
            {
                SpriteRenderer.sprite = Side;
                SpriteRenderer.flipX = false;
                
                
            }
            //other side (toward left - needs mirrorring)
            else
            {
                SpriteRenderer.sprite = Side;
                SpriteRenderer.flipX = true;
                
            }
        }
    }


}
