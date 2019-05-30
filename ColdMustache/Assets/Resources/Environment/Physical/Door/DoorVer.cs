using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorVer : MonoBehaviour
{

    /*
     
     version of Door script, but for vertical doors
     (ones that open vertically and you go through them horizontally)
    
     */

    public Transform TheActualDoor;

    //positive for doors moving right, negative for doors moving left (when they are closed and are opening)
    public float SpeedAndAlsoDirection = 1;

    public float MinRelY = -1;
    public float MaxRelY = 0;

    float TimeSinceCollision = 100;

    //how long will door stay open after being triggered
    //never set it to 0, otherwise it may not work properly
    public float TimeToHoldOpen = 0.5f;

    //after this time, the door is considered fully closed
    //this affects things like whether or not enemies can see through or grenades will bounce off
    public float TimeToBeConsideredFullyClosed = 0.5f;

    //such as when one door segment triggers it will also trigger the other half
    public DoorVer[] DoorToAlsoTrigger;

    //die when any part of doorframe is destroyed (is null)
    //checks these relative positions in navmap, if one of these positions if walkable, that means the wall there was destroyed and this object destroyes self
    public Vector2Int[] RelativePositionsOfDoorframe;

    Vector2Int MyPos;

    void Start()
    {
        MyPos = Util.Vector3To2Int(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (TheActualDoor == null)
        {
            CustomDestroy();
            return;
        }

        foreach (Vector2Int v in RelativePositionsOfDoorframe)
        {
            if (NavTestStatic.IsTileWalkable(MyPos + v))
            {
                CustomDestroy();
                return;
            }
        }

        if (TimeSinceCollision > TimeToHoldOpen)
        {

            //close

            float NewDoorY = TheActualDoor.localPosition.y - SpeedAndAlsoDirection * Time.deltaTime;
            if (NewDoorY > MaxRelY) NewDoorY = MaxRelY;
            if (NewDoorY < MinRelY) NewDoorY = MinRelY;

            TheActualDoor.localPosition = new Vector3(TheActualDoor.localPosition.x, NewDoorY, TheActualDoor.localPosition.z);

        }
        else
        {

            //open

            float NewDoorY = TheActualDoor.localPosition.y + SpeedAndAlsoDirection * Time.deltaTime;
            if (NewDoorY > MaxRelY) NewDoorY = MaxRelY;
            if (NewDoorY < MinRelY) NewDoorY = MinRelY;

            TheActualDoor.localPosition = new Vector3(TheActualDoor.localPosition.x, NewDoorY, TheActualDoor.localPosition.z);


        }

        if (TimeSinceCollision > TimeToBeConsideredFullyClosed)
        {

            //considered fully closed for purposes of light and explosions (explosions also affect grenade bounces)
            NavTestStatic.LightNavArray[MyPos.x, MyPos.y] = NavTestStatic.ImpassableTileValue;
            NavTestStatic.ExplosionNavArray[MyPos.x, MyPos.y] = NavTestStatic.ImpassableTileValue;
        }
        else
        {
            //considered fully open
            NavTestStatic.LightNavArray[MyPos.x, MyPos.y] = NavTestStatic.EmptyTileValue;
            NavTestStatic.ExplosionNavArray[MyPos.x, MyPos.y] = NavTestStatic.EmptyTileValue;
        }

        TimeSinceCollision += Time.deltaTime;
    }




    private void OnTriggerEnter2D(Collider2D coll)
    {
        Collision(coll);
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        Collision(coll);
    }

    private void Collision(Collider2D coll)
    {
        Entity hit = coll.GetComponent<Entity>();
        if (hit != null)
        {
            Trigger();
            foreach (DoorVer d in DoorToAlsoTrigger)
            {
                if (d != null) d.Trigger();
            }

        }
    }


    public void Trigger()
    {

        TimeSinceCollision = 0;
    }


    public void CustomDestroy()
    {
        NavTestStatic.LightNavArray[MyPos.x, MyPos.y] = NavTestStatic.EmptyTileValue;
        NavTestStatic.ExplosionNavArray[MyPos.x, MyPos.y] = NavTestStatic.EmptyTileValue;
        Destroy(this.gameObject);
    }


}
