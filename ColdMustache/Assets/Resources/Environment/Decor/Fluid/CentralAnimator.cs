using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralAnimator : MonoBehaviour
{
    /*
     *  this script is primarily intended for large bodies of water, though can animate any other large amount of identical objects
     *
     */

    public float Duration = 1;

    int LastSpriteIndex = -1;

    float Age = 0;

    public string SpritesPath;

    Sprite[] sprites;

    SpriteRenderer[] sr;

    // Start is called before the first frame update
    void Start()
    {
        sprites = Resources.LoadAll<Sprite>(SpritesPath);
        
        sr = transform.GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Age += Time.deltaTime;
        if (Age > Duration)
        {
            Age = 0;
        }

        int NewSpriteIndex = Mathf.RoundToInt(Age / Duration * (sprites.Length - 1));

        if(NewSpriteIndex!=LastSpriteIndex){
            LastSpriteIndex = NewSpriteIndex;
            for(int i = 0; i < sr.Length; i++){
                sr[i].sprite = sprites[NewSpriteIndex];
            }
        }
    }
}
