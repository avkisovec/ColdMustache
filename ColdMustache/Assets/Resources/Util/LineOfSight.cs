using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{

    public Transform Source;

    public Transform Target;

    public bool IsLineClear = false;

    public int FramesSinceObstruction = 3;

    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
        gameObject.AddComponent<WiggleNonNoticably>();

        gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Source.position + ((Target.position - Source.position) / 2);
        transform.localScale = new Vector3((Target.position - Source.position).magnitude, 0.1f, 1);
        Util.RotateTransformToward(transform, Target);

        FramesSinceObstruction--;
        if (FramesSinceObstruction < 1)
        {
            IsLineClear = true;
        }
        else
        {
            IsLineClear = false;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            FramesSinceObstruction = 6;
        }
    }
}
