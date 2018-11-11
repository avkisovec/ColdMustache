using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public SpriteRenderer spriteRenderer;

    public GameObject Target;
    SpriteManager spriteManager;

    float BaseCooldownToDefaultColor = 0.1f;
    float CurrCooldownToDefaultColor = 0;

    Color DefaultColor;
    Color InjuredColor = new Color(1,0,0);

	// Use this for initialization
	void Start () {
        
        DefaultColor = spriteRenderer.color;
        spriteManager = GetComponent<SpriteManager>();
	}
	
	// Update is called once per frame
	void Update () {

        spriteManager.LookAt(Target.transform.position);

        if (CurrCooldownToDefaultColor > 0)
        {
            CurrCooldownToDefaultColor -= Time.deltaTime;
        }
        else
        {
            spriteRenderer.color = DefaultColor;
        }

        //not yet implemented - if enemy has line of sight on player
        if (true)
        {

        }


    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "PlayerBullet")
        {
            spriteRenderer.color = InjuredColor;
            CurrCooldownToDefaultColor = BaseCooldownToDefaultColor;
        }
    }
}
