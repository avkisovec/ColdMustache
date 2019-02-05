using System.Collections.Generic;
using UnityEngine;

public class ScreenFx : MonoBehaviour {

    private void Awake()
    {
        HaveDeathFxBeenSpawned = false;
    }

    public static void InjuryScreen()
    {
        GameObject Overlay = new GameObject();
        Overlay.transform.parent = UniversalReference.MainCamera.transform;
        Overlay.transform.localScale = new Vector3(1000, 1000, 1);
        Overlay.transform.localPosition = new Vector3(0, 0, 30);
        Overlay.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("pixel");
        Overlay.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.8f);
        Overlay.AddComponent<SlowlyFadeAway>().Duration = 0.4f;
    }

    static bool HaveDeathFxBeenSpawned = false;

    public SpriteRenderer DeathTextPublic;
    static SpriteRenderer DeathImage;

    private void Start()
    {
        DeathImage = DeathTextPublic;
        Setup();
    }
    
    public static void DeathScreen(DamagerInflicter.WeaponTypes WeaponType = DamagerInflicter.WeaponTypes.Undefined)
    {
        if (!HaveDeathFxBeenSpawned)
        {
            HaveDeathFxBeenSpawned = true;

            GameObject RedOverlay = new GameObject();
            RedOverlay.transform.parent = UniversalReference.MainCamera.transform;
            RedOverlay.transform.localScale = new Vector3(1000, 1000, 1);
            RedOverlay.transform.localPosition = new Vector3(0, 0, 28);
            RedOverlay.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("pixel");
            RedOverlay.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
            RedOverlay.AddComponent<SlowlyFadeAway>().Duration = 1.5f;

            DeathImage.color = new Color(1, 1, 1, 1);

            GameObject BlackOverlay = new GameObject();
            BlackOverlay.transform.parent = UniversalReference.MainCamera.transform;
            BlackOverlay.transform.localScale = new Vector3(1000, 1000, 1);
            BlackOverlay.transform.localPosition = new Vector3(0, 0, 29);
            BlackOverlay.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("pixel");
            BlackOverlay.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
            
            GameObject container = new GameObject();
            container.transform.parent = UniversalReference.MainCamera.transform;
            container.transform.localPosition = new Vector3(-40f * 7f / 32f / UniversalReference.camControlPixelPerfect.ActualTileScale, 2 / UniversalReference.camControlPixelPerfect.ActualTileScale, 27);
            container.AddComponent<StayFixedSize>().PixelScale = 2;

            string DeathText = AlphabetManager.BreakText(GetDeathText(WeaponType),40);
            float X = 0;
            float Y = 0;

            Color Invisible = new Color(1f, 0.2f, 0.2f, 0f);
            Color Dark = new Color(1f,0.2f,0.2f,0.4f);
            Color Edge = new Color(1,0.2f,0.2f,1);
            Color Final = new Color(0.9f, 0.9f, 0.9f, 1);
            
            for (int i = 0; i < DeathText.Length; i++)
            {
                if (DeathText[i] == '\n')
                {
                    X = 0;
                    Y -= (float)11 / (float)32;
                    continue;
                }
                GameObject go = new GameObject();
                go.transform.parent = container.transform;
                go.transform.localPosition = new Vector3(X,Y,0);
                go.AddComponent<SpriteRenderer>().sprite = AlphabetManager.GetSprite(DeathText[i]);

                go.GetComponent<SpriteRenderer>().color = Dark;
                TextEffect_ColorWave tecw = go.AddComponent<TextEffect_ColorWave>();
                tecw.Phases = 3;
                tecw.DelayedStart = new[] {(X-3f*Y)/6, 0f,0f};
                tecw.StartingColor = new[] {Invisible, Dark, Edge};
                tecw.EndingColor = new[] {Dark, Edge, Final,};
                tecw.Lifespan = new[] {0.6f, 0.4f, 0.2f,};

                X += (float)7 / (float)32;
            }


            string RestartText = "Press F1 to restart.";
            X = 0;
            Y = -2;
            
            for (int i = 0; i < RestartText.Length; i++)
            {
                if (RestartText[i] == '\n')
                {
                    X = 0;
                    Y -= (float)11 / (float)32;
                    continue;
                }
                GameObject go = new GameObject();
                go.transform.parent = container.transform;
                go.transform.localPosition = new Vector3(X, Y, 0);
                go.AddComponent<SpriteRenderer>().sprite = AlphabetManager.GetSprite(RestartText[i]);

                go.GetComponent<SpriteRenderer>().color = Dark;
                TextEffect_ColorWave tecw = go.AddComponent<TextEffect_ColorWave>();
                tecw.Phases = 1;
                tecw.DelayedStart = new[] {4f};
                tecw.StartingColor = new[] {Invisible};
                tecw.EndingColor = new[] {new Color(0.5f,0.5f,0.5f,1)};
                tecw.Lifespan = new[] { 1f};

                X += (float)7 / (float)32;
            }


        }
    }
        
    public static string GetDeathText(DamagerInflicter.WeaponTypes WeaponType = DamagerInflicter.WeaponTypes.Undefined)
    {
        switch (WeaponType)
        {
            case DamagerInflicter.WeaponTypes.Bullet:
                return Start_Bullet.DoYourThing();
            case DamagerInflicter.WeaponTypes.Claw:
                return Start_Claws.DoYourThing();
            case DamagerInflicter.WeaponTypes.Blade:
                return Start_Blade.DoYourThing();
            case DamagerInflicter.WeaponTypes.Explosion:
                return Start_Explosion.DoYourThing();
        }
        //default option
        return Start_Undefined.DoYourThing();
    }

    public static DeathText Start_Bullet = new DeathText();
    public static DeathText Start_Claws = new DeathText();
    public static DeathText Start_Blade = new DeathText();
    public static DeathText Start_Explosion = new DeathText();
    public static DeathText Start_Undefined = new DeathText();

    static bool HasBeenSetup = false;
    public static void Setup()
    {
        if (HasBeenSetup)
        {
            return;
        }
        HasBeenSetup = true;

        //bullet
        DeathText BulletRibcage = new DeathText();
        DeathText BulletRibcageProjectileOptions = new DeathText();
        DeathText BulletRibcageEntrance = new DeathText();
        DeathText BulletRibcageExit = new DeathText();

        //claws
        DeathText ClawnOptions = new DeathText();

        //blade
        DeathText BladeOptions = new DeathText();

        //sharp melee (claws+blades)
        DeathText SharpMelee = new DeathText();

        //explosion

        //undefined

        //shared
        DeathText PreFinal = new DeathText();
        DeathText Final = new DeathText();

        //bullet
        Start_Bullet.Next.Add(BulletRibcage);

        BulletRibcage.Next.Add(BulletRibcageProjectileOptions);

        BulletRibcageProjectileOptions.Next.Add(new DeathText("As the bullet", BulletRibcageEntrance));
        BulletRibcageProjectileOptions.Next.Add(new DeathText("As the last bullet", BulletRibcageEntrance));
        BulletRibcageProjectileOptions.Next.Add(new DeathText("As the final bullet", BulletRibcageEntrance));

        BulletRibcageEntrance.Next.Add(new DeathText(" pierces your ribcage", BulletRibcageExit));
        BulletRibcageEntrance.Next.Add(new DeathText(" drills into your chest", BulletRibcageExit));
        BulletRibcageEntrance.Next.Add(new DeathText(" tears through your chest", BulletRibcageExit));
        BulletRibcageEntrance.Next.Add(new DeathText(" punches a hole in your ribcage", BulletRibcageExit));
        BulletRibcageEntrance.Next.Add(new DeathText(" finds its way inbetween your ribs", BulletRibcageExit));
        BulletRibcageEntrance.Next.Add(new DeathText(" penetrates your ribcage", BulletRibcageExit));
        BulletRibcageEntrance.Next.Add(new DeathText(" burrows itself into your chest", BulletRibcageExit));

        BulletRibcageExit.Next.Add(new DeathText(" and lodges itself into your spine", PreFinal));
        BulletRibcageExit.Next.Add(new DeathText(", only to leave a gaping hole on the way out", PreFinal));
        BulletRibcageExit.Next.Add(new DeathText(" and slashes your aorta", PreFinal));
        BulletRibcageExit.Next.Add(new DeathText(" and tears your aorta open", PreFinal));
        BulletRibcageExit.Next.Add(new DeathText(" and tears your heart open", PreFinal));
        BulletRibcageExit.Next.Add(new DeathText(" and tears right through the heart", PreFinal));


        //claws
        Start_Claws.Next.Add(new DeathText("As the claws", SharpMelee));
        Start_Claws.Next.Add(new DeathText("As the neddle-like claws", SharpMelee));
        Start_Claws.Next.Add(new DeathText("As the jagged claws", SharpMelee));
        Start_Claws.Next.Add(new DeathText("As the teeth", SharpMelee));
        Start_Claws.Next.Add(new DeathText("As the neddle-like teeth", SharpMelee));
        Start_Claws.Next.Add(new DeathText("As the jagged teeth", SharpMelee));

        //blades
        Start_Blade.Next.Add(new DeathText("As the blade", SharpMelee));
        Start_Blade.Next.Add(new DeathText("As the cold steel", SharpMelee));
        Start_Blade.Next.Add(new DeathText("As the hardened steel", SharpMelee));
        Start_Blade.Next.Add(new DeathText("As the sharp blade", SharpMelee));
        Start_Blade.Next.Add(new DeathText("As the rusty blade", SharpMelee));

        //sharp melee - claws + blades
        SharpMelee.Next.Add(new DeathText(" dig into your flesh",PreFinal));
        SharpMelee.Next.Add(new DeathText(" rend your flesh", PreFinal));
        SharpMelee.Next.Add(new DeathText(" slash open your neck", PreFinal));
        SharpMelee.Next.Add(new DeathText(" slash open your jugular", PreFinal));
        SharpMelee.Next.Add(new DeathText(" tear through your ribs", PreFinal));
        SharpMelee.Next.Add(new DeathText(" tear through your guts", PreFinal));
        SharpMelee.Next.Add(new DeathText(" dig into your face", PreFinal));
        SharpMelee.Next.Add(new DeathText(" spill out your guts", PreFinal));
        SharpMelee.Next.Add(new DeathText(" spill out your innards", PreFinal));

        //explosion
        Start_Explosion.Next.Add(new DeathText("As the pressure wave crushes your lungs", PreFinal));
        Start_Explosion.Next.Add(new DeathText("As the shock wave crushes your lungs", PreFinal));
        Start_Explosion.Next.Add(new DeathText("As the explosion tears your limbs apart", PreFinal));
        Start_Explosion.Next.Add(new DeathText("As the shrapnels dig into your face", PreFinal));
        Start_Explosion.Next.Add(new DeathText("As the shrapnels dig into your chest", PreFinal));
        Start_Explosion.Next.Add(new DeathText("As the shrapnels dig into your stomach", PreFinal));

        //undefined
        Start_Undefined.Next.Add(new DeathText("As you take your last labored breath", PreFinal));
        Start_Undefined.Next.Add(new DeathText("As you take your last dying breath", PreFinal));

        //shared
        PreFinal.Next.Add(new DeathText(", you finally fall down", Final));
        PreFinal.Next.Add(new DeathText(", you stumble to the ground", Final));
        PreFinal.Next.Add(new DeathText(", you fall to your knees", Final));
        PreFinal.Next.Add(new DeathText(", you fall to the ground", Final));
        PreFinal.Next.Add(new DeathText(", you land in a puddle of your blood", Final));

        Final.Next.Add(new DeathText(", realizing it's the last thing you'll ever do."));
        Final.Next.Add(new DeathText(", never to get up again."));
        Final.Next.Add(new DeathText(", awaiting your final moment."));
        Final.Next.Add(new DeathText(", knowing your time has come."));
        Final.Next.Add(new DeathText(", and there's nothing you can do about it."));
        Final.Next.Add(new DeathText(", and there's nothing you can do anymore."));
        Final.Next.Add(new DeathText(" without having much of a choice."));

    }
    
    public class DeathText
    {
        public string Text = "";
        public List<DeathText> Next = new List<DeathText>();
        public DeathText(string text = "", DeathText next = null)
        {
            Text = text;
            if (next != null)
            {
                this.Next.Add(next);
            }
        }
        public string DoYourThing()
        {
            if (Next.Count != 0)
            {
                return Text + Next[Random.Range((int)0, (int)Next.Count)].DoYourThing();
            }
            else
            {
                return Text;
            }
        }
    }


}
