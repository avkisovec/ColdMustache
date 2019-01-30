using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColorOnStart : MonoBehaviour {

    public Color Base = new Color(1, 1, 1, 1);

    public float MaxDeviation = 0;
    
	void Start () {
        GetComponent<SpriteRenderer>().color = new Color(
            Random.Range(Base.r - MaxDeviation, Base.r + MaxDeviation),
            Random.Range(Base.r - MaxDeviation, Base.r + MaxDeviation),
            Random.Range(Base.r - MaxDeviation, Base.r + MaxDeviation),
            Base.a
            );
        Destroy(this);
	}
	
}
