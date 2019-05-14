using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceAllChildrenWPrefab : MonoBehaviour
{
    public bool ENABLED = true;
    /*
     *  this script takes all children of current gameobject and replaces them with a given prefab
     *
     *  the prefab is loaded from string
     *
     */

    public string PrefabPath = "Undefined";

    public bool InheritSpriteRenderer = false;

    private void Awake()
    {
        if(!ENABLED) return;
        ENABLED = false;
        //return;
        int ChildCount = transform.childCount;
        for(int i = 0; i < ChildCount; i++){
            Transform child = transform.GetChild(i);
            GameObject nu = Instantiate(Resources.Load(PrefabPath) as GameObject);
            nu.transform.position = child.position;
            nu.transform.rotation = child.rotation;
            nu.transform.parent = transform;

            //inhering z index manager if applicable
            ZIndexManager zimc = child.GetComponent<ZIndexManager>();
            if(zimc!= null){
                ZIndexManager zimn = nu.GetComponent<ZIndexManager>();
                if(zimn!=null){
                    zimn.SingleUse = zimc.SingleUse;
                    zimn.Type = zimc.Type;
                    zimn.RelativeValue = zimc.RelativeValue;
                }
            }

            if(InheritSpriteRenderer){
                SpriteRenderer src = child.GetComponent<SpriteRenderer>();
                if (src != null)
                {
                    SpriteRenderer srn = nu.GetComponent<SpriteRenderer>();
                    if (srn != null)
                    {
                        srn.sprite = src.sprite;
                        srn.color = src.color;
                        srn.flipX = src.flipX;
                        srn.flipY = src.flipY;
                    }
                }
            }

            //destroying the original
            Destroy(child.gameObject);
        }
    }
}
