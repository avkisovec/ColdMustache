using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSpatterSpawner : MonoBehaviour
{
    public string BloodSpatterSheetPath = "";
    public string BloodSpatterSheet2Path = "";
    public string BloodPoolPath = "";

    static Sprite[] BloodSpatterSprites;
    static Sprite[] BloodSpatterSprites2;
    static Sprite[] BloodPool;

    void Start()
    {
        BloodSpatterSprites = Resources.LoadAll<Sprite>(BloodSpatterSheetPath);
        BloodSpatterSprites2 = Resources.LoadAll<Sprite>(BloodSpatterSheet2Path);
        BloodPool = Resources.LoadAll<Sprite>(BloodPoolPath);
    }
    public static void SpawnSmall(Vector3 Position){

        if(Util.Coinflip()){
            //sheet 1 - long thin ones
            GameObject BloodSpatter = new GameObject();
            BloodSpatter.transform.position = new Vector3(Position.x, Position.y, 209);
            BloodSpatter.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            BloodSpatter.AddComponent<SpriteRenderer>().sprite = BloodSpatterSprites[Random.Range(0, BloodSpatterSprites.Length)];
        }
        else{
            //sheet 2 - smaller circular ones (spawn within 0.5 of target pos)
            GameObject BloodSpatter = new GameObject();
            BloodSpatter.transform.position = new Vector3(Position.x+Random.Range(-0.5f,0.5f), Position.y+Random.Range(-0.5f, 0.5f), 209);
            BloodSpatter.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            BloodSpatter.AddComponent<SpriteRenderer>().sprite = BloodSpatterSprites[Random.Range(0, BloodSpatterSprites2.Length)];
        }       
    }

    public static void SpawnPool(Vector3 Position){
        GameObject BloodSpatter = new GameObject();
        BloodSpatter.transform.position = new Vector3(Position.x, Position.y, 209);
        BloodSpatter.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        BloodSpatter.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Fx/BloodSpatter2");
        BloodSpatter.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
    }

}
