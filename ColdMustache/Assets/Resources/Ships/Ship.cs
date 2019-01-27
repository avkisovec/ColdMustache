using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    public enum Sides { Bow, Stern, Port, Starboard }
    
    public float TurningSpeed = 300f;
    public float ForwardSpeed = 600f;
    public float StrafingSpeed = 120f;

    public float Throttle = 1;

    public Rigidbody2D rb;

    //when the ship is facing right of the screen
    public Vector2 DefaultBow = new Vector2(1, 0);
    public Vector2 DefaultStern = new Vector2(-1, 0);
    public Vector2 DefaultPort = new Vector2(0, 1);
    public Vector2 DefaultStarboard = new Vector2(0, -1);

    public Vector2 RelativeBow = new Vector2(1, 0);
    public Vector2 RelativeStern = new Vector2(-1, 0);
    public Vector2 RelativePort = new Vector2(0, 1);
    public Vector2 RelativeStarboard = new Vector2(0, -1);
    
    public Transform VectorIndicator;

    public ShipThruster[] ThrustersWhenMoveForward;
    public ShipThruster[] ThrustersWhenStrafeTowardPort;
    public ShipThruster[] ThrustersWhenStrafeTowardStarboard;
    public ShipThruster[] ThrustersWhenTurnClockwise;
    public ShipThruster[] ThrustersWhenTurnCounterClockwise;

    public Color FumeColorStart = new Color(0.5f,0.5f,1,1);
    public Color FumeColorEnd = new Color(0,0,1,0); 

    #region start

    // Use this for initialization
    void Start()
    {
        BaseStart();
    }

    public void BaseStart()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    #endregion

    #region update

    public void BaseUpdate()
    {
        CheckThrottleRange();
        UpdateRelativeDirections();
        UpdateVectorIndicator();


        if (rb.velocity.magnitude > ForwardSpeed / 10)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, ForwardSpeed / 10);
        }

    }

    public void UpdateRelativeDirections()
    {
        RelativeBow = Util.RotateVector(DefaultBow, transform.rotation.eulerAngles.z);
        RelativeStern = Util.RotateVector(DefaultStern, transform.rotation.eulerAngles.z);
        RelativePort = Util.RotateVector(DefaultPort, transform.rotation.eulerAngles.z);
        RelativeStarboard = Util.RotateVector(DefaultStarboard, transform.rotation.eulerAngles.z);
    }

    public void UpdateVectorIndicator()
    {
        if(VectorIndicator != null)
        {
            VectorIndicator.position = transform.position + (Vector3)rb.velocity;
        }
    }

    public void CheckThrottleRange()
    {
        if (Throttle > 1)
        {
            Throttle = 1;
        }
        if (Throttle < 0)
        {
            Throttle = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        BaseUpdate();

    }

    #endregion

    #region movement

    public virtual void MoveForward()
    {
        rb.AddForce(RelativeBow * ForwardSpeed * Throttle * Time.deltaTime);
        Fume_MovingForward();
    }

    public virtual void StrafePortside()
    {
        rb.AddForce(RelativePort * StrafingSpeed * Throttle * Time.deltaTime);
        Fume_StrafeTowardPort();
    }

    public virtual void StrafeStarboardSide()
    {
        rb.AddForce(RelativeStarboard * StrafingSpeed * Throttle * Time.deltaTime);
        Fume_StrafeTowardStarboard();
    }

    public virtual void TurnClockwise()
    {
        rb.angularVelocity -= TurningSpeed * Throttle * Time.deltaTime;
        Fume_TurnClockwise();
    }

    public virtual void TurnCounterClockwise()
    {
        rb.angularVelocity += TurningSpeed * Throttle * Time.deltaTime;
        Fume_TurnCounterClockwise();
    }

    #endregion

    #region Fumes

    public virtual void Fume_MovingForward()
    {
        for(int i = 0; i < ThrustersWhenMoveForward.Length; i++)
        {
            SpawnEngineFumes(ThrustersWhenMoveForward[i].transform.position, ThrustersWhenMoveForward[i].GetVector(), 1);
        }

    }

    public virtual void Fume_StrafeTowardPort()
    {
        for (int i = 0; i < ThrustersWhenStrafeTowardPort.Length; i++)
        {
            SpawnEngineFumes(ThrustersWhenStrafeTowardPort[i].transform.position, ThrustersWhenStrafeTowardPort[i].GetVector(), 1);
        }
    }

    public virtual void Fume_StrafeTowardStarboard()
    {
        for (int i = 0; i < ThrustersWhenStrafeTowardStarboard.Length; i++)
        {
            SpawnEngineFumes(ThrustersWhenStrafeTowardStarboard[i].transform.position, ThrustersWhenStrafeTowardStarboard[i].GetVector(), 1);
        }
    }

    public virtual void Fume_TurnClockwise()
    {
        for (int i = 0; i < ThrustersWhenTurnClockwise.Length; i++)
        {
            SpawnEngineFumes(ThrustersWhenTurnClockwise[i].transform.position, ThrustersWhenTurnClockwise[i].GetVector(), 1);
        }
    }

    public virtual void Fume_TurnCounterClockwise()
    {
        for (int i = 0; i < ThrustersWhenTurnCounterClockwise.Length; i++)
        {
            SpawnEngineFumes(ThrustersWhenTurnCounterClockwise[i].transform.position, ThrustersWhenTurnCounterClockwise[i].GetVector(), 1);
        }
    }
    
    public virtual void SpawnEngineFumes(Vector3 Position, Vector2 Vector, float Magnitude = 1)
    {
        Vector += rb.velocity / 60;

        GameObject go = new GameObject();
        go.AddComponent<Particle>();
        go.transform.parent = transform;
        go.transform.position = Position;
        Particle p = go.GetComponent<Particle>();
        p.UseShifts = true;
        p.LeaveParent = true;
        p.StartAt00 = false;
        p.sprite = Resources.Load<Sprite>("pixel");
        p.Lifespan = Random.Range(10, 40);
        p.StartingScale = 0.8f;
        p.EndingScale = 6f;
        p.StartingColor = FumeColorStart;
        p.EndingColor = FumeColorEnd;
        p.StartingHorizontalWind = Vector.x;
        p.StartingVerticalWind = Vector.y;
        p.EndingHorizontalWind = Vector.x + Random.Range(-0.08f, 0.08f);
        p.EndingVerticalWind = Vector.y + Random.Range(-0.08f, 0.08f);
    }

    #endregion

}
