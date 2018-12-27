using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxBulletTrailCustom : MonoBehaviour {

    public Color ColorStart = new Color(1f, 1f, 0.5f, 1);
    public Color ColorEnd = new Color(0.5f, 0, 0, 1);

    Vector3 LastPos;
    Vector3 DeltaPos;
    

    // Use this for initialization
    void Start()
    {
        LastPos = transform.position;
    }

    //float[] DeltaPosRatios = new float[] { 0f, 0.33f, 0.66f };
    float[] DeltaPosRatios = new float[] { 0f, 0.25f, 0.5f, 0.75f };

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
            p.Lifespan = Random.Range(3, 9);
            p.StartingScale = 0.1f;
            p.EndingScale = 0;
            p.StartingColor = ColorStart;
            p.EndingColor = ColorEnd;
            p.StartingHorizontalWind = Random.Range(0, 0);
            p.StartingVerticalWind = Random.Range(0, 0);
            p.EndingHorizontalWind = Random.Range(-0.08f, 0.08f);
            p.EndingVerticalWind = Random.Range(-0.08f, 0.08f);
        }
    }

    public void ini(Color ColorStart, Color ColorEnd)
    {
        this.ColorStart = ColorStart;
        this.ColorEnd = ColorEnd;
    }
}
