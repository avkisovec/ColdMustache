using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerInflicter : MonoBehaviour {

    public Entity.team Team;
    public int Damage = 1;

    public bool SingleUse = true;
    public float CoolDownBetweenHits = 1;
    float CurrCooldown = 0;

    public bool SpawnFxOnTargetInsteadOfSource = false;

    //public bool InflictEffects

    private void Update()
    {
        if (CurrCooldown > 0)
        {
            CurrCooldown -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        Collision(coll);
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        Collision(coll);
    }

    private void Collision(Collider2D coll)
    {
        if (coll.CompareTag("Wall") || coll.CompareTag("PseudoWall")){
            GetComponent<DeathAnimation>().Spawn(transform.position);
            Destroy(this.gameObject);
            return;
        }
        Entity hit = coll.GetComponent<Entity>();
        if (hit != null && CurrCooldown <= 0)
        {
            if (Team != hit.Team)
            {
                CurrCooldown = CoolDownBetweenHits;
                if (Damage!= 0)
                {
                    hit.TakeDamage(Damage);
                }
                if (SpawnFxOnTargetInsteadOfSource)
                {
                    GetComponent<DeathAnimation>().Spawn(coll.transform.position);
                }
                else
                {
                    GetComponent<DeathAnimation>().Spawn(transform.position);
                }
                foreach (Inflicter i in GetComponents<Inflicter>())
                {
                    i.Inflict(coll.gameObject);
                }
                if (SingleUse)
                {
                    Destroy(this.gameObject);
                    return;
                }
            }
        }
    }

    public void ini(Entity.team Team, int Damage, bool SingleUse = true, float CoolDownBetweenHits = 1)
    {
        this.Team = Team;
        this.Damage = Damage;
        this.SingleUse = SingleUse;
        this.CoolDownBetweenHits = CoolDownBetweenHits;
    }
}
