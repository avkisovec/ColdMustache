using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmineItem : ActiveItem
{

    public Grenade.GrenadeTypes GrenadeType = Grenade.GrenadeTypes.Frag;

    public int Charges = 1;
    public Sprite[] ChargesSprites;
    public Sprite EmptySprite;
    public SpriteRenderer sr;

    public string FloatingTextWhenUsingWithoutCharges = "I already used this";

    public override void CodeAfterEquipping()
    {
        UniversalReference.PlayerScript.CurrentlyEquippedItem = this;
        UniversalReference.SelectedItemIcon.sprite = sr.sprite;
    }

    public override void DoYourThing()
    {
        if (Charges > 0)
        {
            GameObject go = new GameObject();
            if (GrenadeType == Grenade.GrenadeTypes.Frag) go.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Items/Active/Throwables/Landmine_projectile");
            //else if (GrenadeType == Grenade.GrenadeTypes.Fire) go.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Items/Active/Throwables/GrenadeIncendiary_projectile");
            go.AddComponent<ZIndexManager>().Type = ZIndexManager.Types.Objects;
            go.GetComponent<ZIndexManager>().RelativeValue = 1.5f;
            go.transform.position = UniversalReference.PlayerObject.transform.position;
            Rigidbody2D rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.mass = 0.1f;
            rb.velocity = Util.GetDirectionVectorToward(go.transform, UniversalReference.MouseWorldPos) * 15;
            rb.angularVelocity = Random.Range(-100f, 100f);
            rb.angularDrag = 100;
            rb.drag = 15;
            Landmine expl = go.AddComponent<Landmine>();
            expl.GrenadeType = GrenadeType;
            expl.ExplosionRadius = 5;
            expl.rb = rb;
            expl.LastPos = go.transform.position;

            CircleCollider2D collider = go.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = 1;

            Charges--;
            if (Charges > 0)
            {
                sr.sprite = ChargesSprites[ChargesSprites.Length - 1];
            }
            else
            {
                sr.sprite = EmptySprite;
            }
        }
        else
        {
            AlphabetManager.SpawnFloatingText(FloatingTextWhenUsingWithoutCharges, new Vector3(UniversalReference.PlayerObject.transform.position.x, UniversalReference.PlayerObject.transform.position.y, -45));
        }
        UniversalReference.SelectedItemIcon.sprite = sr.sprite;
    }

    public override void CodeBeforeRemoving()
    {
        UniversalReference.PlayerScript.CurrentlyEquippedItem = null;
    }

    public override void Refill()
    {
        Charges = 1;
    }
}
