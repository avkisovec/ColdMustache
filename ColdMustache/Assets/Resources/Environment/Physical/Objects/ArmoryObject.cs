using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoryObject : MonoBehaviour
{
    public Sprite Default;
    public Sprite Hover;

    SpriteRenderer sr;

    void Start(){
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (MouseWorldPos.x > transform.position.x - (transform.lossyScale.x / 2) &&
            MouseWorldPos.x < transform.position.x + (transform.lossyScale.x / 2) &&
            MouseWorldPos.y > transform.position.y - (transform.lossyScale.y / 2) &&
            MouseWorldPos.y < transform.position.y + (transform.lossyScale.y / 2)
            )
        {
            sr.sprite = Hover;
            ContextInfo.RequestStatic("Armory.\nChange your equipment here.");

            if(Input.GetKeyUp(KeyCode.E)){
                Window w = UniversalReference.Armory.transform.parent.GetComponent<Window>();
                if(!w.AmIActive){
                    w.UnHide();
                    UniversalReference.Armory.ReloadArmory();
                }
            }
        }
        else{
            sr.sprite = Default;
        }
    }
}
