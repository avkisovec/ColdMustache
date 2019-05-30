using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    
    /*
     
     attatch this script to the container

     the container has a trigger collider, when something enters it,
     it moves its child (the actual door)
    
     */

    public Transform TheActualDoor;

    //positive for doors moving right, negative for doors moving left (when they are closed and are opening)
    public float SpeedAndAlsoDirection = 1;

    public float MinRelX = -1;
    public float MaxRelX = 0;

    float TimeSinceCollision = 99999;

    //how long will door stay open after being triggered
    //never set it to 0, otherwise it may not work properly
    public float TimeToHoldOpen = 0.5f;

    //after this time, the door is considered fully closed
    //this affects things like whether or not enemies can see through or grenades will bounce off
    public float TimeToBeConsideredFullyClosed = 0.5f;

    //such as when one door segment triggers it will also trigger the other half
    public Door[] DoorToAlsoTrigger;

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
        if(TheActualDoor == null) {
            CustomDestroy();
            return;
        }

        foreach(Vector2Int v in RelativePositionsOfDoorframe){
            if(NavTestStatic.IsTileWalkable(MyPos + v)){
                CustomDestroy();
                return;
            }
        }

        if(TimeSinceCollision > TimeToHoldOpen){

            //close

            float NewDoorX = TheActualDoor.localPosition.x - SpeedAndAlsoDirection*Time.deltaTime;
            if(NewDoorX > MaxRelX) NewDoorX = MaxRelX;
            if (NewDoorX < MinRelX) NewDoorX = MinRelX;

            TheActualDoor.localPosition = new Vector3(NewDoorX, TheActualDoor.localPosition.y, TheActualDoor.localPosition.z);

        }
        else{

            //open

            float NewDoorX = TheActualDoor.localPosition.x + SpeedAndAlsoDirection * Time.deltaTime;
            if (NewDoorX > MaxRelX) NewDoorX = MaxRelX;
            if (NewDoorX < MinRelX) NewDoorX = MinRelX;

            TheActualDoor.localPosition = new Vector3(NewDoorX, TheActualDoor.localPosition.y, TheActualDoor.localPosition.z);


        }

        if(TimeSinceCollision > TimeToBeConsideredFullyClosed){

            //considered fully closed for purposes of light and explosions (explosions also affect grenade bounces)
            NavTestStatic.LightNavArray[MyPos.x, MyPos.y] = NavTestStatic.ImpassableTileValue;
            NavTestStatic.ExplosionNavArray[MyPos.x, MyPos.y] = NavTestStatic.ImpassableTileValue;
        }
        else{
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
            foreach(Door d in DoorToAlsoTrigger){
                if(d!=null) d.Trigger();
            }
            
        }
    }


    public void Trigger(){

        TimeSinceCollision = 0;
    }

    public void CustomDestroy(){
        NavTestStatic.LightNavArray[MyPos.x, MyPos.y] = NavTestStatic.EmptyTileValue;
        NavTestStatic.ExplosionNavArray[MyPos.x, MyPos.y] = NavTestStatic.EmptyTileValue;
        Destroy(this.gameObject);
    }


}
