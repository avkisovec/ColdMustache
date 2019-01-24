using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shuttle : MonoBehaviour {

    public enum Sides { Bow, Stern, Port, Starboard }

    //between 0 and 1, all speeds get multiplied by this
    public float Throttle = 1;

    public Text ThrottleText;

    public float ThrottleIncreaseSpeed = 1;

    public float TurningSpeed = 300f;
    public float ForwardSpeed = 600f;
    public float ReverseSpeed = 60f;
    public float StrafingSpeed = 120f;

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

    public Transform BowIndicator;
    public Transform SternIndicator;
    public Transform PortIndicator;
    public Transform StarboardIndicator;

    public Transform Turret;

    public Transform OutsideLayer;
    public Transform InteriorLayer;

    public int FramesSinceTouchedDock = 0;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();        
	}
	
	// Update is called once per frame
	void Update () {

        if (!Docked)
        {

            RelativeBow = Util.RotateVector(DefaultBow, transform.rotation.eulerAngles.z);
            RelativeStern = Util.RotateVector(DefaultStern, transform.rotation.eulerAngles.z);
            RelativePort = Util.RotateVector(DefaultPort, transform.rotation.eulerAngles.z);
            RelativeStarboard = Util.RotateVector(DefaultStarboard, transform.rotation.eulerAngles.z);

            BowIndicator.transform.position = (Vector2)transform.position + (RelativeBow * 15);
            SternIndicator.transform.position = (Vector2)transform.position + (RelativeStern * 15);
            PortIndicator.transform.position = (Vector2)transform.position + (RelativePort * 15);
            StarboardIndicator.transform.position = (Vector2)transform.position + (RelativeStarboard * 15);

            if (Input.GetKey(KeyCode.Q))
            {
                rb.angularVelocity += TurningSpeed * Throttle * Time.deltaTime;

                FumeTurnCounterClockwise();

            }
            if (Input.GetKey(KeyCode.E))
            {
                rb.angularVelocity -= TurningSpeed * Throttle * Time.deltaTime;

                FumeTurnClockwise();

            }
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(RelativeBow * ForwardSpeed * Throttle * Time.deltaTime);
                FumeFromStern();

            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(RelativeStern * ReverseSpeed * Throttle * Time.deltaTime);
                FumeFromBow();
            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(RelativePort * StrafingSpeed * Throttle * Time.deltaTime);

                FumeFromStarboard();
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(RelativeStarboard * StrafingSpeed * Throttle * Time.deltaTime);

                FumeFromPort();
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
            ThrottleText.text = (Throttle * 100).ToString().PadRight(4).Substring(0, 4);

            Util.RotateTransformToward(Turret, Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //bullet object, position, scale ...
                GameObject bullet = new GameObject();
                bullet.transform.position = Turret.position;
                bullet.transform.localScale = new Vector3(3f, 0.5f, 1);

                GameObject BulletLightCircle = new GameObject();
                BulletLightCircle.transform.parent = bullet.transform;
                BulletLightCircle.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Fx/Goop256");
                BulletLightCircle.GetComponent<SpriteRenderer>().color = new Color(1, 0.6f, 0, 1);
                BulletLightCircle.transform.localPosition = new Vector3(0, 0, 0.1f);
                BulletLightCircle.transform.localScale = new Vector3(4, 12, 1);


                Util.RotateTransformToward(bullet.transform, Camera.main.ScreenToWorldPoint(Input.mousePosition));

                //bullet physics
                bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                bullet.GetComponent<Rigidbody2D>().velocity = Util.GetDirectionVectorToward(transform, Camera.main.ScreenToWorldPoint(Input.mousePosition)) * 250;
                bullet.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                bullet.gameObject.AddComponent<CircleCollider2D>().isTrigger = true;
                bullet.GetComponent<CircleCollider2D>().radius = 3f;

                //bullet visuals
                bullet.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Fx/Goop256");
                bullet.GetComponent<SpriteRenderer>().color = new Color(1, 0.6f, 0, 0.5f);
                bullet.AddComponent<FxShipRailgunTrail>();

                /*
                //custom mechanics
                bullet.AddComponent<DamagerInflicter>().ini(Entity.team.Player, 10, true);
                bullet.AddComponent<DieInSeconds>().Seconds = 5;
                bullet.AddComponent<SlowlySlowDown>().EffectiveRange = EffectiveRange;
                bullet.AddComponent<InflicterSlow>().ini(2, 0.5f, true);
                */

                bullet.AddComponent<DieInSeconds>().Seconds = 5;

                /*
                //muzzle flash (visual effect)
                GameObject Muzzleflash = new GameObject();
                Muzzleflash.transform.parent = UniversalReference.GunRotator.GunSpriteRenderer.transform;
                Muzzleflash.transform.localPosition = new Vector3(0, 0, 0); //split into two to set local position 0 0, ant then z -31 according to z-index protocol
                Muzzleflash.transform.position = new Vector3(Muzzleflash.transform.position.x, Muzzleflash.transform.position.y, -31);
                Muzzleflash.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                Muzzleflash.transform.localScale = new Vector3(1, 1, 1);
                Muzzleflash.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(MuzzleFlashSpritePath);
                Muzzleflash.GetComponent<SpriteRenderer>().color = BulletColorStart;
                Muzzleflash.AddComponent<DieIn>().Frames = 0; //original: 0
                */

                /*
                //light from muzzle flash
                if (PlayerBeingInDarknessManager.IsPlayerInDarkness)
                {
                    GameObject MuzzleFlashLight = new GameObject();
                    MuzzleFlashLight.AddComponent<SpriteMask>().sprite = Resources.Load<Sprite>("circle512");
                    MuzzleFlashLight.transform.position = transform.position;
                    MuzzleFlashLight.transform.localScale = new Vector3(1, 1, 1);
                    MuzzleFlashLight.AddComponent<DieInFramesFramerateIndependent>().Frames = 2;
                }
                */

                /*
                //increase inaccuracy
                UniversalReference.PlayerScript.RequestInaccuracyIncrease(InaccuracyPerShot);
                if (UniversalReference.PlayerScript.InaccuracyRecoveryBlock < BaseCooldownBetweenShots)
                {
                    UniversalReference.PlayerScript.InaccuracyRecoveryBlock += BaseCooldownBetweenShots;
                }
                */
            }

            if (Input.GetKeyUp(KeyCode.Return))
            {
                BecomeDocked();
            }

        }
        else
        {
            transform.position = DockedPosition;
            transform.rotation = DockedRotation;
            if (Input.GetKeyUp(KeyCode.Return))
            {
                BecomeUndocked();
            }
        }


    }


    public virtual void FumeFromStern()
    {
        //rear thrusters
        SpawnEngineFumes(new Vector3(-2.905f, 1.848f, 1), 0.5f * Throttle, Sides.Stern);
        SpawnEngineFumes(new Vector3(-2.905f, -1.848f, 1), 0.5f * Throttle, Sides.Stern);
    }

    public virtual void FumeFromBow()
    {
        //bow cluster - bowside thrusters
        SpawnEngineFumes(new Vector3(2.965f, 1.172f, 1), 0.1f * Throttle, Sides.Bow);
        SpawnEngineFumes(new Vector3(2.965f, 0.920f, 1), 0.1f * Throttle, Sides.Bow);

        //bow cluster - bowside thrusters
        SpawnEngineFumes(new Vector3(2.965f, -1.172f, 1), 0.1f * Throttle, Sides.Bow);
        SpawnEngineFumes(new Vector3(2.965f, -0.920f, 1), 0.1f * Throttle, Sides.Bow);
    }

    public virtual void FumeFromPort()
    {
        //portside strafe thruster
        SpawnEngineFumes(new Vector3(-0.768f, 2.229f, 1), 0.2f * Throttle, Sides.Port);
        SpawnEngineFumes(new Vector3(-0.433f, 2.229f, 1), 0.2f * Throttle, Sides.Port);

        //bow cluster - portside thrusters
        SpawnEngineFumes(new Vector3(2.471f, 1.414f, 1), 0.1f * Throttle, Sides.Port);
        SpawnEngineFumes(new Vector3(2.722f, 1.414f, 1), 0.1f * Throttle, Sides.Port);
    }

    public virtual void FumeFromStarboard()
    {
        //starboardside strafe thruster
        SpawnEngineFumes(new Vector3(-0.768f, -2.229f, 1), 0.2f * Throttle, Sides.Starboard);
        SpawnEngineFumes(new Vector3(-0.433f, -2.229f, 1), 0.2f * Throttle, Sides.Starboard);

        //bow cluster - starboardside thrusters
        SpawnEngineFumes(new Vector3(2.471f, -1.414f, 1), 0.1f * Throttle, Sides.Starboard);
        SpawnEngineFumes(new Vector3(2.722f, -1.414f, 1), 0.1f * Throttle, Sides.Starboard);
    }

    public virtual void FumeTurnClockwise()
    {
        //bow cluster - portside thrusters
        SpawnEngineFumes(new Vector3(2.471f, 1.414f, 1), 0.1f * Throttle, Sides.Port);
        SpawnEngineFumes(new Vector3(2.722f, 1.414f, 1), 0.1f * Throttle, Sides.Port);

        //starboardside strafe thruster
        SpawnEngineFumes(new Vector3(-0.768f, -2.229f, 1), 0.1f * Throttle, Sides.Starboard);
        SpawnEngineFumes(new Vector3(-0.433f, -2.229f, 1), 0.1f * Throttle, Sides.Starboard);

        //bow cluster - bowside thrusters
        SpawnEngineFumes(new Vector3(2.965f, -1.172f, 1), 0.1f * Throttle, Sides.Bow);
        SpawnEngineFumes(new Vector3(2.965f, -0.920f, 1), 0.1f * Throttle, Sides.Bow);

        //rear thrusters
        SpawnEngineFumes(new Vector3(-2.905f, 1.848f, 1), 0.1f * Throttle, Sides.Stern);
    }

    public virtual void FumeTurnCounterClockwise()
    {
        //bow cluster - starboardside thrusters
        SpawnEngineFumes(new Vector3(2.471f, -1.414f, 1), 0.1f * Throttle, Sides.Starboard);
        SpawnEngineFumes(new Vector3(2.722f, -1.414f, 1), 0.1f * Throttle, Sides.Starboard);

        //portside strafe thruster
        SpawnEngineFumes(new Vector3(-0.768f, 2.229f, 1), 0.1f * Throttle, Sides.Port);
        SpawnEngineFumes(new Vector3(-0.433f, 2.229f, 1), 0.1f * Throttle, Sides.Port);

        //bow cluster - bowside thrusters
        SpawnEngineFumes(new Vector3(2.965f, 1.172f, 1), 0.1f * Throttle, Sides.Bow);
        SpawnEngineFumes(new Vector3(2.965f, 0.920f, 1), 0.1f * Throttle, Sides.Bow);

        //rear thrusters
        SpawnEngineFumes(new Vector3(-2.905f, -1.848f, 1), 0.1f * Throttle, Sides.Stern);
    }



    public virtual void SpawnEngineFumes(Vector3 Position, float Magnitude, Sides Side)
    {
        Vector2 MainVector = new Vector2(0, 0);

        switch (Side)
        {
            case Sides.Bow:
                MainVector = RelativeBow * Magnitude + rb.velocity/ 60;
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




    public bool Docked = false;
    Vector3 DockedPosition = new Vector3(0, 0, 0);
    Quaternion DockedRotation = new Quaternion(0,0,0,0);

    public void BecomeDocked()
    {
        Docked = true;
        DockedPosition = transform.position;
        DockedRotation = transform.rotation;

        OutsideLayer.transform.localPosition = new Vector3(9999, -9999, 9999);
        InteriorLayer.gameObject.GetComponent<PolygonCollider2D>().enabled = true;

    }

    public void BecomeUndocked()
    {
        Docked = false;
        InteriorLayer.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        OutsideLayer.transform.localPosition = new Vector3(0, 0, -1);
    }

}
