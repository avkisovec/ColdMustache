using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTestNavigation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.M))
        {/*
            GetComponent<WaypointNavigator>().WayPoints = GameObject.Find("ScriptHolder").GetComponent<NavTest>().FindAPath(
                Mathf.RoundToInt(transform.position.x),
                Mathf.RoundToInt(transform.position.y),
                Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x),
                Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y)
                );*/
            GetComponent<WaypointNavigator>().WayPoints = NavTestStatic.FindAPath(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y),
            Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x),
            Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y)
            );
        }
    }
}
