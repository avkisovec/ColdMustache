using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoStatus : MonoBehaviour {

    public static Image ammoStatusImage;

    //static ones which will be used
    public static Sprite IconReloading;
    public static Sprite IconChambering;
    public static Sprite IconEmptyMag;
    public static Sprite Empty;
    public static Image AmmoCounter;
    public static SpriteRenderer CrosshairOverlay;

    //assigned via editor, then passed to the static ones
    public Sprite IconReloading_;
    public Sprite IconChambering_;
    public Sprite IconEmptyMag_;
    public Sprite Empty_;
    public Image AmmoCounter_;
    public SpriteRenderer CrosshairOverlay_;

    public bool HasBeenSetup = false;
    void Start () {
        if (HasBeenSetup)
        {
            return;
        }
        HasBeenSetup = true;

        ammoStatusImage = this.GetComponent<Image>();
        IconReloading = IconReloading_;
        IconChambering = IconChambering_;
        IconEmptyMag = IconEmptyMag_;
        Empty = Empty_;
        AmmoCounter = AmmoCounter_;
        CrosshairOverlay = CrosshairOverlay_;
	}
	
	// Update is called once per frame
	void Update () {
        StaticUpdate();
	}

    public static float TimeTillEmpty = 0;

    public static void StaticUpdate()
    {
        if (TimeTillEmpty > 0)
        {
            TimeTillEmpty -= Time.deltaTime;
        }
        else
        {
            ammoStatusImage.sprite = Empty;
            AmmoCounter.color = new Color(1, 1, 1, 1);
            CrosshairOverlay.sprite = Empty;
        }
    }

    public static void EmptyMag()
    {
        /*
         * if TimeTillEmpty is above zero, it means weapon is currently reloading/chambering and therefore this symbol shouldnt be displayed
         * 
         * otherwise, when this method is called it will display the empty mag icon, but it will disappear in the next frame when update is called (this script is being executed before other scripts, so its update sets blank sprite but if someone later that frame reports empty mag it gets overwritten for that frame)
         * empty mag icon will only be displayed in frames where this function is called; in all other frames its empty (or reload/chamber)
         * 
         */
        if( TimeTillEmpty <= 0 ){
            ammoStatusImage.sprite = IconEmptyMag;
            CrosshairOverlay.sprite = IconEmptyMag;
        }
    }

    public static void NowReloading(float time)
    {
        TimeTillEmpty = time;
        ammoStatusImage.sprite = IconReloading;
        AmmoCounter.color = new Color(1, 1, 1, 0.5f);

        CrosshairOverlay.sprite = IconReloading;
    }
    
    public static void NowChambering(float time)
    {
        TimeTillEmpty = time;
        ammoStatusImage.sprite = IconChambering;
        AmmoCounter.color = new Color(1, 1, 1, 0.5f);

        CrosshairOverlay.sprite = IconChambering;
    }

}
