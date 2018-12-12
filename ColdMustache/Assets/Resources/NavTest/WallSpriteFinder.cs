using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpriteFinder : MonoBehaviour {


    public static Sprite Find(Sprite[] sprites, bool ConnectedRight, bool ConnectedUp, bool ConnectedLeft, bool ConnectedDown)
    {
        int Connections = 0;
        if (ConnectedRight)
        {
            Connections++;
        }
        if (ConnectedUp)
        {
            Connections++;
        }
        if (ConnectedLeft)
        {
            Connections++;
        }
        if (ConnectedDown)
        {
            Connections++;
        }
        //cross
        if (Connections == 4)
        {
            return sprites[0];
        }
        if(Connections == 0)
        {
            return sprites[1];
        }
        if(Connections == 3)
        {
            if (!ConnectedRight)
            {
                return sprites[4];
            }
            if (!ConnectedUp)
            {
                return sprites[5];
            }
            if (!ConnectedLeft)
            {
                return sprites[6];
            }
            if (!ConnectedDown)
            {
                return sprites[7];
            }
        }
        if(Connections == 1)
        {
            if (ConnectedRight)
            {
                return sprites[12];
            }
            if (ConnectedUp)
            {
                return sprites[13];
            }
            if (ConnectedLeft)
            {
                return sprites[14];
            }
            if (ConnectedDown)
            {
                return sprites[15];
            }
        }
        if (ConnectedRight && ConnectedLeft)
        {
            return sprites[2];
        }
        if (ConnectedDown && ConnectedUp)
        {
            return sprites[3];
        }
        if (ConnectedRight && ConnectedUp)
        {
            return sprites[8];
        }
        if (ConnectedUp && ConnectedLeft)
        {
            return sprites[9];
        }
        if (ConnectedLeft && ConnectedDown)
        {
            return sprites[10];
        }
        if (ConnectedDown && ConnectedRight)
        {
            return sprites[11];
        }

        return null;
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
