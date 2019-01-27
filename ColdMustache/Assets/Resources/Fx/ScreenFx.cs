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
}
