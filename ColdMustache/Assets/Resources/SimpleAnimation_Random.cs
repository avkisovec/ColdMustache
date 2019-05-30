using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimation_Random : MonoBehaviour
{
    /*
    
        just like normal SimpleAnimation

        but this one can have random duration (out of range)
        and start at a random percentage (ratio) of the range
    
    
    
     */

    public float MinDuration = 0.5f;

    public float MaxDuration = 2f;

    public float MinPercentageRatioStart = 0;

    public float MaxPercentageRatioEnd = 1;

    float Duration = 1;

    float Age = 0;

    public string SpritesPath;

    Sprite[] sprites;

    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        Duration = Random.Range((float)MinDuration, (float)MaxDuration);
        Age = Random.Range(MinPercentageRatioStart, MaxPercentageRatioEnd) * Duration;

        sprites = Resources.LoadAll<Sprite>(SpritesPath);
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

        sr.sprite = sprites[Mathf.RoundToInt(Age / Duration * (sprites.Length - 1))];

    }
}
