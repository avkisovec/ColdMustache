using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipShader : MonoBehaviour {

    public Ship.Sides Side = Ship.Sides.Bow;

    public Transform LightSource;
    SpriteRenderer sr;

    public Ship ParentShuttle;

    public float Ratio = 150;
    public float Bias = 0;

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
            case Ship.Sides.Bow:
                MyVector = ParentShuttle.RelativeBow;
                break;
            case Ship.Sides.Stern:
                MyVector = ParentShuttle.RelativeStern;
                break;
            case Ship.Sides.Port:
                MyVector = ParentShuttle.RelativePort;
                break;
            case Ship.Sides.Starboard:
                MyVector = ParentShuttle.RelativeStarboard;
                break;

        }
        
        float Angle = Vector2.Angle(
            Util.GetDirectionVectorToward(transform, LightSource),
            MyVector
            );

        float Alpha = Util.NormalizeAngle(Angle) / Ratio + Bias;


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
