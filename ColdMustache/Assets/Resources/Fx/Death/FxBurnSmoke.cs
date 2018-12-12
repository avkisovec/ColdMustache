using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxBurnSmoke : DeathAnimation {

    public override void Spawn(Vector3 Coordinates)
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject go = new GameObject();
            go.AddComponent<Particle>();
            go.transform.position = Coordinates;
            Particle p = go.GetComponent<Particle>();
            p.UseShifts = true;
            p.LeaveParent = true;
            p.StartAt00 = false;
            p.sprite = Resources.Load<Sprite>("pixel");
            p.Lifespan = (int)Mathf.Pow(Random.Range(4, 13), 2);
            p.StartingScale = 0.3f;
            p.EndingScale = 0.8f;
            p.StartingColor = new Color(0.2f, 0.2f, 0.2f, 1);
            p.EndingColor = new Color(0.7f, 0.7f, 0.7f, 0);
            p.StartingHorizontalWind = Random.Range(-0.02f, 0.02f);
            p.StartingVerticalWind = Random.Range(-0.02f, 0.02f);
            p.EndingHorizontalWind = 0;
            p.EndingVerticalWind = 0;
        }
    }
}
