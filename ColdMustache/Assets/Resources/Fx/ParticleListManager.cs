using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleListManager : MonoBehaviour
{

    public static List<Particle_List> list = new List<Particle_List>();

    void Start()
    {
        list.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(list.Count);
        for(int i = 0; i < list.Count; i++){
            //Debug.Log("i" + "/" + list.Count);
            list[i].CustomUpdate();
        }
        //Debug.Log("done updatin...");
    }
}
