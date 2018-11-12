using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour {

    public Entity.team Team;
    public int Damage = 1;

    public bool SpawnEffectOnEnemyInsteadOfProjectile = false;
    
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "Wall")
        {
            if (SpawnEffectOnEnemyInsteadOfProjectile)
            {
                GetComponent<DeathAnimation>().Spawn(coll.transform.position);
            }
            else
            {
                GetComponent<DeathAnimation>().Spawn(transform.position);
            }
            Destroy(gameObject);
            return;
        }
        Entity hit = coll.GetComponent<Entity>();
        if (hit!=null)
        {
            if(Team != hit.Team)
            {
                hit.TakeDamage(1);
                if (SpawnEffectOnEnemyInsteadOfProjectile)
                {
                    GetComponent<DeathAnimation>().Spawn(coll.transform.position);
                }
                else
                {
                    GetComponent<DeathAnimation>().Spawn(transform.position);
                }
                Destroy(gameObject);
            }
        }
    }

}
