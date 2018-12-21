using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    public Entity entity;
    public string HealthSpritesheetPath;
    public string HealthOverlayPath;

    Sprite[] Health;
    Sprite[] HealthOverlay;

    public SpriteRenderer HealthSR;
    public SpriteRenderer HealthOverlaySr;

	// Use this for initialization
	void Start () {

        Health = Resources.LoadAll<Sprite>(HealthSpritesheetPath);
        HealthOverlay = Resources.LoadAll<Sprite>(HealthOverlayPath);

	}
	
	// Update is called once per frame
	void Update () {

        if(entity.Health <= 0)
        {
            Destroy(this);
        }

        HealthSR.sprite = Health[Mathf.RoundToInt(Health.Length- 1 - (entity.Health / entity.MaxHealth * Health.Length-1))];

        if(entity.MaxHealth < 11)
        {
            HealthOverlaySr.sprite = HealthOverlay[0];
            return;
        }
        if (entity.MaxHealth < 21)
        {
            HealthOverlaySr.sprite = HealthOverlay[1];
            return;
        }
        if (entity.MaxHealth < 31)
        {
            HealthOverlaySr.sprite = HealthOverlay[2];
            return;
        }
        if (entity.MaxHealth < 41)
        {
            HealthOverlaySr.sprite = HealthOverlay[3];
            return;
        }
        if (entity.MaxHealth < 51)
        {
            HealthOverlaySr.sprite = HealthOverlay[4];
            return;
        }
        if (entity.MaxHealth < 61)
        {
            HealthOverlaySr.sprite = HealthOverlay[5];
            return;
        }
        if (entity.MaxHealth < 71)
        {
            HealthOverlaySr.sprite = HealthOverlay[6];
            return;
        }
        HealthOverlaySr.sprite = HealthOverlay[7];
        return;
    }
}
