using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimation_codefed : MonoBehaviour
{
    //this simple animation doesnt load sprites via string, but directly as an array (likely done via other script, hence the name "Code-Fed")
    public float Duration = 1;

    float Age = 0;

    public Sprite[] sprites;

    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Age += Time.deltaTime;
        if (Age > Duration)
        {
            Age = 0;
        }

        sr.sprite = sprites[Mathf.RoundToInt((Age / Duration) * (sprites.Length - 1))];
    }
}
