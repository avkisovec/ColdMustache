using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxBloodSlash : DeathAnimation {
    
    public override void Spawn(Vector3 Coordinates)
    {
        Vector2 BaseDirection = new Vector2(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f));
        BaseDirection /= BaseDirection.magnitude;

        float BaseHorWind = BaseDirection.x / 300;
        float BaseVerWind = BaseDirection.y / 300;

        //float BaseHorWind = Random.Range(-0.004f, 0.004f);
        //float BaseVerWind = Random.Range(-0.004f, 0.004f);
        

        for (int i = 0; i < 20; i++)
        {
            GameObject go = new GameObject();
            go.AddComponent<Particle>();
            go.transform.position = Coordinates + new Vector3(0,0,-0.5f);
            Particle p = go.GetComponent<Particle>();
            p.UseShifts = true;
            p.LeaveParent = true;
            p.StartAt00 = false;
            p.sprite = Resources.Load<Sprite>("pixel");
            p.Lifespan = Random.Range(20,40);
            p.StartingScale = 0.2f;
            p.EndingScale = 0f;
            p.StartingColor = new Color(1f, 0, 0f, 1);
            p.EndingColor = new Color(0.2f, 0, 0, 1);
            p.StartingHorizontalWind = BaseHorWind * i;
            p.StartingVerticalWind = BaseVerWind * i;
            p.EndingHorizontalWind = BaseHorWind + Random.Range(-0.02f, 0.02f);
            p.EndingVerticalWind = BaseVerWind + Random.Range(-0.02f, 0.02f);
        }
    }
}
