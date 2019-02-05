using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float Delay = 3;
    public float ExplosionRadius = 5;
    float Age = 0;

    void Update()
    {
        //collider is added split second after its thrown, so it doesnt have a problem with the collider being inside player's collider
        if (Age < 0.2f && Age + Time.deltaTime > 0.2f)
        {
            gameObject.AddComponent<CircleCollider2D>().radius = 0.1f;
        }

        Age += Time.deltaTime;
        if(Age > Delay){
            Explosion_AvkisLight.SpawnOriginal(Util.Vector3To2Int(transform.position), 5);
            Destroy(gameObject);
        }
        
    }
}
