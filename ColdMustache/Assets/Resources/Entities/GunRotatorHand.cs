using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotatorHand : MonoBehaviour {

    Transform GunContainer;
    public SpriteRenderer GunSpriteRenderer;

    Vector3 GunFrontPos;
    Vector3 GunBackPos;
    Vector3 GunSidePos; //default = toward right
    Vector3 GunOtherSidePos; //mirrorred = toward left

    //right on screen, not the actual character's right shoulder (if character is looking toward screen, his left shoulder is on right)
    Vector3 FrontBackLeftShoulder = new Vector3(-0.18f, 0.05f);
    Vector3 FrontBackRightShoulder = new Vector3(0.18f, 0.05f);
    Vector3 SideShoulder = new Vector3(0, 0.05f);

    //public Transform Head;
    public Transform OtherHand;
    public Transform HoldPoint;

    public SpriteRenderer OtherHandSr;
    public SpriteRenderer HandSr;

    float VisibleDepth = -0.01f; //when in front of body
    float ObscuredDepth = 0.01f; //when behind the body


    public Entity entity;

    public bool ActivelyAiming = false; //if not - just hold the gun in a relaxed way

    void Start()
    {

        GunContainer = transform;

        GunFrontPos = new Vector3(GunContainer.localPosition.x, GunContainer.localPosition.y - 0.1f, GunContainer.localPosition.z);
        GunBackPos = new Vector3(GunContainer.localPosition.x, GunContainer.localPosition.y + 0.3f, -GunContainer.localPosition.z);
        GunSidePos = new Vector3(GunContainer.localPosition.x + 0.2f, GunContainer.localPosition.y, GunContainer.localPosition.z);
        GunOtherSidePos = new Vector3(GunContainer.localPosition.x - 0.2f, GunContainer.localPosition.y, GunContainer.localPosition.z);

        if (ActivelyAiming)
        {
            AimGun(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else
        {
            HoldGun(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (ActivelyAiming)
        {
            AimGun(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else
        {
            HoldGun(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

    }

    public void AimGun(Vector3 Target)
    {

        Vector2 Delta = (Vector2)Target - (Vector2)transform.parent.position;

        //Target = Target / Target.magnitude;

        //if target is farther "up/down" than "left/right", choose between up/down
        if (Mathf.Abs(Delta.y) > Mathf.Abs(Delta.x))
        {
            //up (back is visible)
            if (Delta.y > 0)
            {
                if (Delta.x > 0)
                {
                    GunContainer.localPosition = new Vector3(FrontBackRightShoulder.x, FrontBackRightShoulder.y, ObscuredDepth);
                    transform.localScale = new Vector3(1, 1, 1);
                }
                //other side (toward left - needs mirrorring)
                else
                {
                    GunContainer.localPosition = new Vector3(FrontBackLeftShoulder.x, FrontBackLeftShoulder.y, ObscuredDepth);
                    transform.localScale = new Vector3(1, -1, 1);
                }
                OtherHand.localPosition = new Vector3(0, 0, 999999);
            }
            //down (front is visible)
            else
            {
                if (Delta.x > 0)
                {
                    GunContainer.localPosition = new Vector3(FrontBackLeftShoulder.x, FrontBackLeftShoulder.y, VisibleDepth);
                    transform.localScale = new Vector3(1, 1, 1);

                    OtherHand.localPosition = new Vector3(FrontBackRightShoulder.x, FrontBackRightShoulder.y, VisibleDepth/2);
                    Util.RotateTransformToward(OtherHand, HoldPoint);
                    OtherHandSr.flipY = false;
                }
                //other side (toward left - needs mirrorring)
                else
                {
                    GunContainer.localPosition = new Vector3(FrontBackRightShoulder.x, FrontBackRightShoulder.y, VisibleDepth);
                    transform.localScale = new Vector3(1, -1, 1);

                    OtherHand.localPosition = new Vector3(FrontBackLeftShoulder.x, FrontBackLeftShoulder.y, VisibleDepth/2);
                    Util.RotateTransformToward(OtherHand, HoldPoint);
                    OtherHandSr.flipY = true;
                }
            }

            //Head.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
        }
        //target is on one of your sides - choose between left/right
        else
        {
            //side (toward right = dafault)
            if (Delta.x > 0)
            {
                GunContainer.localPosition = new Vector3(SideShoulder.x, SideShoulder.y, VisibleDepth);
                transform.localScale = new Vector3(1, 1, 1);
            }
            //other side (toward left - needs mirrorring)
            else
            {
                GunContainer.localPosition = new Vector3(SideShoulder.x, SideShoulder.y, VisibleDepth);
                transform.localScale = new Vector3(1, -1, 1);
            }

            //Util.RotateTransformToward(Head, Camera.main.ScreenToWorldPoint(Input.mousePosition));

            OtherHand.localPosition = new Vector3(0, 0, 999999);
        }
        Util.RotateTransformToward(GunContainer, Target);
    }

    public void HoldGun(Vector3 Target)
    {
        Vector2 Delta = (Vector2)Target - (Vector2)transform.parent.position;

        //if target is farther "up/down" than "left/right", choose between up/down
        if (Mathf.Abs(Delta.y) > Mathf.Abs(Delta.x))
        {
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
