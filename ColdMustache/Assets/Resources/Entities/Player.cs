using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //after player shoots, he will be briefly stuck in aiming position (making him slower) (in seconds)
    float BaseShootingWindDownDuration = 0.3f;
    float CurrShootingWindDownDuration = 0;
    //duration between shots (in seconds)
    float BaseShootingCooldown = 0.2f;
    float CurrShootingCooldown = 0;

    Entity entity;

    public SpriteManagerHand spriteManagerHand;

    public Weapon CurrentlyEquippedWeapon;

    public ActiveItem CurrentlyEquippedItem = null;

    public float Inaccuracy = 0;
    //ranges between 0 and 1
    //random bullet spread is multiplied by this value
    //inaccuracy is decreased every frame (but to no less than one)
    //moving and shooting increase this value
    public float InaccuracyMin = 0;
    public float InaccuracyMax = 1;
    public float InaccuracyRecoveryFactor = 4; //how much inaccuracy you lose every second

    public float InaccuracyRecoveryBlock = 0; //how many seconds will inaccuracy not decrease for

    public float InaccuracyInLastFrame = 0;

    public SpriteRenderer LowHealthOverlay;


    public GunRotatorHand gunRotatorHand;
    public bool ActivelyAiming = true;


    // Use this for initialization
    void Start() {
        entity = GetComponent<Entity>();
    }

    // Update is called once per frame
    void Update() {

        if(UniversalReference.MouseScreenPosDelta != new Vector2(0, 0))
        {
            if (ActivelyAiming)
            {
                gunRotatorHand.AimGun(UniversalReference.MouseWorldPos);
            }
            else
            {
                gunRotatorHand.HoldGun(UniversalReference.MouseWorldPos);
            }
        }

        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        entity.LookingToward = MouseWorldPos;



        //decreasing active cooldowns
        if (CurrShootingWindDownDuration > 0)
        {
            //when decreasing cooldowns, always use Time.deltaTime (the time it took to render last frame, in seconds) - framerate independent
            CurrShootingWindDownDuration -= Time.deltaTime;
        }
        if (CurrShootingCooldown > 0)
        {
            CurrShootingCooldown -= Time.deltaTime;
        }


        //inaccuracy
        Inaccuracy += UniversalReference.PlayerRb.velocity.magnitude / 30;

        //inaccuracy starts decreasing only when it has not increased
        if (Inaccuracy <= InaccuracyInLastFrame && InaccuracyRecoveryBlock <= 0)
        {
            if (Inaccuracy > InaccuracyMin)
            {
                Inaccuracy -= Time.deltaTime * InaccuracyRecoveryFactor; //decreased by 1 every second (can be different for balance purposes)
            }
        }

        if (InaccuracyRecoveryBlock > 0)
        {
            InaccuracyRecoveryBlock -= Time.deltaTime;
        }

        if (Inaccuracy > InaccuracyMax)
        {
            Inaccuracy = InaccuracyMax;
        }
        if (Inaccuracy < InaccuracyMin)
        {
            Inaccuracy = InaccuracyMin;
        }

        InaccuracyInLastFrame = Inaccuracy;

        /*

        //when not firing/aiming, hold the gun in resting position
        if(Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1) || CurrShootingWindDownDuration > 0)
        {
            smg.ActivelyAiming = true;
        }
        else
        {
            smg.ActivelyAiming = false;
        }
        
        */

        //movement vector will hold information about direction, speed is added after, in Entity script
        //you cannot walk when mouse is dragging a window, that causes problems (though MovementVector has to be set to 0 stop previous movement)
        Vector2 MovementVector = new Vector2(0, 0);
        if(MouseInterceptor.IsMouseAvailable()){

            if (Input.GetKey(KeybindManager.MoveUp))
            {
                MovementVector += new Vector2(0, 1);
            }
            if (Input.GetKey(KeybindManager.MoveDown))
            {
                MovementVector += new Vector2(0, -1);
            }
            if (Input.GetKey(KeybindManager.MoveLeft))
            {
                MovementVector += new Vector2(-1, 0);
            }
            if (Input.GetKey(KeybindManager.MoveRight))
            {
                MovementVector += new Vector2(1, 0);
            }
            entity.MoveInDirection(MovementVector);
        }
        

        float HealthRatio = entity.Health / entity.MaxHealth;
        if(HealthRatio < 0.5f)
        {
            LowHealthOverlay.color = new Color(LowHealthOverlay.color.r, LowHealthOverlay.color.g, LowHealthOverlay.color.b,
                (0.5f - HealthRatio)*0.85f
                );
        }
        else
        {
            LowHealthOverlay.color = new Color(LowHealthOverlay.color.r, LowHealthOverlay.color.g, LowHealthOverlay.color.b,0);
        }
        
        //test purposes - can delete

        if (Input.GetKeyDown(KeyCode.F10))
        {
            //export navmap as text

            //hard path means Drive:/path - the complete path, not relative to application directory
            NavTestStatic.ExportNavMap();           

            Debug.Log("hi");
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            //Grenade(MouseWorldPos);
        }

        if (Input.GetKeyUp(KeyCode.Mouse2))
        {
            //NavTestStatic.SightLine(transform.position, UniversalReference.MouseWorldPos);
            //NavTestStatic.FieldOfView(transform.position);
        }
        
        if(CheatManager.LastCheat == "GODMODE")
        {
            entity.MaxHealth = float.MaxValue;
            entity.Health = float.MaxValue;
            entity.BaseMoveSpeed = 25;
        }

        if (CheatManager.LastCheat == "NUKE")
        {
            //ExplosionFrag.SpawnOriginal(Util.Vector3To2Int(transform.position), 100, 10);
            ExplosionFrag.SpawnOriginal(Util.Vector3To2Int(transform.position), 5);
        }

        if (CheatManager.LastCheat == "KILLALL")
        {
            foreach(Entity e in GameObject.FindObjectsOfType<Entity>()){
                if(e.Team == Entity.team.Enemy) Destroy(e.gameObject);
            }
        }

        if (CheatManager.LastCheat == "DRAWUNWALKABLE")
        {
            for(int x = 0; x < NavTestStatic.MapWidth; x++){
                for(int y = 0; y < NavTestStatic.MapHeight; y++){
                    if(!NavTestStatic.IsTileWalkable(x,y)){
                        GameObject go = new GameObject();
                        go.transform.position = new Vector3(x,y,-10);
                        go.AddComponent<SpriteRenderer>().sprite = UniversalReference.Pixel;
                        go.GetComponent<SpriteRenderer>().color = new Color(1,0,0,0.6f);
                    }
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            AlphabetManager.SpawnFloatingText("Hi!", new Vector3(transform.position.x, transform.position.y, -35));
        }
        
        //end of test stuff

        if(Input.GetKeyDown(KeybindManager.UseItem)){
            if(CurrentlyEquippedItem!=null){
                CurrentlyEquippedItem.DoYourThing();
            }
        }
        
        if (Input.GetKeyUp(KeyCode.F5))
        {
            SaverLoader.QuickSave(spriteManagerHand);
        }

        //quickload
        if (Input.GetKeyUp(KeyCode.F6))
        {
            SaverLoader.QuickLoad(spriteManagerHand);

            //Debug.Log("Quickload Complete.");
        }

        if (Input.GetKey(KeybindManager.MousePrimary) && MouseInterceptor.IsMouseAvailable())
        {
           CurrentlyEquippedWeapon.TryShooting(MouseWorldPos);
        }
        if (Input.GetKey(KeybindManager.AltFire))
        {
            CurrentlyEquippedWeapon.TryAltFire();
            CurrentlyEquippedWeapon.TryAltFire(MouseWorldPos);
        }
        if (Input.GetKey(KeybindManager.Reload))
        {
            CurrentlyEquippedWeapon.ForceReload();
        }
        if (Input.GetKey(KeyCode.P))
        {
            SpamParticlesToFuckWithFramerate();
        }
    }

    void Shoot(Vector2 Target)
    {
        GameObject bullet = new GameObject();
        bullet.transform.position = transform.position;
        bullet.transform.localScale = new Vector3(0.1f, 0.1f, 1);
        bullet.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("pixel");
        bullet.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0);
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = Util.GetDirectionVectorToward(transform, Target) * 20;
        bullet.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        bullet.AddComponent<FxBulletTrail>();
        bullet.AddComponent<FxBulletExplosion>();

        bullet.AddComponent<DamagerInflicter>().ini(Entity.team.Player, 1, true);
        bullet.AddComponent<InflicterSlow>().ini(2, 0.5f, true);
        bullet.gameObject.name = "bullet";

        bullet.AddComponent<DieInSeconds>().Seconds = 5;
        //bullet.gameObject.tag = "PlayerBullet";
        bullet.gameObject.AddComponent<CircleCollider2D>().isTrigger = true;
        //bullet.AddComponent<InflicterSlow>().ini(Entity.team.Enemy, 2, 0.25f);
    }

    void Grenade(Vector2 Target)
    {
        foreach (Vector3 v in NavTestStatic.CalculateExplosion_DistributorNoBacksiesTileSplit(
                Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x),
                Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y),
                200
               ))
        {

            GameObject blastAnim = new GameObject();
            blastAnim.transform.position = new Vector3(v.x, v.y, -5);
            blastAnim.AddComponent<SpriteRenderer>();

            SpriteSheetAnimation banim = blastAnim.AddComponent<SpriteSheetAnimation>();
            banim.Sprites = Resources.LoadAll<Sprite>("Fx/Explosion");
            banim.LifeSpanInSeconds = 0.5f;
            banim.Mode = SpriteSheetAnimation.Modes.Destroy;

            blastAnim.GetComponent<SpriteRenderer>().color = new Color(1, v.z / 30, 0);
            blastAnim.name = v.z.ToString();


            GameObject ActualBlast = new GameObject();
            ActualBlast.transform.position = new Vector3(v.x, v.y, -5);

            ActualBlast.AddComponent<DamagerInflicter>().ini(Entity.team.Neutral, (int)v.z, true, true, 0, 0, false, DamagerInflicter.WeaponTypes.Explosion);
            ActualBlast.AddComponent<CircleCollider2D>().isTrigger = true;
            ActualBlast.GetComponent<CircleCollider2D>().radius = 0.6f;
            ActualBlast.AddComponent<Rigidbody2D>().isKinematic = true;
            ActualBlast.AddComponent<DieIn>().Frames = 0;


        }
    }

    void SpamParticlesToFuckWithFramerate()
    {
        for (int i = 0; i < 100; i++)
        {
            GameObject go = new GameObject();
            go.AddComponent<Particle>();
            go.transform.position = transform.position;
            Particle p = go.GetComponent<Particle>();
            p.UseShifts = true;
            p.LeaveParent = true;
            p.StartAt00 = false;
            p.sprite = Resources.Load<Sprite>("pixel");
            p.Lifespan = (int)Mathf.Pow(Random.Range(3, 10), 2);
            p.StartingScale = 0.1f;
            p.EndingScale = 0;
            p.StartingColor = new Color(1f, 1f, 0.5f, 1);
            p.EndingColor = new Color(0.5f, 0, 0, 1);
            p.StartingHorizontalWind = Random.Range(-0.05f, 0.05f);
            p.StartingVerticalWind = Random.Range(-0.05f, 0.05f);
            p.EndingHorizontalWind = Random.Range(-0.01f, 0.01f);
            p.EndingVerticalWind = Random.Range(-0.01f, 0.01f);
        }
    }

    public void RequestInaccuracyIncrease(float Inaccuracy)
    {
        if(this.Inaccuracy + Inaccuracy < InaccuracyMax)
        {
            this.Inaccuracy += Inaccuracy;
        }
        else
        {
            this.Inaccuracy = 1;
        }
    }
    
}
