using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryStorageCloser : MonoBehaviour {
    
    public GameObject CloseButton;

    public InventoryStorage inventory;
    	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeybindManager.CloseMenus) ||
            (inventory.ParentStorageObject.transform.position - UniversalReference.PlayerObject.transform.position).magnitude > 10 ||
            (
                Input.GetKeyDown(KeyCode.Mouse0) &&
                UniversalReference.MouseWorldPos.x > CloseButton.transform.position.x - (CloseButton.transform.lossyScale.x / 2) &&
                UniversalReference.MouseWorldPos.x < CloseButton.transform.position.x + (CloseButton.transform.lossyScale.x / 2) &&
                UniversalReference.MouseWorldPos.y > CloseButton.transform.position.y - (CloseButton.transform.lossyScale.y / 2) &&
                UniversalReference.MouseWorldPos.y < CloseButton.transform.position.y + (CloseButton.transform.lossyScale.y / 2)
                ))
        {
            Close();  
        }
    }

    public void Close()
    {
        //end interaction with player inventory
        UniversalReference.PlayerInventory.OpenStorage = null;
        UniversalReference.PlayerInventory.UpdateStorageState();

        inventory.SaveBeforeClosing();

        inventory.ParentStorageObject.IsCurrentlyOpened = false;

        Destroy(this.gameObject);

    }

}
