using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxMeatExplosionPlusCorpse : DeathAnimation
{

    public Transform[] StuffToBePreservedAfterDeath;

    public override void Spawn(Vector3 Coordinates)
    {

        SpriteManagerHandLeg sm = (SpriteManagerHandLeg)GetComponent<Entity>().spriteManager;

        sm.EndTemporaryColor();
        sm.UpdateEverything();

        GameObject Corpse = new GameObject();

        Corpse.transform.position = transform.position;

        foreach(Transform t in StuffToBePreservedAfterDeath){
            t.parent = Corpse.transform;
        }



        Corpse.transform.localRotation = Quaternion.Euler(0,0,Random.Range(90, 270));


        for (int i = 0; i < 50; i++)
        {
            GameObject go = new GameObject();
            go.AddComponent<Particle>();
            go.transform.position = Coordinates;
            Particle p = go.GetComponent<Particle>();
            p.UseShifts = true;
            p.LeaveParent = true;
            p.StartAt00 = false;
            p.sprite = Resources.Load<Sprite>("pixel");
            p.Lifespan = (int)Mathf.Pow(Random.Range(4, 13), 2);
            p.StartingScale = 0.5f;
            p.EndingScale = 0f;
            p.StartingColor = new Color(1f, 0, 0f, 1);
            p.EndingColor = new Color(0.2f, 0, 0, 1);
            p.StartingHorizontalWind = Random.Range(-0.02f, 0.02f);
            p.StartingVerticalWind = Random.Range(-0.02f, 0.02f);
            p.EndingHorizontalWind = 0;
            p.EndingVerticalWind = 0;
        }
    }
}
