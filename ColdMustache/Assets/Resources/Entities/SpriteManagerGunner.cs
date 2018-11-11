using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManagerGunner : SpriteManager {
    

    public Transform GunContainer;
    public SpriteRenderer GunSpriteRenderer;
    
    Vector3 GunFrontPos;
    Vector3 GunBackPos;
    Vector3 GunSidePos; //default = toward right
    Vector3 GunOtherSidePos; //mirrorred = toward left

    public bool ActivelyAiming = false; //if not - just hold the gun in a relaxed way
    
    // Use this for initialization
    void Start()
    {
        StartOriginal();

        GunFrontPos = new Vector3(GunContainer.localPosition.x, GunContainer.localPosition.y - 0.1f, GunContainer.localPosition.z);
        GunBackPos = new Vector3(GunContainer.localPosition.x, GunContainer.localPosition.y + 0.3f, -GunContainer.localPosition.z);
        GunSidePos = new Vector3(GunContainer.localPosition.x + 0.2f, GunContainer.localPosition.y, GunContainer.localPosition.z);
        GunOtherSidePos = new Vector3(GunContainer.localPosition.x - 0.2f, GunContainer.localPosition.y, GunContainer.localPosition.z);
    }

    private void Update()
    {
        UpdateOriginal();

        if (ActivelyAiming)
        {
            AimGun(entity.LookingToward);
        }
        else
        {
            HoldGun(entity.LookingToward);
        }
    }

    public void AimGun(Vector3 Target)
    {
        Vector2 Delta = (Vector2)Target - (Vector2)transform.position;

        //if target is farther "up/down" than "left/right", choose between up/down
        if (Mathf.Abs(Delta.y) > Mathf.Abs(Delta.x))
        {
            spriteRenderer.flipX = false;
            //up (back is visible)
            if (Delta.y > 0)
            {
                GunContainer.localPosition = GunBackPos;
            }
            //down (front is visible)
            else
            {
                GunContainer.localPosition = GunFrontPos;
            }
        }
        //target is on one of your sides - choose between left/right
        else
        {
            //side (toward right = dafault)
            if (Delta.x > 0)
            {
                GunContainer.localPosition = GunSidePos;
            }
            //other side (toward left - needs mirrorring)
            else
            {
                GunContainer.localPosition = GunOtherSidePos;
            }
        }
        Util.RotateTransformToward(GunContainer, Target);
        if(Delta.x > 0)
        {
            GunSpriteRenderer.flipY = false;
        }
        else
        {
            GunSpriteRenderer.flipY = true;
        }
    }
    
    public void HoldGun(Vector3 Target)
    {
        Vector2 Delta = (Vector2)Target - (Vector2)transform.position;

        //if target is farther "up/down" than "left/right", choose between up/down
        if (Mathf.Abs(Delta.y) > Mathf.Abs(Delta.x))
        {
            spriteRenderer.flipX = false;
            //up (back is visible)
            if (Delta.y > 0)
            {
                GunContainer.localPosition = GunBackPos;
                GunSpriteRenderer.flipY = true;
                GunContainer.localRotation = Quaternion.Euler(0, 0, 225);
            }
            //down (front is visible)
            else
            {
                GunContainer.localPosition = GunFrontPos;
                GunSpriteRenderer.flipY = false;
                GunContainer.localRotation = Quaternion.Euler(0, 0, 315);

            }
        }
        //target is on one of your sides - choose between left/right
        else
        {
            //side (toward right = dafault)
            if (Delta.x > 0)
            {
                GunContainer.localPosition = GunSidePos;
                GunSpriteRenderer.flipY = false;
                GunContainer.localRotation = Quaternion.Euler(0, 0, 290); //orig = 315
            }
            //other side (toward left - needs mirrorring)
            else
            {
                GunContainer.localPosition = GunOtherSidePos;
                GunSpriteRenderer.flipY = true;
                GunContainer.localRotation = Quaternion.Euler(0, 0, 250); //orig = 225
            }
        }
    }
}
