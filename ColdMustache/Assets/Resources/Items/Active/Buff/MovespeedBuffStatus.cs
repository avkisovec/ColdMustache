using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovespeedBuffStatus : MonoBehaviour
{

    public float Healing;

    public float DurationSeconds = 30;
    public float Coefficient = 1.5f;

    int Frame = 0;

    Entity e;

    private void Start()
    {
        e = GetComponent<Entity>();
    }

    // Update is called once per frame
    void Update()
    {

        if (e.MoveSpeedBuffModifier < Coefficient)
        {
            e.MoveSpeedBuffModifier = Coefficient;
        }

        if (Frame == 10)
        {
            Frame = 0;

            GameObject go = new GameObject();
            go.AddComponent<Particle>();
            go.transform.position = transform.position + new Vector3(0, 0, -0.5f);
            Particle p = go.GetComponent<Particle>();
            p.UseShifts = true;
            p.LeaveParent = true;
            p.StartAt00 = false;
            p.sprite = Resources.Load<Sprite>("Items/Active/Buff/ParticleArrow");
            p.Lifespan = 20;
            p.StartingScale = 0.4f;
            p.EndingScale = 1.0f;
            p.StartingColor = new Color(0, 1, 1, 1);
            p.EndingColor = new Color(0, 1, 1, 0);
            p.StartingHorizontalWind = Random.Range(-0.03f, 0.03f);
            p.StartingVerticalWind = Random.Range(-0.01f, 0.08f);
            p.EndingHorizontalWind = 0;
            p.EndingVerticalWind = 0;
        }
        else
        {
            Frame++;
        }

        if (DurationSeconds > 0)
        {
            DurationSeconds -= Time.deltaTime;
        }
        else
        {
            if (e.Health + Healing < e.MaxHealth)
            {
                e.Health += Healing;
            }
            else
            {
                e.Health = e.MaxHealth;
            }
            Destroy(this);
        }
    }

    public void ini(float DurationSeconds = 1, float BuffCoefficient = 1.5f)
    {
        this.DurationSeconds = DurationSeconds;
        this.Coefficient = BuffCoefficient;
    }
}
