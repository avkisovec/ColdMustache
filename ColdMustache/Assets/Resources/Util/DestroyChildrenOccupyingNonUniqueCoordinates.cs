using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyChildrenOccupyingNonUniqueCoordinates : MonoBehaviour
{

    /*
    
        goes through all direct children

        notes their position

        if some child if located on an already occupied position, it is destroyed

        this solves issues if lets say duplication mistake causes two identical to be stacked inside one another


        ROUNDS ALL COORDINATES TO INT
        
     */


    public bool ENABLED = true;


    // Start is called before the first frame update
    void Start()
    {

        if(!ENABLED) return;
        ENABLED = false;

        bool[,] Occupied = new bool[NavTestStatic.MapWidth, NavTestStatic.MapHeight];


        int ChildCount = transform.childCount;
        for (int i = 0; i < ChildCount; i++)
        {
            Transform Child = transform.GetChild(i);
            int x = Mathf.RoundToInt(Child.position.x);
            int y = Mathf.RoundToInt(Child.position.y);

            if(Occupied[x,y])
            {
                Destroy(Child.gameObject);
                continue;
            }
            else{
                Occupied[x,y] = true;
            }


        }

    }

}
