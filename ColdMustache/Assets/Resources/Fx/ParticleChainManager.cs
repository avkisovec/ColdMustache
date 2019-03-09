using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleChainManager : MonoBehaviour
{



    public static Particle_Chain FirstOne;

    public static Particle_Chain LastOne;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {;
        if(FirstOne!=null || true){
            
            Particle_Chain Pointer = FirstOne;

            while(Pointer != null){

                Pointer.CustomUpdate();
                Pointer = Pointer.Child;

            }

        }
    }
}
