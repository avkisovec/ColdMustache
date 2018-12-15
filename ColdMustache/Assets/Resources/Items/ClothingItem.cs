using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothingItem : InventoryItem {

    public Sprite[] Sprites = null;
    public string SpritesPath = "";

    public Color Color;

    private void Start()
    {
        if(true || Sprites == null)
        {
            Sprites = Resources.LoadAll<Sprite>(SpritesPath);
        }
    }

    public override void CodeBeforeRemoving()
    {
        PlayerReference.SpriteManagerBuilder.UnequipClothing(Type);
    }

    public override void CodeAfterEquipping()
    {
        PlayerReference.SpriteManagerBuilder.EquipClothing(Type, Sprites, Color);
    }
}
