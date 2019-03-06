using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceAllChildrenWPrefab : MonoBehaviour
{
    public bool Active = true;
    /*
     *  this script takes all children of current gameobject and replaces them with a given prefab
     *
     *  the prefab is loaded from string
     *
     */

    public string PrefabPath;

    private void Awake()
    {
        if(!Active) return;
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

            //destroying the original
            Destroy(child.gameObject);
        }
    }
}
