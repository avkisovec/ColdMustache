using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReference : MonoBehaviour {

    //hopefully faster using this than Gameobject.find().getcomponent<>() every time

    public static GameObject PlayerObject;

    public static Player PlayerScript;

    public static GunRotator GunRotator;

    public static SpriteManagerBuilder SpriteManagerBuilder;

}
