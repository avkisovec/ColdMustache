using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityIdDistributor : MonoBehaviour
{

    /*
     *  this script gives unique int ID to entities that request one
     *
     *  this is for the purpose of DoT AoE - they need to know who they already damaged
     *
     *
     */

    static int LatestId = 0;

    public static int GetUniqueId(){
        LatestId++;
        return LatestId;
    }

}
