using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour {

    public string[] ItemPaths;

    public string StorageInventoryMenuPath;

    Sprite DefaultSprite;
    public Sprite HoverSprite;

    SpriteRenderer sr;

    public bool IsCurrentlyOpened = false; //can only be opened if not currently opened; when menu is closed it has to change this to false

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        DefaultSprite = sr.sprite;
	}
	
	// Update is called once per frame
	void Update () {

        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (MouseWorldPos.x > transform.position.x - (transform.lossyScale.x / 2) &&
            MouseWorldPos.x < transform.position.x + (transform.lossyScale.x / 2) &&
            MouseWorldPos.y > transform.position.y - (transform.lossyScale.y / 2) &&
            MouseWorldPos.y < transform.position.y + (transform.lossyScale.y / 2) &&
            !IsCurrentlyOpened
            )
        {
            UniversalReference.crosshair.SetTemporaryColor(new Color(1,1,0,1), 0.1f);
            sr.sprite = HoverSprite;

            if (Input.GetKeyDown(KeybindManager.Interaction))
            {
                /*
                GameObject MenuContainer = new GameObject();
                MenuContainer.transform.position = new Vector3(transform.position.x, transform.position.y, -50);
                MenuContainer.AddComponent<>
                */

                IsCurrentlyOpened = true;

                //if another one is currently opened, close that one
                if(UniversalReference.PlayerInventory.OpenStorage != null)
                {
                    UniversalReference.PlayerInventory.OpenStorage.transform.parent.GetComponent<InventoryStorageCloser>().Close();
                }

                GameObject go = Instantiate(Resources.Load<GameObject>(StorageInventoryMenuPath) as GameObject);

                go.transform.position = new Vector3(transform.position.x, transform.position.y, -50);
                go.transform.localScale = UniversalReference.PlayerInventory.transform.parent.localScale; //same scale as inventory

                InventoryStorage invs = go.transform.GetChild(0).GetComponent<InventoryStorage>();

                invs.ParentStorageObject = this; // storageInventory needs reference back here, as it needs to load all the items from strings in this script; but it can only load them later when slots have reported themselves in, so i cant load items now


                UniversalReference.PlayerInventory.OpenStorage = invs;
                UniversalReference.PlayerInventory.UpdateStorageState();

            }
        }
        else
        {
            sr.sprite = DefaultSprite;
        }

    }
}
