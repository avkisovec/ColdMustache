using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSegmentTile : EnvironmentObject
{

    public int IndexInSegment = -1;
    public WallSegment Parent = null;

    void Start()
    {

        MaxHealth = Health;
        posX = Mathf.RoundToInt(transform.position.x);
        posY = Mathf.RoundToInt(transform.position.y);


        if (DoDamageOverlay)
        {
            GameObject overlay = new GameObject();
            overlay.transform.parent = transform;
            overlay.transform.localPosition = new Vector3(0, 0, -1);
            DamageOverlay = overlay.AddComponent<SpriteRenderer>();
            DamageSprites = Resources.LoadAll<Sprite>(DamageOverlayPath);
        }

    }

    public override void TakeDamage(float Damage)
    {
        if (!Indestructible)
        {
            Parent.ReportTileTakingDamage(IndexInSegment, Damage);
        }
    }
    public override void Die()
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
        if (!Nav_LightCanPass)
        {
            NavTestStatic.LightNavArray[posX, posY] = NavTestStatic.EmptyTileValue;
        }


        if (DeathRemains != null)
        {
            GameObject remains = new GameObject();
            remains.transform.position = new Vector3(transform.position.x, transform.position.y, ZIndexManager.Const_Floors - 1);
            remains.AddComponent<SpriteRenderer>().sprite = DeathRemains;
            remains.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        }
        Health = 999999999; //this is to make sure object doesnt die twice - if multiple hits land and once, while target if below 0 hp, each new hit is hit bbringing him below 0hp therefore spawning new corpse etc
        Destroy(this.gameObject);

        return;
    }
}