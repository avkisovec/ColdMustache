using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimation : MonoBehaviour
{

    public float Duration = 1;

    float Age = 0;

    public string SpritesPath;

    Sprite[] sprites;

    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sprites = Resources.LoadAll<Sprite>(SpritesPath);
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Age+=Time.deltaTime;
        if(Age > Duration){
            Age = 0;
        }

        sr.sprite = sprites[Mathf.RoundToInt(Age/Duration*(sprites.Length-1))];

    }
}
