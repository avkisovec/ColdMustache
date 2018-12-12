using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepsSource : MonoBehaviour {

    public float LifespanInSeconds = 3.5f;
    public float CooldownInSeconds = 0.1f;
    public Sprite sprite;
    public Color color;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    private void OnTriggerExit2D(Collider2D coll)
    {
        Entity hit = coll.GetComponent<Entity>();
        if (hit != null)
        {
            coll.gameObject.AddComponent<FxFootSteps>().ini(LifespanInSeconds, CooldownInSeconds, sprite, color);
        }
    }

}
