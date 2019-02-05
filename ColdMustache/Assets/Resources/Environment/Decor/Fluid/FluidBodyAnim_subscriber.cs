using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidBodyAnim_subscriber : MonoBehaviour
{
    /*
    
        voluntary part of FluidBodyAnim

        its easier in editor to link many objects to a single one than in reverse

        this script just does that they automatically add themselves to correct layer in parent FluidBodyAnim
    
     */

    public enum Layer {A,B};
    public Layer layer = Layer.A;
    public FluidBodyAnim Parent;
    void Start()
    {
        if(layer==Layer.A){
            Parent.spriteRenderersA.Add(GetComponent<SpriteRenderer>());
        }
        else{
            Parent.spriteRenderersB.Add(GetComponent<SpriteRenderer>());
        }
        Destroy(this);
    }
    
}
