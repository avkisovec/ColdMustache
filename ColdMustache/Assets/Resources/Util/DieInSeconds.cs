using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieInSeconds : MonoBehaviour {

    public float Seconds = 0;
    
    // Update is called once per frame
    void Update()
    {
        if (Seconds > 0)
        {
            Seconds-=Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
