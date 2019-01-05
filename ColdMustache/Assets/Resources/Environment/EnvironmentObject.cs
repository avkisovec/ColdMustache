using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObject : MonoBehaviour {

    public float Health = 10;
    public float MaxHealth;

    public bool Indestructible = false;
    public bool InterceptProjectiles = true;

    public bool Nav_Walkable = false;
    public bool Nav_ExplosionCanPass = false;
    public bool Nav_LightCanPass = false;

    //automatically generated from transform.position, rounded
    public int posX = 0;
    public int posY = 0;

    public bool LinkToNerbyImpassables = true;
    public string SpritePath = "";

    public string DamageOverlayPath = "";
    public Sprite[] DamageSprites;

    SpriteRenderer DamageOverlay;

    public bool DoDamageOverlay = true;
    public Sprite DeathRemains;

	// Use this for initialization
	void Start () {

        MaxHealth = Health;
        posX = Mathf.RoundToInt(transform.position.x);
        posY = Mathf.RoundToInt(transform.position.y);
        
        //takes in consideration only stuff impassable for explosions
        if (LinkToNerbyImpassables)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = WallSpriteFinder.Find(
                Resources.LoadAll<Sprite>(SpritePath),
                posX<NavTestStatic.MapWidth && NavTestStatic.ExplosionNavArray[posX + 1, posY] == NavTestStatic.ImpassableTileValue,
                posY < NavTestStatic.MapHeight && NavTestStatic.ExplosionNavArray[posX, posY + 1] == NavTestStatic.ImpassableTileValue,
                posX > 0 && NavTestStatic.ExplosionNavArray[posX - 1, posY] == NavTestStatic.ImpassableTileValue,
                posY > 0 && NavTestStatic.ExplosionNavArray[posX, posY - 1] == NavTestStatic.ImpassableTileValue
            );
        }
        
        if (DoDamageOverlay)
        {
            GameObject overlay = new GameObject();
            overlay.transform.parent = transform;
            overlay.transform.localPosition = new Vector3(0, 0, -1);
            DamageOverlay = overlay.AddComponent<SpriteRenderer>();
            DamageSprites = Resources.LoadAll<Sprite>(DamageOverlayPath);
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(float Damage)
    {
        if (!Indestructible)
        {
            Health -= Damage;
            if (Health <= 0)
            {
                Die();
            }

            if (DoDamageOverlay)
            {
                if (Health < 0.25 * MaxHealth)
                {
                    DamageOverlay.sprite = DamageSprites[2];
                }
                else if (Health < 0.50 * MaxHealth)
                {
                    DamageOverlay.sprite = DamageSprites[1];
                }
                else if (Health < 0.75 * MaxHealth)
                {
                    DamageOverlay.sprite = DamageSprites[0];
                }
            }

        }
    }
    public void Die()
    {
        //GetComponent<DeathAnimation>().Spawn(transform.position);
        if (!Nav_Walkable)
        {
            NavTestStatic.NavArray[posX, posY] = NavTestStatic.EmptyTileValue;
        }
        if (!Nav_ExplosionCanPass)
        {
            NavTestStatic.ExplosionNavArray[posX, posY] = NavTestStatic.EmptyTileValue;
        }


        if (DeathRemains != null)
        {
            GameObject remains = new GameObject();
            remains.transform.position = new Vector3(transform.position.x, transform.position.y, ZIndexManager.Const_Floors-1);
            remains.AddComponent<SpriteRenderer>().sprite = DeathRemains;
            remains.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        }
        Health = 999999999; //this is to make sure object doesnt die twice - if multiple hits land and once, while target if below 0 hp, each new hit is hit bbringing him below 0hp therefore spawning new corpse etc
        Destroy(this.gameObject);

        return;
    }
}
