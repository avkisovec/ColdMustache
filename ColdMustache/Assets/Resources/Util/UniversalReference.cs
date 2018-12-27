using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniversalReference : MonoBehaviour {

    // PART OF A SCRIPTHOLDER

    //hopefully faster using this than Gameobject.find().getcomponent<>() every time

    //player
    public static GameObject PlayerObject;
    public static Entity PlayerEntity;
    public static Player PlayerScript;
    public static Rigidbody2D PlayerRb;
    public static SpriteManagerBuilder SpriteManagerBuilder;

    public static GunRotatorHand GunRotator;

    public static Transform PlayerBulletsOrigin;

    public static Inventory PlayerInventory;

    //gui
    public static Image AmmoCounter;
    public static Image AmmoStatus;
    public static Image WeaponStatus;

    //camera cursor, crosshair
    public static Camera MainCamera;
    public static CamControlPixelPerfect camControlPixelPerfect;

    public static Vector2 MouseWorldPos;

    public static Crosshair crosshair;







    // Use this for initialization
    void Start () {

        //player
        PlayerObject = GameObject.Find("PlayerContainer");
        PlayerEntity = PlayerObject.GetComponent<Entity>();
        PlayerScript = PlayerObject.GetComponent<Player>();
        PlayerRb = PlayerObject.GetComponent<Rigidbody2D>();
        SpriteManagerBuilder = GameObject.Find("PlayerContainer/Body").GetComponent<SpriteManagerBuilder>();

        GunRotator = GameObject.Find("PlayerContainer/GunContainer").GetComponent<GunRotatorHand>();

        PlayerBulletsOrigin = GameObject.Find("PlayerContainer/GunContainer/BulletsOrigin").transform;

        PlayerInventory = GameObject.Find("UNCHANGING/Inventory").GetComponent<Inventory>();

        //gui
        AmmoCounter = GameObject.Find("Canvas/AmmoCounter").GetComponent<Image>();
        AmmoStatus = GameObject.Find("Canvas/AmmoStatus").GetComponent<Image>();
        WeaponStatus = GameObject.Find("Canvas/WeaponStatus").GetComponent<Image>();

        //camera cursor, crosshair
        MainCamera = Camera.main;
        camControlPixelPerfect = MainCamera.GetComponent<CamControlPixelPerfect>();

        MouseWorldPos = MainCamera.ScreenToWorldPoint(Input.mousePosition);

        crosshair = GameObject.Find("Crosshair").GetComponent<Crosshair>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
