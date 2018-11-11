using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    
    Vector3 LastPos;
    Vector3 DeltaPos;
    
    // Use this for initialization
    void Start () {
        LastPos = transform.position;
	}

    //float[] DeltaPosRatios = new float[] { 0f, 0.33f, 0.66f };
    float[] DeltaPosRatios = new float[] { 0f, 0.25f, 0.5f, 0.75f };

    // Update is called once per frame
    void Update () {
        DeltaPos = LastPos - transform.position;
        LastPos = transform.position;
        
        foreach(float DeltaPosRatio in DeltaPosRatios)
        {
            GameObject go = new GameObject();
            go.AddComponent<Particle>();
            go.transform.position = transform.position + (DeltaPos * DeltaPosRatio);
            Particle p = go.GetComponent<Particle>();
            p.UseShifts = true;
            p.LeaveParent = true;
            p.StartAt00 = false;
            p.sprite = Resources.Load<Sprite>("pixel");
            p.Lifespan = Random.Range(3,9);
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
    

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "Enemy" || coll.tag == "WallSmooth" || coll.tag == "WallClimbable" || coll.tag == "Floor")
        {
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
                p.StartingColor = new Color(1f, 1f, 0.5f, 1);
                p.EndingColor = new Color(0.5f, 0, 0, 1);
                p.StartingHorizontalWind = Random.Range(-0.05f, 0.05f);
                p.StartingVerticalWind = Random.Range(-0.05f, 0.05f);
                p.EndingHorizontalWind = Random.Range(-0.01f, 0.01f);
                p.EndingVerticalWind = Random.Range(-0.01f, 0.01f);
            }
            Destroy(gameObject);
        }
    }

}
