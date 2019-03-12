using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanLegsManager : MonoBehaviour
{
    public enum AnimStates {RunRight, RunRightBack, RunUp, RunUpBack, RunLeft, RunLeftBack, RunDown, RunDownBack, IdleRight, IdleUp, IdleLeft, IdleDown};

    public AnimStates CurrState = AnimStates.RunRight;

    public Sprite[] Sprites;
    public Color BaseColor;
    public Color DarkerColor;

    public SpriteRenderer FrontUpper;
    public SpriteRenderer FrontLower;
    public SpriteRenderer BackUpper;
    public SpriteRenderer BackLower;


    Animator anim;

    public string CurrentlyActiveAnim = "";

    void Start()
    {
        anim = GetComponent<Animator>();
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

    public void SetSprites_side(){

        if(!AreCurrentSpritesSide){
            AreCurrentSpritesSide = true;

            BackUpper.sprite = FrontUpper.sprite = Sprites[1];
            BackLower.sprite = FrontLower.sprite = Sprites[3];

            BackUpper.color = BackLower.color = DarkerColor;
            BackUpper.flipX = BackLower.flipX = false;

        }

    }

    public void SetSprites_front(){
        if (AreCurrentSpritesSide)
        {
            AreCurrentSpritesSide = false;

            BackUpper.sprite = FrontUpper.sprite = Sprites[0];
            BackLower.sprite = FrontLower.sprite = Sprites[2];

            BackUpper.color = BackLower.color = BaseColor;
            BackUpper.flipX = BackLower.flipX = true;

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
        SetSprites_front();

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
        RequestIdleDown();
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
