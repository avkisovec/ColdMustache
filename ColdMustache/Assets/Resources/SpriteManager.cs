using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour {
    
    public Sprite Front;
    public Sprite Side;
    public Sprite Back;

    public SpriteRenderer spriteRenderer;

    protected Entity entity;


    float BaseCooldownToDefaultColor = 0.1f;
    float CurrCooldownToDefaultColor = 0;

    Color DefaultColor;
    Color InjuredColor = new Color(1, 0, 0);

    // Use this for initialization
    void Start ()
    {
        StartOriginal();
    }

    //start default is what is called in the original class - every child class has its own start(), but they still need to call the original
    protected void StartOriginal()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Front;
        DefaultColor = spriteRenderer.color;
        entity = GetComponent<Entity>();
    }
	
	public void LookAt(Vector3 Target)
    {
        Vector2 Delta = (Vector2)Target - (Vector2)transform.position;

        //if target is farther "up/down" than "left/right", choose between up/down
        if (Mathf.Abs(Delta.y) > Mathf.Abs(Delta.x))
        {
            spriteRenderer.flipX = false;
            //up (back is visible)
            if (Delta.y > 0)
            {
                spriteRenderer.sprite = Back;
                
            }
            //down (front is visible)
            else
            {
                spriteRenderer.sprite = Front;

            }
        }
        //target is on one of your sides - choose between left/right
        else
        {
            //side (toward right = dafault)
            if (Delta.x > 0)
            {
                spriteRenderer.sprite = Side;
                spriteRenderer.flipX = false;
                
                
            }
            //other side (toward left - needs mirrorring)
            else
            {
                spriteRenderer.sprite = Side;
                spriteRenderer.flipX = true;
                
            }
        }
    }

    private void Update()
    {
        UpdateOriginal();
    }

    //update default is what is updated in the original class - every child  class has its own update(), but they still need to call the original
    protected void UpdateOriginal()
    {
        LookAt(entity.LookingToward);
        if (CurrCooldownToDefaultColor > 0)
        {
            CurrCooldownToDefaultColor -= Time.deltaTime;
        }
        else
        {
            spriteRenderer.color = DefaultColor;
        }
    }

    public void FlashInjuredColor()
    {
        spriteRenderer.color = InjuredColor;
        CurrCooldownToDefaultColor = BaseCooldownToDefaultColor;
    }

}
