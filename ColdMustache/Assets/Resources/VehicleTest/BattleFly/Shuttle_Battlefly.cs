using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuttle_Battlefly : Shuttle {

    

    Vector3 RearMainThrusterPortSide = new Vector3(-41, 6.85f,0);
    Vector3 RearMainThrusterStarboardSide = new Vector3(-41, -6.85f, 0);

    //from stern to bow; A is the stern-most in the cluster, C is the bow-most
    Vector3 SternPortClusterA = new Vector3(-32.4f, 12.58f, 0);
    Vector3 SternPortClusterB = new Vector3(-27.6f, 12.58f, 0);
    Vector3 SternPortClusterC = new Vector3(-23.05f, 12.58f, 0);

    Vector3 SternStarboardClusterA = new Vector3(-32.4f, -12.58f, 0);
    Vector3 SternStarboardClusterB = new Vector3(-27.6f, -12.58f, 0);
    Vector3 SternStarboardClusterC = new Vector3(-23.05f, -12.58f, 0);

    Vector3 BowPortClusterA = new Vector3(17.45f,9.88f,0);
    Vector3 BowPortClusterB = new Vector3(22.51f,9.12f,0);
    Vector3 BowPortClusterC = new Vector3(27.49f,8.02f,0);

    Vector3 BowStarboardClusterA = new Vector3(17.45f, -9.88f, 0);
    Vector3 BowStarboardClusterB = new Vector3(22.51f, -9.12f, 0);
    Vector3 BowStarboardClusterC = new Vector3(27.49f, -8.02f, 0);

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (true)
        {

            RelativeBow = Util.RotateVector(DefaultBow, transform.rotation.eulerAngles.z);
            RelativeStern = Util.RotateVector(DefaultStern, transform.rotation.eulerAngles.z);
            RelativePort = Util.RotateVector(DefaultPort, transform.rotation.eulerAngles.z);
            RelativeStarboard = Util.RotateVector(DefaultStarboard, transform.rotation.eulerAngles.z);
            
            if (Input.GetKey(KeyCode.Q))
            {
                rb.angularVelocity += TurningSpeed * Throttle * Time.deltaTime;

                
                SpawnEngineFumes(BowStarboardClusterA, Throttle, Sides.Starboard);
                SpawnEngineFumes(BowStarboardClusterB, Throttle, Sides.Starboard);
                SpawnEngineFumes(BowStarboardClusterC, Throttle, Sides.Starboard);
                
                SpawnEngineFumes(SternPortClusterA, Throttle, Sides.Port);
                SpawnEngineFumes(SternPortClusterB, Throttle, Sides.Port);
                SpawnEngineFumes(SternPortClusterC, Throttle, Sides.Port);
                

            }
            if (Input.GetKey(KeyCode.E))
            {
                rb.angularVelocity -= TurningSpeed * Throttle * Time.deltaTime;

                
                SpawnEngineFumes(BowPortClusterA, Throttle, Sides.Port);
                SpawnEngineFumes(BowPortClusterB, Throttle, Sides.Port);
                SpawnEngineFumes(BowPortClusterC, Throttle, Sides.Port);
                
                SpawnEngineFumes(SternStarboardClusterA, Throttle, Sides.Starboard);
                SpawnEngineFumes(SternStarboardClusterB, Throttle, Sides.Starboard);
                SpawnEngineFumes(SternStarboardClusterC, Throttle, Sides.Starboard);
                
            }
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(RelativeBow * ForwardSpeed * Throttle * Time.deltaTime);
                
                SpawnEngineFumes(RearMainThrusterPortSide, 3 * Throttle, Sides.Stern);
                SpawnEngineFumes(RearMainThrusterStarboardSide, 3 * Throttle, Sides.Stern);

            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(RelativeStern * ReverseSpeed * Throttle * Time.deltaTime);

                //bow cluster - bowside thrusters
                SpawnEngineFumes(new Vector3(2.965f, 1.172f, 1), Throttle, Sides.Bow);
                SpawnEngineFumes(new Vector3(2.965f, 0.920f, 1), Throttle, Sides.Bow);

                //bow cluster - bowside thrusters
                SpawnEngineFumes(new Vector3(2.965f, -1.172f, 1), Throttle, Sides.Bow);
                SpawnEngineFumes(new Vector3(2.965f, -0.920f, 1), Throttle, Sides.Bow);
            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(RelativePort * StrafingSpeed * Throttle * Time.deltaTime);
                
                SpawnEngineFumes(SternStarboardClusterA, Throttle, Sides.Starboard);
                SpawnEngineFumes(SternStarboardClusterB, Throttle, Sides.Starboard);
                SpawnEngineFumes(SternStarboardClusterC, Throttle, Sides.Starboard);
                
                SpawnEngineFumes(BowStarboardClusterA, Throttle, Sides.Starboard);
                SpawnEngineFumes(BowStarboardClusterB, Throttle, Sides.Starboard);
                SpawnEngineFumes(BowStarboardClusterC, Throttle, Sides.Starboard);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(RelativeStarboard * StrafingSpeed * Throttle * Time.deltaTime);
                
                SpawnEngineFumes(SternPortClusterA, Throttle, Sides.Port);
                SpawnEngineFumes(SternPortClusterB, Throttle, Sides.Port);
                SpawnEngineFumes(SternPortClusterC, Throttle, Sides.Port);
                
                SpawnEngineFumes(BowPortClusterA, Throttle, Sides.Port);
                SpawnEngineFumes(BowPortClusterB, Throttle, Sides.Port);
                SpawnEngineFumes(BowPortClusterC, Throttle, Sides.Port);
            }

            //throttle controls
            if (Input.GetKey(KeyCode.R))
            {
                Throttle += ThrottleIncreaseSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.F))
            {
                Throttle -= ThrottleIncreaseSpeed * Time.deltaTime;
            }
            if (Throttle > 1)
            {
                Throttle = 1;
            }
            if (Throttle < 0)
            {
                Throttle = 0;
            }
            //ThrottleText.text = (Throttle * 100).ToString().PadRight(4).Substring(0, 4);
            
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
            } 
        }
        else
        {
        }


    }

    public override void SpawnEngineFumes(Vector3 Position, float Magnitude, Sides Side)
    {
        Vector2 MainVector = new Vector2(0, 0);

        //Magnitude *= 10;

        switch (Side)
        {
            case Sides.Bow:
                MainVector = RelativeBow * Magnitude + rb.velocity / 60;
                break;
            case Sides.Stern:
                MainVector = RelativeStern * Magnitude + rb.velocity / 60;
                break;
            case Sides.Port:
                MainVector = RelativePort * Magnitude + rb.velocity / 60;
                break;
            case Sides.Starboard:
                MainVector = RelativeStarboard * Magnitude + rb.velocity / 60;
                break;
        }

        GameObject go = new GameObject();
        go.AddComponent<Particle>();
        go.transform.parent = transform;
        go.transform.localPosition = Position;
        Particle p = go.GetComponent<Particle>();
        p.UseShifts = true;
        p.LeaveParent = true;
        p.StartAt00 = false;
        p.sprite = Resources.Load<Sprite>("pixel");
        p.Lifespan = Random.Range(10, 40);
        p.StartingScale = 0.8f * Magnitude;
        p.EndingScale = 6f * Magnitude;
        p.StartingColor = new Color(1f, 1f, 1f, 1);
        p.EndingColor = new Color(0f, 0, 1, 0);
        p.StartingHorizontalWind = MainVector.x;
        p.StartingVerticalWind = MainVector.y;
        p.EndingHorizontalWind = MainVector.x + Random.Range(-0.08f, 0.08f);
        p.EndingVerticalWind = MainVector.y + Random.Range(-0.08f, 0.08f);
    }
        
}
