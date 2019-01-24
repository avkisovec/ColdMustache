using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWallLines : MonoBehaviour {

    public string LineSheetPath;
    Sprite[] LineSheet;
    
    // Use this for initialization
    void Start () {

        NavTestStatic.ExportNavMap();

        LineSheet = Resources.LoadAll<Sprite>(LineSheetPath);

        int Impassable = NavTestStatic.ImpassableTileValue;
        for(int x = 1; x <NavTestStatic.MapWidth - 1; x++)
        {
            for(int y = 1; y <NavTestStatic.MapHeight - 1; y++)
            {
                if(NavTestStatic.NavArray[x,y] != Impassable)
                {
                    bool ImpassableRight = false;
                    bool ImpassableUp = false;
                    bool ImpassableLeft = false;
                    bool ImpassableDown = false;

                    if (NavTestStatic.NavArray[x + 1, y] == Impassable)
                    {
                        ImpassableRight = true;
                    }
                    if (NavTestStatic.NavArray[x, y + 1] == Impassable)
                    {
                        ImpassableUp = true;
                    }
                    if(NavTestStatic.NavArray[x-1, y] == Impassable)
                    {
                        ImpassableLeft = true;
                    }
                    if(NavTestStatic.NavArray[x, y-1] == Impassable)
                    {
                        ImpassableDown = true;
                    }

                    //one nearby impassable found, lets spawn a line
                    if(ImpassableRight || ImpassableUp || ImpassableLeft || ImpassableDown)
                    {
                        GameObject go = new GameObject();
                        go.transform.position = new Vector3(x, y, -10);
                        go.AddComponent<SpriteRenderer>().sprite = WallSpriteFinder.Find(LineSheet, !ImpassableRight, !ImpassableUp, !ImpassableLeft, !ImpassableDown);
                        go.AddComponent<ZIndexManager>().SingleUse = true;
                        go.GetComponent<ZIndexManager>().RelativeValue = 1.01f;

                    }

                }
            }
        }

	}
	
}
