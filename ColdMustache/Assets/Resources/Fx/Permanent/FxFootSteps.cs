using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxFootSteps : MonoBehaviour {

    public float LifespanInSeconds = 3.5f;
    public float CooldownInSeconds = 1;
    public float CurrCooldown = 0;
    public Sprite sprite;
    public Color color;
    
	// Update is called once per frame
	void Update () {
        LifespanInSeconds -= Time.deltaTime;
        if(LifespanInSeconds < 0)
        {
            Destroy(this);
            return;
        }
        if(CurrCooldown > 0)
        {
            CurrCooldown -= Time.deltaTime;
        }
        else
        {
            CurrCooldown = CooldownInSeconds;

            GameObject footStep = new GameObject();
            footStep.transform.position = transform.position;
            footStep.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));

            footStep.AddComponent<SpriteRenderer>().sprite = sprite;
            footStep.GetComponent<SpriteRenderer>().color = color;

            footStep.AddComponent<DieInSeconds>().Seconds = 20;

        }
	}

    public void ini(float LifespanInSeconds, float CooldownInSeconds, Sprite sprite, Color color)
    {
        this.LifespanInSeconds = LifespanInSeconds;
        this.CooldownInSeconds = CooldownInSeconds;
        this.sprite = sprite;
        this.color = color;
    }
}
