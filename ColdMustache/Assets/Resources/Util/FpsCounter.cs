using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour {

    int Frames;
    int LastSecond;
	// Use this for initialization
	void Start () {
        LastSecond = Mathf.FloorToInt(Time.time);
	}
	
	// Update is called once per frame
	void Update () {
        if (LastSecond == Mathf.FloorToInt(Time.time))
        {
            Frames++;
        }
        else
        {
            LastSecond = Mathf.FloorToInt(Time.time);
            GetComponent<Text>().text = Frames.ToString();
            Frames = 1;
        }
	}
}
