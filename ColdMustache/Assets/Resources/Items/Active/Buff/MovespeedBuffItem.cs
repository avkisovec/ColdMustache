﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovespeedBuffItem : ActiveItem
{
    public float BuffCoefficient = 1.5f;

    public float Duration = 30;

    public int Charges = 1;
    public Sprite[] ChargesSprites;
    public Sprite EmptySprite;
    public SpriteRenderer sr;

    public string FloatingTextWhenUsingWithoutCharges = "I cannot use this anymore.";

    public override void CodeAfterEquipping()
    {
        UniversalReference.PlayerScript.CurrentlyEquippedItem = this;
        UniversalReference.SelectedItemIcon.sprite = sr.sprite;
    }

    public override void DoYourThing()
    {
        if (Charges > 0)
        {
            Entity e = UniversalReference.PlayerEntity;
            e.gameObject.AddComponent<MovespeedBuffStatus>().ini(Duration, BuffCoefficient);
            Charges--;
            if (Charges > 0)
            {
                sr.sprite = ChargesSprites[ChargesSprites.Length - 1];
            }
            else
            {
                sr.sprite = EmptySprite;
            }
        }
        else
        {
            AlphabetManager.SpawnFloatingText(FloatingTextWhenUsingWithoutCharges, new Vector3(UniversalReference.PlayerObject.transform.position.x, UniversalReference.PlayerObject.transform.position.y, -45));
        }
        UniversalReference.SelectedItemIcon.sprite = sr.sprite;
    }

    public override void CodeBeforeRemoving()
    {
        UniversalReference.PlayerScript.CurrentlyEquippedItem = null;
    }

    public override void Refill()
    {
        Charges = 1;
    }
}
