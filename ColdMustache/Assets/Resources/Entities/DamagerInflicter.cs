using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerInflicter : MonoBehaviour {

    public Entity.team Team;
    public float Damage = 1;

    public bool SingleUse = true;

    public float CoolDownBetweenHits = 1;
    public float BecomeActiveAfterSeconds = 0;
    float PossibleActivation;

    public bool AttackWalls = true;

    public bool SpawnFxOnTargetInsteadOfSource = false;


    public bool IsProjectile = false;



    //public bool InflictEffects

    private void Start()
    {
        PossibleActivation = Time.time + BecomeActiveAfterSeconds;
        float f = Time.time + 0.5f + BecomeActiveAfterSeconds;
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
        if (Time.time >= PossibleActivation)
        {
            /*
            if (coll.CompareTag("Wall") || coll.CompareTag("PseudoWall"))
            {
                GetComponent<DeathAnimation>().Spawn(transform.position);
                Destroy(this.gameObject);
                return;
            }
            */
            Entity hit = coll.GetComponent<Entity>();
            if (hit != null)
            {
                if (Team != hit.Team)
                {
                    PossibleActivation = Time.time + CoolDownBetweenHits;
                    if (Damage != 0)
                    {
                        hit.TakeDamage(Damage);
                    }
                    if (SpawnFxOnTargetInsteadOfSource)
                    {
                        foreach (DeathAnimation da in GetComponents<DeathAnimation>())
                        {
                            GetComponent<DeathAnimation>().Spawn(coll.transform.position);
                        }
                    }
                    else
                    {
                        foreach (DeathAnimation da in GetComponents<DeathAnimation>())
                        {
                            GetComponent<DeathAnimation>().Spawn(transform.position);
                        }
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

            EnvironmentObject hit2 = coll.GetComponent<EnvironmentObject>();
            if (hit2 != null && AttackWalls)
            {
                if (!IsProjectile || hit2.InterceptProjectiles)
                {
                    PossibleActivation = Time.time + CoolDownBetweenHits;
                    if (Damage != 0)
                    {
                        hit2.TakeDamage(Damage);
                    }
                    if (SpawnFxOnTargetInsteadOfSource)
                    {
                        foreach(DeathAnimation da in GetComponents<DeathAnimation>())
                        {
                            GetComponent<DeathAnimation>().Spawn(coll.transform.position);
                        }
                    }
                    else
                    {
                        foreach (DeathAnimation da in GetComponents<DeathAnimation>())
                        {
                            GetComponent<DeathAnimation>().Spawn(transform.position);
                        }
                    }
                    if (SingleUse)
                    {
                        Destroy(this.gameObject);
                        return;
                    }
                }
            }

        }
    }

    public void ini(Entity.team Team, float Damage, bool SingleUse = true, bool SpawnFxOnTargetInsteadOfSource = false, float CoolDownBetweenHits = 1, float BecomeActiveAfterSeconds = 0, bool IsProjectile = true)
    {
        this.Team = Team;
        this.Damage = Damage;
        this.SingleUse = SingleUse;
        this.SpawnFxOnTargetInsteadOfSource = SpawnFxOnTargetInsteadOfSource;
        this.CoolDownBetweenHits = CoolDownBetweenHits;
        this.BecomeActiveAfterSeconds = BecomeActiveAfterSeconds;
        this.IsProjectile = IsProjectile;
    }
}
