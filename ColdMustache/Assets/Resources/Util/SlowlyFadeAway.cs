using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowlyFadeAway : MonoBehaviour {

    /*
     * this script is attacked to gameobject with SpriteRenderer component
     * 
     * it makes the SR more and more transparent, eventually making it completely transparent and deleting the object (self)
     * 
     * 
     */

    public float Duration = 1;

    float OriginalDuration;
    float OriginalAlpha;

    SpriteRenderer sr;

	// Use this for initialization
	void Start () {

        sr = GetComponent<SpriteRenderer>();

        OriginalDuration = Duration;

        OriginalAlpha = sr.color.a;
        
	}
	
	// Update is called once per frame
	void Update () {

        Duration -= Time.deltaTime;

        if(Duration <= 0)
        {
            Destroy(this.gameObject);
            return;
        }

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, OriginalAlpha * Duration / OriginalDuration);

	}
}
