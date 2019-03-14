using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanLegsManager : MonoBehaviour
{
    public enum AnimStates {RunRight, RunRightBack, RunUp, RunUpBack, RunLeft, RunLeftBack, RunDown, RunDownBack, IdleRight, IdleUp, IdleLeft, IdleDown};

    public AnimStates CurrState = AnimStates.RunRight;

    public SpriteManagerGeneric SpriteManagerToStealFrom;

    public SpriteRenderer FrontUpper;
    public SpriteRenderer FrontLower;
    public SpriteRenderer BackUpper;
    public SpriteRenderer BackLower;

    public SpriteRenderer FrontUpper_clothing;
    public SpriteRenderer FrontLower_clothing;
    public SpriteRenderer BackUpper_clothing;
    public SpriteRenderer BackLower_clothing;



    Animator anim;

    public string CurrentlyActiveAnim = "";

    void Start()
    {
        anim = GetComponent<Animator>();

        //the color of the front (or left from front view) doesnt cange, so i set it here
        FrontUpper.color = FrontLower.color = SpriteManagerToStealFrom.colors[0];
    }

    void Update()
    {

    }

    public void ResetTriggers(){
        anim.ResetTrigger("RunDown");
        anim.ResetTrigger("RunRight");
        anim.ResetTrigger("RunRightBack");
        anim.ResetTrigger("IdleRight");
        anim.ResetTrigger("IdleDown");
    }

    public bool AreCurrentSpritesSide = false;

    //what sprites are currently used (facing side/down/up), and also how are they colored (if they are facing side the one further back is darker...)
    public enum SpriteSet{Undefined, Side, Front, Back}

    public SpriteSet CurrSpriteSet = SpriteSet.Undefined;

    public void ResetSpriteSet(){
        if(CurrSpriteSet == SpriteSet.Side){

            //setting undefinined bcs otherwise it sees that its already set and will not resedt
            CurrSpriteSet = SpriteSet.Undefined;
            SetSprites_side();
            return;
        }
        if (CurrSpriteSet == SpriteSet.Front)
        {
            CurrSpriteSet = SpriteSet.Undefined;
            SetSprites_front();
            return;
        }
        if (CurrSpriteSet == SpriteSet.Back)
        {
            CurrSpriteSet = SpriteSet.Undefined;
            SetSprites_back();
            return;
        }
    }

    public void SetSprites_side(){

        if(CurrSpriteSet != SpriteSet.Side){
            CurrSpriteSet = SpriteSet.Side;

            BackUpper.sprite = FrontUpper.sprite = SpriteManagerToStealFrom.sprites[9][1];
            BackLower.sprite = FrontLower.sprite = SpriteManagerToStealFrom.sprites[9][4];

            //the color of the front (or left from front view) doesnt cange, so i set in start
            BackUpper.color = BackLower.color = Util.MakeColorDarker(SpriteManagerToStealFrom.colors[0], 0.75f);
            BackUpper.flipX = BackLower.flipX = false;



            BackUpper_clothing.sprite = FrontUpper_clothing.sprite = SpriteManagerToStealFrom.sprites[10][4];
            BackLower_clothing.sprite = FrontLower_clothing.sprite = SpriteManagerToStealFrom.sprites[10][7];
            FrontUpper_clothing.color = FrontLower_clothing.color = SpriteManagerToStealFrom.colors[10];
            BackUpper_clothing.color = BackLower_clothing.color = Util.MakeColorDarker(SpriteManagerToStealFrom.colors[10], 0.75f);
            BackUpper_clothing.flipX = BackLower_clothing.flipX = false;

        }

    }

    public void SetSprites_front(){

        if (CurrSpriteSet != SpriteSet.Front)
        {
            CurrSpriteSet = SpriteSet.Front;

            BackUpper.sprite = FrontUpper.sprite = SpriteManagerToStealFrom.sprites[9][0];
            BackLower.sprite = FrontLower.sprite = SpriteManagerToStealFrom.sprites[9][3];

            //the color of the front (or left from front view) doesnt cange, so i set in start
            BackUpper.color = BackLower.color = SpriteManagerToStealFrom.colors[0];
            BackUpper.flipX = BackLower.flipX = true;



            BackUpper_clothing.sprite = FrontUpper_clothing.sprite = SpriteManagerToStealFrom.sprites[10][3];
            BackLower_clothing.sprite = FrontLower_clothing.sprite = SpriteManagerToStealFrom.sprites[10][6];
            BackUpper_clothing.color = BackLower_clothing.color = FrontUpper_clothing.color = FrontLower_clothing.color = SpriteManagerToStealFrom.colors[10];
            BackUpper_clothing.flipX = BackLower_clothing.flipX = true;

        }
    }

    public void SetSprites_back()
    {
        if (CurrSpriteSet != SpriteSet.Back)
        {
            CurrSpriteSet = SpriteSet.Back;

            BackUpper.sprite = FrontUpper.sprite = SpriteManagerToStealFrom.sprites[9][2];
            BackLower.sprite = FrontLower.sprite = SpriteManagerToStealFrom.sprites[9][5];

            //the color of the front (or left from front view) doesnt cange, so i set in start
            BackUpper.color = BackLower.color = SpriteManagerToStealFrom.colors[0];
            BackUpper.flipX = BackLower.flipX = true;



            BackUpper_clothing.sprite = FrontUpper_clothing.sprite = SpriteManagerToStealFrom.sprites[10][5];
            BackLower_clothing.sprite = FrontLower_clothing.sprite = SpriteManagerToStealFrom.sprites[10][8];
            BackUpper_clothing.color = BackLower_clothing.color = FrontUpper_clothing.color = FrontLower_clothing.color = SpriteManagerToStealFrom.colors[10];
            BackUpper_clothing.flipX = BackLower_clothing.flipX = true;

        }
    }

    public void RequestRunRight(bool LookingRight = true){

        SetSprites_side();

        //normal - walking and looking right
        if(LookingRight){        
            if(CurrState!=AnimStates.RunRight){
                CurrState = AnimStates.RunRight;
                transform.localScale = new Vector3(1, 1, 1);

                ResetTriggers();
                anim.SetTrigger("RunRight");
            }
        }

        //looking the other way - flip and play backward animation
        else{
            if (CurrState != AnimStates.RunRightBack)
            {
                CurrState = AnimStates.RunRightBack;

                transform.localScale = new Vector3(-1, 1, 1);

                ResetTriggers();
                anim.SetTrigger("RunRightBack");
            }
        }
    }

    public void RequestRunLeft(bool LookingRight = true)
    {
        SetSprites_side();

        //backwards - flip x (which is flipped normally so its normal now) and play backward anim
        if(LookingRight){
            if (CurrState != AnimStates.RunLeftBack)
            {
                CurrState = AnimStates.RunLeftBack;

                transform.localScale = new Vector3(1, 1, 1);

                ResetTriggers();
                anim.SetTrigger("RunRightBack");
            }
        }

        //normal - looking and walking left
        else{
            if (CurrState != AnimStates.RunLeft)
            {
                CurrState = AnimStates.RunLeft;

                transform.localScale = new Vector3(-1,1,1);

                ResetTriggers();            
                anim.SetTrigger("RunRight");
            }
        }
    }

    public void RequestRunDown(bool LookingRight = true){
        
        SetSprites_front();        
        
        if (CurrState != AnimStates.RunDown)
        {
            CurrState = AnimStates.RunDown;
            ResetTriggers();
            anim.SetTrigger("RunDown");
        }
    }

    public void RequestRunUp(bool LookingRight = true)
    {
        SetSprites_back();

        if (CurrState != AnimStates.RunUp)
        {
            CurrState = AnimStates.RunUp;
            ResetTriggers();
            anim.SetTrigger("RunDown");
        }
    }

    public void RequestIdle(Vector2 LookingToward){

        Vector2 delta = LookingToward - (Vector2)transform.position;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0)
            {
                RequestIdleRight();
            }
            else
            {
                RequestIdleLeft();
            }
        }
        else
        {
            if (delta.y > 0)
            {
                RequestIdleUp();
            }
            else
            {
                RequestIdleDown();
            }
        }

    }

    public void RequestIdleRight()
    {
        SetSprites_side();

        if (CurrState != AnimStates.IdleRight)
        {
            CurrState = AnimStates.IdleRight;

            transform.localScale = new Vector3(1, 1, 1);

            ResetTriggers();
            anim.SetTrigger("IdleRight");
        }
    }

    public void RequestIdleLeft()
    {
        SetSprites_side();

        if (CurrState != AnimStates.IdleLeft)
        {
            CurrState = AnimStates.IdleLeft;

            transform.localScale = new Vector3(-1, 1, 1);

            ResetTriggers();
            anim.SetTrigger("IdleRight");
        }
    }

    public void RequestIdleDown(){

        SetSprites_front();

        if (CurrState != AnimStates.IdleDown)
        {
            CurrState = AnimStates.IdleDown;

            transform.localScale = new Vector3(1, 1, 1);

            ResetTriggers();
            anim.SetTrigger("IdleDown");
        }
    }

    public void RequestIdleUp()
    {
        SetSprites_back();

        if (CurrState != AnimStates.IdleDown)
        {
            CurrState = AnimStates.IdleDown;

            transform.localScale = new Vector3(1, 1, 1);

            ResetTriggers();
            anim.SetTrigger("IdleDown");
        }
    }




    public void Request(string TriggerName)
    {
        if (TriggerName != CurrentlyActiveAnim)
        {
            CurrentlyActiveAnim = TriggerName;
            anim.ResetTrigger("Sprint");        //all triggers that exist have to be reset here, otherwise they queue up and cause problems
            anim.ResetTrigger("GunWalk");
            anim.ResetTrigger("GunStop");
            anim.ResetTrigger("Dive");
            anim.ResetTrigger("Air");
            anim.ResetTrigger("ClimbStuck");
            anim.ResetTrigger("Jump");
            anim.ResetTrigger("ClimbUp");
            anim.SetTrigger(TriggerName);

        }
    }
}
