using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavTestStaticFeeder : MonoBehaviour {
    
	void Start () {

        //set up an empty nav array (all tiles considered empty/walkable)
        NavTestStatic.NavArray = new int[NavTestStatic.MapWidth, NavTestStatic.MapHeight];
        for(int y = 0; y < NavTestStatic.MapWidth; y++)
        {
            for(int x = 0; x < NavTestStatic.MapHeight; x++)
            {
                NavTestStatic.NavArray[x, y] = NavTestStatic.EmptyTileValue;
            }
        }

        //set up the other arrays
        //explosion nav array and light array are clones of nav array, therefore empty on default
        NavTestStatic.ExplosionNavArray = (int[,])NavTestStatic.NavArray.Clone();
        NavTestStatic.LightNavArray = (int[,])NavTestStatic.NavArray.Clone();

        //go through all existing environment objects in scene, and if they block particular thing, note in in the array
        foreach (EnvironmentObject eo in GameObject.FindObjectsOfType<EnvironmentObject>())
        {
            if (!eo.Nav_Walkable)
            {
                NavTestStatic.NavArray[Mathf.RoundToInt(eo.transform.position.x), Mathf.RoundToInt(eo.transform.position.y)] = NavTestStatic.ImpassableTileValue;
            }
            if (!eo.Nav_ExplosionCanPass)
            {
                NavTestStatic.ExplosionNavArray[Mathf.RoundToInt(eo.transform.position.x), Mathf.RoundToInt(eo.transform.position.y)] = NavTestStatic.ImpassableTileValue;
            }
            if (!eo.Nav_LightCanPass)
            {
                NavTestStatic.LightNavArray[Mathf.RoundToInt(eo.transform.position.x), Mathf.RoundToInt(eo.transform.position.y)] = NavTestStatic.ImpassableTileValue;
            }
        }



    }
	
}
