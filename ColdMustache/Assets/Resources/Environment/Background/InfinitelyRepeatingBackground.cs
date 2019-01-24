using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinitelyRepeatingBackground : MonoBehaviour {

    /*
     * when player enters this background, it spawns 4 copies of itself on all 4 edges and 4 corners
     * 
     * if player enters one of those copies, all current backgrounds will be deleted and new copies will spawn around the one that player rjust entered
     * 
     * 
     * 
     * 
     */


    public static List<GameObject> CurrentBackgrounds = new List<GameObject>();


    public Sprite Background;

    public Transform Player;

    // 1920 px / 32ppu * 100x scale = 6000 units
    public int Width = 6000;

    // 1920 px / 32ppu * 100x scale = 3375 units
    public int Height = 3375;

    public bool HasAlreadySpawnedClones = false;

	// Use this for initialization
	void Start () {
        
	}

    // Update is called once per frame
    void Update()
    {
        if (!HasAlreadySpawnedClones)
        {
            if (Player.transform.position.x > transform.position.x - Width / 2 &&
               Player.transform.position.x < transform.position.x + Width / 2 &&
               Player.transform.position.y > transform.position.y - Height / 2 &&
               Player.transform.position.y < transform.position.y + Height / 2)
            {
                CurrentBackgrounds.Remove(gameObject);

                foreach(GameObject go in CurrentBackgrounds)
                {
                    Destroy(go);
                }

                CurrentBackgrounds.Add(gameObject);
                for (int x = -Width; x <= Width; x += Width)
                {
                    for (int y = -Height; y <= Height; y += Height)
                    {
                        if (x != 0 || y != 0)
                        {
                            GameObject bg = new GameObject();
                            bg.AddComponent<SpriteRenderer>().sprite = Background;
                            bg.transform.position = transform.position + new Vector3(x, y, 0);
                            bg.transform.localScale = transform.localScale;

                            InfinitelyRepeatingBackground irb = bg.AddComponent<InfinitelyRepeatingBackground>();
                            //instead of passing the values it would be better to have them static
                            irb.Background = Background;
                            irb.Player = Player;
                            irb.Width = Width;
                            irb.Height = Height;

                            CurrentBackgrounds.Add(bg);
                        }
                    }
                }

                HasAlreadySpawnedClones = true;
            }
        }
        
    }
}
