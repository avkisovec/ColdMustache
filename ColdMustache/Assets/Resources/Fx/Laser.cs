using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public Vector3 Origin;

    public Vector3 End;

    public Sprite[] Sprites;

    public float LifeSpanInSeconds = 1;
    float MaxLifeSpan;
    
    int CurrFrameRounded = 0;
    
    SpriteRenderer sr;

	// Use this for initialization
	void Start () {

        MaxLifeSpan = LifeSpanInSeconds;

        transform.position = Origin + ((End - Origin) / 2);
        transform.localScale = new Vector3((End - Origin).magnitude * 32, 1, 1);

        Util.RotateTransformToward(transform, End);

        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = Sprites[CurrFrameRounded];

	}
	
	// Update is called once per frame
	void Update () {

        LifeSpanInSeconds -= Time.deltaTime;
        CurrFrameRounded = Sprites.Length - Mathf.RoundToInt(LifeSpanInSeconds / MaxLifeSpan * Sprites.Length);
        
        if(!(CurrFrameRounded < Sprites.Length))
        {
            Destroy(this.gameObject);
            return;
        }

        sr.sprite = Sprites[CurrFrameRounded];
    }
}
