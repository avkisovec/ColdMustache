using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavTestStaticFeeder : MonoBehaviour {
    
	void Start () {
        NavTestStatic.NavArray = new int[NavTestStatic.MapWidth, NavTestStatic.MapHeight];
        for(int y = 0; y < NavTestStatic.MapWidth; y++)
        {
            for(int x = 0; x < NavTestStatic.MapHeight; x++)
            {
                NavTestStatic.NavArray[x, y] = NavTestStatic.EmptyTileValue;
            }
        }

        NavTestStatic.ExplosionNavArray = (int[,])NavTestStatic.NavArray.Clone();

		foreach(EnvironmentObject eo in GameObject.FindObjectsOfType<EnvironmentObject>())
        {
            if (!eo.Nav_Walkable)
            {
                NavTestStatic.NavArray[Mathf.RoundToInt(eo.transform.position.x), Mathf.RoundToInt(eo.transform.position.y)] = NavTestStatic.ImpassableTileValue;
            }
            if (!eo.Nav_ExplosionCanPass)
            {
                NavTestStatic.ExplosionNavArray[Mathf.RoundToInt(eo.transform.position.x), Mathf.RoundToInt(eo.transform.position.y)] = NavTestStatic.ImpassableTileValue;
            }
        }
	}
	
}
