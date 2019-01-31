using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFx : MonoBehaviour {
    
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
    
    public static void DeathScreen()
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
            container.transform.localPosition = new Vector3(-50f * 7f / 32f, 0, 27);
            container.AddComponent<StayFixedSize>().PixelScale = 2;

            string DeathText = AlphabetManager.BreakText(GetDeathText(),50);
            float X = 0;
            float Y = 0;

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
                X += (float)7 / (float)32;
            }

        }
    }

    
    public enum DamageTypes { Bullet, Claws, Blade, Explosion };
    
    public static string GetDeathText(DamageTypes DamageType = DamageTypes.Bullet)
    {
        return BulletRibcage.DoYourThing();
        /*
        string output = "";
        
        if(DamageType == DamageTypes.Bullet)
        {
            switch (Random.Range(0, 4))
            {
                
            }
            
            return output;
        }

        //defualt

        output += "As you take your last labored breath, you finally fall down, never to get up again.";
        return output;
        */
    }

    public static DeathText BulletRibcage = new DeathText();

    public static void Setup()
    {
        DeathText RibcageProjectileOptions = new DeathText();

        DeathText RibcageEntrance = new DeathText();

        DeathText RibcageExit = new DeathText();

        DeathText PreFinal = new DeathText();
        DeathText Final = new DeathText();


        BulletRibcage.Next.Add(RibcageProjectileOptions);

        RibcageProjectileOptions.Next.Add(new DeathText("As the bullet", RibcageEntrance));
        RibcageProjectileOptions.Next.Add(new DeathText("As the last bullet", RibcageEntrance));
        RibcageProjectileOptions.Next.Add(new DeathText("As the final bullet", RibcageEntrance));

        RibcageEntrance.Next.Add(new DeathText(" pierces your ribcage", RibcageExit));
        RibcageEntrance.Next.Add(new DeathText(" drills into your ribcage", RibcageExit));
        RibcageEntrance.Next.Add(new DeathText(" tears through your ribcage", RibcageExit));
        RibcageEntrance.Next.Add(new DeathText(" punches a hole in your ribcage", RibcageExit));
        RibcageEntrance.Next.Add(new DeathText(" finds its way inbetween your ribs", RibcageExit));

        RibcageExit.Next.Add(new DeathText(" and lodges itself into your spine", PreFinal));
        RibcageExit.Next.Add(new DeathText(", only to leave a gaping hole on the way out", PreFinal));
        RibcageExit.Next.Add(new DeathText(" and slashes your aorta", PreFinal));
        RibcageExit.Next.Add(new DeathText(" and right thrught the heart", PreFinal));

        PreFinal.Next.Add(new DeathText(", you finally fall down", Final));
        PreFinal.Next.Add(new DeathText(", you stumble to the ground", Final));
        PreFinal.Next.Add(new DeathText(", you fall to your knees", Final));

        Final.Next.Add(new DeathText(", realizing it's the last thing you'll ever do."));
        Final.Next.Add(new DeathText(", never to get up again."));
        Final.Next.Add(new DeathText(", awaiting your final moment."));

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
