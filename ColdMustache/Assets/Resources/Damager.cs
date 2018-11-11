﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour {

    public Entity.team Team;

    public int Damage = 1;

	// Use this for initialization
	void Start () {
		
        

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D coll)
    {
        Entity hit = coll.GetComponent<Entity>();
        if (hit!=null)
        {
            if(Team != hit.Team)
            {
                hit.TakeDamage(1);
                GetComponent<DeathAnimation>().Spawn();
                Destroy(gameObject);
                /*
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
                }*/
            }

            
        }
    }

}
