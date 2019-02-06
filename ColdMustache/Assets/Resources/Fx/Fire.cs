using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public Sprite[] sprites;
    int LastSpriteIndex = -1;

    float TimeToSpriteChange = 0.1f;

    public float Age = 0;
    public float MaxAge = 10;

    SpriteRenderer sr;

    void Start()
    {
        sprites = Resources.LoadAll<Sprite>("Fx/Fire");
        sr = GetComponent<SpriteRenderer>();
        BoxCollider2D bc = gameObject.AddComponent<BoxCollider2D>();
        bc.isTrigger = true;
        bc.size = new Vector2(1,1);
    
        gameObject.AddComponent<DamagerInflicterAoE>().ini(Entity.team.Neutral, 1, MaxAge, true, 0.5f, -Age, DamagerInflicter.WeaponTypes.Fire);
    }

    // Update is called once per frame
    void Update()
    {
        Age+=Time.deltaTime;
        if(Age>MaxAge){
            Destroy(gameObject);
            return;
        }
        if(Age < 0){
            return;
        }
        if(TimeToSpriteChange <= 0){
            TimeToSpriteChange = 0.1f;
            RollNewSpriteIndex();
            sr.sprite = sprites[LastSpriteIndex];
        }
        else{
            TimeToSpriteChange -= Time.deltaTime;
        }
    }

    void RollNewSpriteIndex(){
        int NewIndex = Random.Range(0,sprites.Length);
        while(NewIndex == LastSpriteIndex){
            NewIndex = Random.Range(0, sprites.Length);
        }
        LastSpriteIndex = NewIndex;
    }
}
