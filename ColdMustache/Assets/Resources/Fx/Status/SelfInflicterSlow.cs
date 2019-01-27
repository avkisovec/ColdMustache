using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfInflicterSlow : Inflicter {

    public GameObject entityObject;

    public float DurationSeconds = 2;
    public float Coefficient = 0.5f;
    public bool SpawnParticles = true;

    public void ini(float DurationSeconds = 1, float Coefficient = 0.5f, bool SpawnParticles = true)
    {
        this.DurationSeconds = DurationSeconds;
        this.Coefficient = Coefficient;
        this.SpawnParticles = SpawnParticles;
    }

    public override void Inflict(GameObject Target = null)
    {
        entityObject.AddComponent<StatusSlow>().ini(DurationSeconds, Coefficient, SpawnParticles);
    }
}
