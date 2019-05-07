using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintGun : Weapon
{
    //general things - ammo, cooldown, reload, range


    public float BaseCooldownBetweenShots = 0.1f;
    public float CurrCooldownBetweenShots = 0;

    public float EffectiveRange = 10;




    //visual things - sprites, icons, colors

    public Sprite sprite = null;

    public Sprite ColorSprite = null;

    public Color BulletColorStart = new Color(1f, 1f, 0.5f, 1);
    public Color BulletColorEnd = new Color(0.5f, 0, 0, 1);

    public string AmmoSpriteSheetPath = "Gui/Hud/Ammo/Bullets10";
    public Sprite[] AmmoSpriteSheet;

    public string StatusFullPath = "Gui/Hud/WeaponStatus/ModeFull";
    public Sprite StatusFullSprite;
    public string StatusSemiPath = "Gui/Hud/WeaponStatus/ModeSemi";
    public Sprite StatusSemiSprite;

    public string MuzzleFlashSpritePath = "Fx/SmgMuzzleFlash";

    //alt fire

    public enum SmgModes { Semi, Full }
    public SmgModes Mode = SmgModes.Full;

    //backend stuff

    float SpecialValueThatIndicatesWeaponHasJustBeenReloaded = -999999;

    public int FramesSincePlayerRequestedShooting = 0; //for the purposes of semi-auto
    public int FramesSincePlayerRequestedAltFire = 0;


    //special mechanics

    Color ActiveColor;

    SpriteRenderer WeaponSr;
    public SpriteRenderer ColorSr;

    Color[] Colors = new Color[]{Color.white, Color.red, Color.yellow, Color.green, Color.cyan, Color.blue, Color.magenta, Color.black};

    int ActiveColorPointer = 0;


    public override void OnBecomingActive()
    {
        ActiveColor = Colors[0];

        //sprites
        UniversalReference.GunRotator.GunSpriteRenderer.sprite = sprite;

        GameObject GoOverlay = new GameObject();
        GoOverlay.transform.parent = UniversalReference.GunRotator.GunSpriteRenderer.transform;
        GoOverlay.transform.localPosition = new Vector3(0,0,-0.01f);
        GoOverlay.transform.localScale = new Vector3(1,1,1);
        GoOverlay.transform.localRotation = Quaternion.Euler(0,0,0);

        ColorSr = GoOverlay.AddComponent<SpriteRenderer>();
        ColorSr.sprite = ColorSprite;
        UpdateColor();



        //ammo
        AmmoSpriteSheet = null;

        DisplayAmmo();

        //status
        StatusFullSprite = null;
        StatusSemiSprite = null;

        AmmoStatus.SetEmptySpriteAndNormalColor();

        DisplayCorrectStatusImage();

        //crosshair
        UniversalReference.crosshair.EffectiveRange = EffectiveRange;
    }

    private void Update()
    {
        if (CurrentlyActive)
        {

            if (CurrCooldownBetweenShots > 0)
            {
                CurrCooldownBetweenShots -= Time.deltaTime;
            }

            if (FramesSincePlayerRequestedShooting < 1000)
            {
                FramesSincePlayerRequestedShooting++;
            }
            if (FramesSincePlayerRequestedAltFire < 1000)
            {
                FramesSincePlayerRequestedAltFire++;
            }

            if(CheatManager.LastCheat != ""){

                string[] data = CheatManager.LastCheat.Split(' ');

                if(data.Length == 2){

                    if(data[0] == "CLR"){
                        ActiveColor = HexColorToUnityColor(data[1]);
                        //Debug.Log("in:" + data[1] + ", out:" + ActiveColor);
                        UpdateColor();

                    }

                }

            }


        }
    }

    public override void TryShooting(Vector3 Target)
    {
        ParticleSpray();

        Vector2Int Coordinates = Util.Vector3To2Int(UniversalReference.MouseWorldPos);

        if(NavTestStatic.WallTransformsArray[Coordinates.x, Coordinates.y] == null) return;

        //to prevent index out of range
        if(Coordinates.x >= 2){

            //check if this wall tile is not hidden by front (if it has exactly one tile below)
            //if yes, move coordinates to refer to front
            if(NavTestStatic.WallTransformsArray[Coordinates.x, Coordinates.y-1] != null
            &&NavTestStatic.WallTransformsArray[Coordinates.x, Coordinates.y-2] == null){
                Coordinates = new Vector2Int(Coordinates.x, Coordinates.y-1);
            }

        }

        Transform tr = NavTestStatic.WallTransformsArray[Coordinates.x, Coordinates.y];

        tr.GetComponent<SpriteRenderer>().color = ActiveColor;

        FramesSincePlayerRequestedShooting = 0;
        DisplayAmmo();
    }


    void UpdateColor(){

        ColorSr.color = ActiveColor;

    }


    public override void TryAltFire()
    {
        if(FramesSincePlayerRequestedAltFire >= 2){
            ActiveColorPointer++;
            ActiveColorPointer = ActiveColorPointer % Colors.Length;
            ActiveColor = Colors[ActiveColorPointer];
            UpdateColor();
        }

        FramesSincePlayerRequestedAltFire=0;

    }

    public override void ForceReload()
    {
        DisplayAmmo();

    }

    public void DisplayCorrectStatusImage()
    {
        UniversalReference.WeaponStatus.sprite = null;

    }

    public void DisplayAmmo()
    {
        UniversalReference.AmmoCounter.sprite = null;
    }

    public void ParticleSpray(){

        Color EndColor = new Color(ActiveColor.r, ActiveColor.g, ActiveColor.b, 0);

        Vector3 Origin = new Vector3(UniversalReference.PlayerBulletsOrigin.position.x, UniversalReference.PlayerBulletsOrigin.position.y, -31);

        Vector2 BaseVector = Util.GetDirectionVectorToward(Origin, UniversalReference.MouseWorldPos);

        BaseVector /= 20;
        for(int i = 0; i < 10; i++){

            GameObject go = new GameObject();
            go.AddComponent<Particle>();
            go.transform.position = Origin;
            Particle p = go.GetComponent<Particle>();
            p.UseShifts = true;
            p.LeaveParent = true;
            p.StartAt00 = false;
            p.sprite = Resources.Load<Sprite>("pixel");
            p.Lifespan = (int)Mathf.Pow(Random.Range(3, 7), 2);
            p.StartingScale = 0.05f;
            p.EndingScale = 0.3f;

            

            p.StartingColor = ActiveColor;
            p.EndingColor = EndColor;


            p.StartingHorizontalWind = BaseVector.x + Random.Range(-0.01f, 0.01f);
            p.StartingVerticalWind = BaseVector.y + Random.Range(-0.01f, 0.01f);
            p.EndingHorizontalWind = BaseVector.x + Random.Range(-0.01f, 0.01f);
            p.EndingVerticalWind = BaseVector.y + Random.Range(-0.01f, 0.01f);

        }

    }

    //without # on the start, just FF0000 (no alpha either)
    public Color HexColorToUnityColor(string Hex){

        if(Hex.Length != 6) return Color.white;

        float r = 16 * HexCharToNumber(Hex[0]) + HexCharToNumber(Hex[1]);
        float g = 16 * HexCharToNumber(Hex[2]) + HexCharToNumber(Hex[3]);
        float b = 16 * HexCharToNumber(Hex[4]) + HexCharToNumber(Hex[5]);

        return new Color(r/256, g/256, b/256, 1);

    }

    //only uppercase please
    public int HexCharToNumber(char c){
    
        /*

        ascii values:

        48 = '0'
        57 = '9'
        65 = 'A'
        70 = 'F'

        */

        if(c >= 48 && c <= 57) return (int)c-48;
        if(c >= 65 && c <= 70) return (int)c-55;

        return 0;
    }

}
