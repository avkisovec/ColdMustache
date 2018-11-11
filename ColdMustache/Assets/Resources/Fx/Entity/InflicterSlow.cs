using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InflicterSlow : MonoBehaviour {

    public Entity.team Team;

    public float DurationSeconds = 2;
    public float Coefficient = 0.5f;
    public bool SpawnParticles = true;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        Entity hit = coll.GetComponent<Entity>();
        if (hit != null)
        {
            if (Team != hit.Team)
            {
                hit.gameObject.AddComponent<StatusSlow>().ini();
                Destroy(gameObject);
            }
        }
    }

    public void ini(Entity.team Team, float DurationSeconds = 1, float Coefficient = 0.5f, bool SpawnParticles = true)
    {
        this.Team = Team;
        this.DurationSeconds = DurationSeconds;
        this.Coefficient = Coefficient;
        this.SpawnParticles = SpawnParticles;
    }
}
