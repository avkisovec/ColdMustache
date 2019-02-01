using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfEnemyShips : MonoBehaviour {

    public static List<Transform> list = new List<Transform>();

    private void Awake()
    {
        list.Clear();
    }

}
