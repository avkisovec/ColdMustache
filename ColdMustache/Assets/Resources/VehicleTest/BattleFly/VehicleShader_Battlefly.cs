using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleShader_Battlefly : MonoBehaviour {

    public Shuttle.Sides Side = Shuttle.Sides.Bow;

    public Transform LightSource;
    SpriteRenderer sr;

    public Shuttle_Battlefly ParentShuttle;

    // Use this for initialization
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();



    }

    // Update is called once per frame
    void Update()
    {

        Vector2 MyVector = new Vector2(0, 0);

        switch (Side)
        {
            case Shuttle.Sides.Bow:
                MyVector = ParentShuttle.RelativeBow;
                break;
            case Shuttle.Sides.Stern:
                MyVector = ParentShuttle.RelativeStern;
                break;
            case Shuttle.Sides.Port:
                MyVector = ParentShuttle.RelativePort;
                break;
            case Shuttle.Sides.Starboard:
                MyVector = ParentShuttle.RelativeStarboard;
                break;

        }

        //float MyAngle = AngleRelativeToNose + Mathf.Sin(transform.rotation.eulerAngles.z);


        /*
        float Angle = Util.GetAngleBetweenVectors(
            Util.GetDirectionVectorToward(transform, LightSource),
            MyVector
            );
*/
        float Angle = Vector2.Angle(
            Util.GetDirectionVectorToward(transform, LightSource),
            MyVector
            );

        float Alpha = Util.NormalizeAngle(Angle) / 180;
        //now, alpha will always be between 0 and 1

        //this makes the light harsher, sharper (changing to something like *1,5 - 0,5 will make the light softer)
        Alpha = Alpha * 2 - 1;

        if (Alpha < 0)
        {
            Alpha = 0;
        }
        if (Alpha > 1)
        {
            Alpha = 1;
        }


        sr.color = new Color(1, 1, 1, Alpha);

    }
}
