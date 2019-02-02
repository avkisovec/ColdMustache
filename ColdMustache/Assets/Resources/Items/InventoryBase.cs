using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBase : MonoBehaviour {
    
    public List<InventorySlot> SlotScripts = new List<InventorySlot>();

    public virtual void ReportSlotBeingClicked(int id)
    {
        
    }
}
