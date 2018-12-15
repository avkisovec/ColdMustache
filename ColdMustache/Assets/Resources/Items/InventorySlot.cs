using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour {

    /*
     * 
     * INVENTORY SLOT CANNOT HAVE ANY CHILDREN
     * 
     * WITH THE EXCEPTION OF ITEM OCCUPYING THAT SLOT
     * WHICH HAS TO BE THE ONLY CHILD
     * 
     * (technically when a thing gets equipped for a few lines of code there are two items in one slot, but the old one immediately leaves)
     * 
     * 
     * 
     */


    public Inventory ParentInventory = null;

    public int SlotID = -1;

	// Use this for initialization
	void Start () {
        ParentInventory.Slots.Add(this);
	}
	
	// Update is called once per frame
	void Update () {

        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.Mouse0) &&
            MouseWorldPos.x > transform.position.x - (transform.lossyScale.x / 2) &&
            MouseWorldPos.x < transform.position.x + (transform.lossyScale.x / 2) &&
            MouseWorldPos.y > transform.position.y - (transform.lossyScale.y / 2) &&
            MouseWorldPos.y < transform.position.y + (transform.lossyScale.y / 2)
            )
        {
            ParentInventory.ReportSlotBeingClicked(SlotID);
        }

    }
}
