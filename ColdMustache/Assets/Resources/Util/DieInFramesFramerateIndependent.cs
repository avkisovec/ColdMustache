using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieInFramesFramerateIndependent : MonoBehaviour {

    public int Frames = 0;
    
    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < FramerateManager.FramesRequiredToRenderToKeepUp; i++)
        {
            if (Frames > 0)
            {
                Frames--;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
