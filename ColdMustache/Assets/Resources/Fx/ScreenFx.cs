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
    static SpriteRenderer DeathText;

    private void Start()
    {
        DeathText = DeathTextPublic;
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

            DeathText.color = new Color(1, 1, 1, 1);

            GameObject BlackOverlay = new GameObject();
            BlackOverlay.transform.parent = UniversalReference.MainCamera.transform;
            BlackOverlay.transform.localScale = new Vector3(1000, 1000, 1);
            BlackOverlay.transform.localPosition = new Vector3(0, 0, 29);
            BlackOverlay.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("pixel");
            BlackOverlay.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
        }
    }
}
