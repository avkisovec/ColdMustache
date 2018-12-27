using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxBulletExplosionCustom : DeathAnimation {

    public Color ColorStart = new Color(1f, 1f, 0.5f, 1);
    public Color ColorEnd = new Color(0.5f, 0, 0, 1);

    public override void Spawn(Vector3 Coordinates)
    {
        //base.Spawn();
        for (int i = 0; i < 30; i++)
        {
            GameObject go = new GameObject();
            go.AddComponent<Particle>();
            go.transform.position = transform.position;
            Particle p = go.GetComponent<Particle>();
            p.UseShifts = true;
            p.LeaveParent = true;
            p.StartAt00 = false;
            p.sprite = Resources.Load<Sprite>("pixel");
            p.Lifespan = (int)Mathf.Pow(Random.Range(3, 10), 2);
            p.StartingScale = 0.1f;
            p.EndingScale = 0;
            p.StartingColor = ColorStart;
            p.EndingColor = ColorEnd;
            p.StartingHorizontalWind = Random.Range(-0.05f, 0.05f);
            p.StartingVerticalWind = Random.Range(-0.05f, 0.05f);
            p.EndingHorizontalWind = Random.Range(-0.01f, 0.01f);
            p.EndingVerticalWind = Random.Range(-0.01f, 0.01f);
        }
    }

    public void ini(Color ColorStart, Color ColorEnd)
    {
        this.ColorStart = ColorStart;
        this.ColorEnd = ColorEnd;
    }
}
