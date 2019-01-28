using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentrallyUpdatable : MonoBehaviour {

    /*
     * part of CentralUpdater
     * 
     * if you want to use central updater, inherit from this (:CentrallyUpdatable)
     * then override the CentralUpdate()
     * for all intents and purposes CentralUpdate works just like Update
     * 
     * in start, dont forget to call
     * CentralUpdater.Objects.Add(this);
     * 
     * 
     */

	public virtual void CentralUpdate()
    {

    }
}
