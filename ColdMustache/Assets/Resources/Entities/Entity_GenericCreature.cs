using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_GenericCreature : Entity
{

    public KeyframerController kfc;

    public Transform LookingDownContainer;
    public Transform LookingRightContainer;
    public Transform LookingUpContainer;

    //1 = normal -1 = reverse (speed will be multiplied by this)
    float WalkAnimPlayDirection = 1;
    public float WalkAnimPlaySpeed = 1f;


    // Start is called before the first frame update
    void Start()
    {
        kfc = GetComponent<KeyframerController>();

        UniqueId = EntityIdDistributor.GetUniqueId();
        rb = GetComponent<Rigidbody2D>();
        if (spriteManager == null && UseSpriteManager)
        {
            spriteManager = GetComponent<SpriteManagerBase>();
        }        
    }

    // Update is called once per frame
    void Update()
    {

        kfc.TimeSpeed = rb.velocity.magnitude * WalkAnimPlayDirection * WalkAnimPlaySpeed;



    }

    int LastDirection = -1;

    //sets directional sprites
    void LookInDirection(int Direction){
        switch(Direction){
            case 0: // right
                LookingRightContainer.transform.localPosition = new Vector3(0, 0, 0);
                LookingRightContainer.transform.localScale = new Vector3(1,1,1);
                LookingUpContainer.transform.localPosition = new Vector3(999, 999, 999);
                LookingDownContainer.transform.localPosition = new Vector3(999, 999, 999);
                WalkAnimPlayDirection = 1;
            return;
            case 90: //up
                LookingRightContainer.transform.localPosition = new Vector3(999, 999, 999);
                LookingUpContainer.transform.localPosition = new Vector3(0, 0, 0);
                LookingDownContainer.transform.localPosition = new Vector3(999, 999, 999);
                WalkAnimPlayDirection = 1;
            return;
            case 180: //left
                LookingRightContainer.transform.localPosition = new Vector3(0, 0, 0);
                LookingRightContainer.transform.localScale = new Vector3(-1, 1, 1);
                LookingUpContainer.transform.localPosition = new Vector3(999, 999, 999);
                LookingDownContainer.transform.localPosition = new Vector3(999, 999, 999);
                WalkAnimPlayDirection = 1;
            return;
            case 270: //down
                LookingRightContainer.transform.localPosition = new Vector3(999, 999, 999);
                LookingUpContainer.transform.localPosition = new Vector3(999, 999, 999);
                LookingDownContainer.transform.localPosition = new Vector3(0, 0, 0);
                WalkAnimPlayDirection = -1;
            return;
        }
    }


    public override void MoveInDirection(Vector2 MovementVector)
    {
        //fixing some bug when computing/assigning vector 0,0
        if (MovementVector.x != 0 || MovementVector.y != 0)
        {
            //dividing by magnitude to make diagonal travel as fast as horizontal/vertical
            rb.velocity = MovementVector / MovementVector.magnitude * BaseMoveSpeed * MoveSpeedSlowModifier * MoveSpeedBuffModifier;

            if (LookDirectionBasedOnMovement)
            {
                LookingToward = (Vector2)transform.position + MovementVector;
            }
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }


        MoveSpeedSlowModifier = 1;
        MoveSpeedBuffModifier = 1;


        int LookDirection = 0;
        //looking farther in x direction
        if (Mathf.Abs(MovementVector.x) > Mathf.Abs(MovementVector.y))
        {

            if (MovementVector.x > 0) LookDirection = 0;
            else LookDirection = 180;

        }
        //looking farther in y direction
        else
        {

            if (MovementVector.y > 0) LookDirection = 90;
            else LookDirection = 270;
        }

        if(LookDirection != LastDirection){
            LookInDirection(LookDirection);
        }
        LastDirection = LookDirection;



    }


}
