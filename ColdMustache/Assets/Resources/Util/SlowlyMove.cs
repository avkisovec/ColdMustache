using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowlyMove : MonoBehaviour {

    public Vector3 PositionChange = new Vector3(0, 0);

    private void FixedUpdate()
    {
        transform.position += PositionChange;
    }

}
