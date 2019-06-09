using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChildrenRandomlyRecursively : MonoBehaviour
{

    /*
    
        this script chooses random color for each of its children,

        and then recursively goes through its children and colors them all
    
    
     */

    // Start is called before the first frame update

    public bool ENABLED = true;

    void Start()
    {

        if(!ENABLED) return;
        ENABLED = false;

        for (int i = 0; i < transform.childCount; i++)
        {

            ColorRecursively(transform.GetChild(i), new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f),1));

        }

    }

    void ColorRecursively(Transform tr, Color clr)
    {
        
        if(tr.name != "DamageOverlay"){

            SpriteRenderer sr = tr.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = clr;
            }
        }

        for (int i = 0; i < tr.childCount; i++)
        {

            ColorRecursively(tr.GetChild(i), clr);

        }

    }

}
