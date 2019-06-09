using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditChildrenBoxCollider : MonoBehaviour
{
    /*
    
        goes through all children and if they have a box collider it is edited

        this script will classify the box-collider-having objact as either wall top or wallfront,
        and based on that will edit the boxcollider

        DO NOT USE THIS SCRIPT IF THE CONTAINER OBJECT CONTAINS OTHER BOX-COLLIDER-HAVING OBJECTS
        OTHER THAN WALL TOPS AND WALL FRONTS

        //wall tops: offset [0;0] size [1;1]
        //wall fronts: offset [0;0.25] size [1;0.5]
    
     */


    public bool ENABLED = true;

    // FOR THE PURPOSE OF THIS SCRIPT,
    // WALL TOP ALSO INCLUDES THE ONE HIDDEN BY WALLFRONT

    Vector2 WallTopColliderOffset = new Vector2(0,0);
    Vector2 WallTopColliderSize = new Vector2(1,1);
    Vector2 WallFrontColliderOffset = new Vector2(0, 0.25f);
    Vector2 WallFrontColliderSize = new Vector2(1, 0.5f);

    void Start()
    {

        if (!ENABLED) return;
        ENABLED = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            DoYourRecursiveThing(transform.GetChild(i));
        }

    }

    void DoYourRecursiveThing(Transform tr)
    {
        BoxCollider2D bc = tr.GetComponent<BoxCollider2D>();
        if (bc != null)
        {
            Vector2Int TilePos = Util.Vector3To2Int(tr.position);

            if(NavTestStatic.IsTileWallFront(TilePos.x, TilePos.y))
            {
                bc.offset = WallFrontColliderOffset;
                bc.size = WallFrontColliderSize;
            }
            else
            {
                bc.offset = WallTopColliderOffset;
                bc.size = WallTopColliderSize;
            }

        }

        for (int i = 0; i < tr.childCount; i++)
        {
            DoYourRecursiveThing(tr.GetChild(i));
        }

    }

}
