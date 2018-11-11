using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour {

    public Entity.team Team;
    public int Damage = 1;
    
    private void OnTriggerEnter2D(Collider2D coll)
    {
        Entity hit = coll.GetComponent<Entity>();
        if (hit!=null)
        {
            if(Team != hit.Team)
            {
                hit.TakeDamage(1);
                GetComponent<DeathAnimation>().Spawn();
                Destroy(gameObject);
            }
        }
    }

}
