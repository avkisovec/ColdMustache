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

    //inventory which this slot belongs to
    public InventoryBase ParentInventory = null;

    //what type of item goes into this slot
    public InventoryItem.ItemType SlotType = InventoryItem.ItemType.Undefined;

    //unique id of this specific slot
    public int SlotId;


	// Use this for initialization
	void Start () {
	}

    private void Awake()
    {
        ParentInventory.SlotsScripts.Add(this);
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
            ParentInventory.ReportSlotBeingClicked(SlotId);
        }

    }
}
