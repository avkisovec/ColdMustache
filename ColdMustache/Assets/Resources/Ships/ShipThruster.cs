using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipThruster : MonoBehaviour {

    public Ship ParentShip;

    public Ship.Sides ShipSide = Ship.Sides.Stern;

    //public Vector2 FumeVector = new Vector2();

	// Use this for initialization
	void Start () {
        //FumeVector /= FumeVector.magnitude;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector2 GetVector()
    {
        switch (ShipSide)
        {
            case Ship.Sides.Stern:
                return ParentShip.RelativeStern;
            case Ship.Sides.Port:
                return ParentShip.RelativePort;
            case Ship.Sides.Starboard:
                return ParentShip.RelativeStarboard;
        }
        return new Vector2(0, 0);
    }
}
