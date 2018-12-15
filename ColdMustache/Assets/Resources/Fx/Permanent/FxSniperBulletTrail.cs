using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxSniperBulletTrail : MonoBehaviour {

    Vector3 LastPos;
    Vector3 DeltaPos;

    // Use this for initialization
    void Start()
    {
        LastPos = transform.position;
    }

    float[] DeltaPosRatios = new float[] { 0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f };
    //float[] DeltaPosRatios = new float[] { 0f, 0.05f, 0.1f, 0.15f, 0.20f, 0.25f, 0.30f, 0.35f, 0.40f, 0.45f, 50f, 0.55f, 0.60f, 0.65f, 0.70f, 0.75f, 0.80f, 0.85f, 0.90f, 0.95f };

    // Update is called once per frame
    void Update()
    {
        DeltaPos = LastPos - transform.position;
        LastPos = transform.position;

        foreach (float DeltaPosRatio in DeltaPosRatios)
        {
            GameObject go = new GameObject();
            go.AddComponent<Particle>();
            go.transform.position = transform.position + (DeltaPos * DeltaPosRatio);
            Particle p = go.GetComponent<Particle>();
            p.UseShifts = true;
            p.LeaveParent = true;
            p.StartAt00 = false;
            p.sprite = Resources.Load<Sprite>("pixel");
            p.Lifespan = Random.Range(5, 12);
            p.StartingScale = 0.1f;
            p.EndingScale = 0;
            p.StartingColor = new Color(1f, 1f, 0.5f, 1);
            p.EndingColor = new Color(0.5f, 0, 0, 1);
            p.StartingHorizontalWind = Random.Range(0, 0);
            p.StartingVerticalWind = Random.Range(0, 0);
            p.EndingHorizontalWind = Random.Range(-0.08f, 0.08f);
            p.EndingVerticalWind = Random.Range(-0.08f, 0.08f);
        }
    }
}
