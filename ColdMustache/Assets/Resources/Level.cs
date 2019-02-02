using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour {
    	
	void Update () {
        
        if (Input.GetKeyUp(KeyCode.F1))
        {
            ReloadScene();
        }
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
