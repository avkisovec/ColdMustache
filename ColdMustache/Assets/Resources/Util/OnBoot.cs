﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class OnBoot : MonoBehaviour {

    /*
     * this script always has to be the first one executed during the booting of the game
     * 
     * its main purpose is to check if save files and other required text files exist
     * if they do not exist (such as when first launching the game), they are created as a "clone" of the default files
     * 
     */

    public bool HasBeenCalled = false;
    private void Awake()
    {
        if (!HasBeenCalled)
        {
            HasBeenCalled = true;
            //Debug.Log(Application.dataPath);
            SaverLoader.CheckFileAndCreateIfNeeded("Generated/Preset/Animal01_Default.preset", "Entities/Animal/Animal01/Preset_Default");
            SaverLoader.CheckFileAndCreateIfNeeded("Generated/Preset/Human_Default.preset", "Entities/Human/Preset_Default");
            SaverLoader.CheckFileAndCreateIfNeeded("Generated/Preset/ShippingCrate01_Default.preset", "Environment/Physical/Objects/ShippingCrate01/Preset_Default");
        }

    }
}
