using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothingItem : InventoryItem {

    public Sprite[] Sprites = null;
    public string SpritesPath = "";

    public Color color;

    private void Start()
    {
        LoadSpritesIfNeeded();
    }

    public void LoadSpritesIfNeeded()
    {
        if (true || Sprites == null)
        {
            Sprites = Resources.LoadAll<Sprite>(SpritesPath);
        }
    }

    public override void CodeBeforeRemoving()
    {
        //UniversalReference.PlayerSpriteManager.UnequipClothing(Type);
        switch (Type)
        {
            case ItemType.ClothingShirt:
                UniversalReference.PlayerSpriteManager.sprites[1] = UniversalReference.EmptyBodyPart;
                break;
            case ItemType.ClothingJacket:
                UniversalReference.PlayerSpriteManager.sprites[3] = UniversalReference.EmptyBodyPart;
                break;
            case ItemType.ClothingHead:
                UniversalReference.PlayerSpriteManager.sprites[7] = UniversalReference.EmptyBodyPart;
                break;
        }
        UniversalReference.PlayerSpriteManager.UpdateEverything();
    }

    public override void CodeAfterEquipping()
    {
        LoadSpritesIfNeeded();
        {
            switch (Type)
            {
                case ItemType.ClothingShirt:
                    UniversalReference.PlayerSpriteManager.sprites[1] = Sprites;
                    UniversalReference.PlayerSpriteManager.colors[1] = color;
                    break;
                case ItemType.ClothingJacket:
                    UniversalReference.PlayerSpriteManager.sprites[3] = Sprites;
                    UniversalReference.PlayerSpriteManager.colors[3] = color;
                    break;
                case ItemType.ClothingHead:
                    UniversalReference.PlayerSpriteManager.sprites[7] = Sprites;
                    UniversalReference.PlayerSpriteManager.colors[7] = color;
                    break;
            }
        }
        UniversalReference.PlayerSpriteManager.UpdateEverything();
    }
}
