using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusSlow : MonoBehaviour {


    public float DurationSeconds = 2;
    public float Coefficient = 0.5f;

    public bool SpawnParticles = true;
    
    Entity e;

    private void Start()
    {
        e = GetComponent<Entity>();
    }

    // Update is called once per frame
    void Update () {

        if(e.MoveSpeedSlowModifier > Coefficient)
        {
            e.MoveSpeedSlowModifier = Coefficient;
        }

        if (SpawnParticles)
        {
            GameObject go = new GameObject();
            go.AddComponent<Particle>();
            go.transform.position = transform.position + new Vector3(0, -0.3f, -0.5f);
            Particle p = go.GetComponent<Particle>();
            p.UseShifts = true;
            p.LeaveParent = true;
            p.StartAt00 = false;
            p.sprite = Resources.Load<Sprite>("pixel");
            p.Lifespan = 20;
            p.StartingScale = 0.2f;
            p.EndingScale = 0.2f;
            p.StartingColor = new Color(0.5f, 0.5f, 1, 1);
            p.EndingColor = new Color(1, 1, 1, 0);
            p.StartingHorizontalWind = Random.Range(-0.03f, 0.03f);
            p.StartingVerticalWind = Random.Range(-0.01f, 0.01f);
            p.EndingHorizontalWind = 0;
            p.EndingVerticalWind = 0;
        }

        if(DurationSeconds > 0)
        {
            DurationSeconds -= Time.deltaTime;
        }
        else
        {
            Destroy(this);
        }
    }

    public void ini(float DurationSeconds = 1, float Coefficient = 0.5f, bool SpawnParticles = true)
    {
        this.DurationSeconds = DurationSeconds;
        this.Coefficient = Coefficient;
        this.SpawnParticles = SpawnParticles;
    }
}
