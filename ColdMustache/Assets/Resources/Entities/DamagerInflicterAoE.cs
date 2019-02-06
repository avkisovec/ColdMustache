using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerInflicterAoE : MonoBehaviour
{

    public Entity.team Team;
    public float Damage = 1;

    public float DelayBetweenHits = 1;
    float CurrDelayBetweenHits = 1;
    public float BecomeActiveAfterSeconds = 0;
    float PossibleActivation = -1;

    public float Lifespan = 5;
    float DeathOn = -1;

    public bool SpawnFxOnTargetInsteadOfSource = false;

    public List<int> AlreadyDamagedEntities = new List<int>();


    //for the purpose of death text, has nothing to do with damage types
    public DamagerInflicter.WeaponTypes WeaponType = DamagerInflicter.WeaponTypes.Undefined;

    List<DeathAnimation> deathAnimations = new List<DeathAnimation>();
    List<Inflicter> inflicters = new List<Inflicter>();

    
    private void Start()
    {
        PossibleActivation = Time.time + BecomeActiveAfterSeconds;
        DeathOn = PossibleActivation + Lifespan;
        foreach (DeathAnimation da in GetComponents<DeathAnimation>())
        {
            deathAnimations.Add(da);
        }
        foreach (Inflicter i in GetComponents<Inflicter>())
        {
            inflicters.Add(i);
        }
    }
    private void Update()
    {
        if(Time.time > DeathOn){
            Destroy(gameObject);
        }

        if(CurrDelayBetweenHits < 0){
            AlreadyDamagedEntities.Clear();
            CurrDelayBetweenHits = DelayBetweenHits;
        }
        CurrDelayBetweenHits -= Time.deltaTime;

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
            Entity hit = coll.GetComponent<Entity>();

            
            if (hit != null)
            {
                if (Team != hit.Team)
                {
                    foreach (int i in AlreadyDamagedEntities)
                    {
                        if (hit.UniqueId == i)
                        {
                            return;
                        }
                    }
                    AlreadyDamagedEntities.Add(hit.UniqueId);

                    PossibleActivation = Time.time + DelayBetweenHits;
                    if (Damage != 0)
                    {
                        hit.TakeDamage(Damage, WeaponType);
                    }
                    if (SpawnFxOnTargetInsteadOfSource)
                    {
                        foreach (DeathAnimation da in deathAnimations)
                        {
                            GetComponent<DeathAnimation>().Spawn(coll.transform.position);
                        }
                    }
                    else
                    {
                        foreach (DeathAnimation da in deathAnimations)
                        {
                            GetComponent<DeathAnimation>().Spawn(coll.transform.position);
                        }
                    }
                    foreach (Inflicter i in inflicters)
                    {
                        i.Inflict(coll.gameObject);
                    }
                    
                }
            }
        }
    }

    public void ini(Entity.team Team, float Damage, float Lifespan, bool SpawnFxOnTargetInsteadOfSource = false, float DelayBetweenHits = 1, float BecomeActiveAfterSeconds = 0, DamagerInflicter.WeaponTypes WeaponType = DamagerInflicter.WeaponTypes.Undefined)
    {
        this.Team = Team;
        this.Damage = Damage;
        this.Lifespan = Lifespan;
        this.SpawnFxOnTargetInsteadOfSource = SpawnFxOnTargetInsteadOfSource;
        this.DelayBetweenHits = DelayBetweenHits;
        this.BecomeActiveAfterSeconds = BecomeActiveAfterSeconds;
        this.WeaponType = WeaponType;
    }
}
