using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManagerHand : SpriteManagerGeneric {

    /*
    * INDEXES: (for human AND HIS HAND)
    * 
    * SPRITES:
    * 
    * 0 - body      (triple)
    * 1 - shirt     ([0][1][2] normal, [3] hand [4] otherhand)
    * 2 - head      (triple)
    * 3 - coat      ([0][1][2] normal, [3] hand [4] otherhand)
    * 4 - eyes      (triple)
    * 5 - face      (triple)
    * 6 - hair      (triple)
    * 7 - headwear  (triple)
    * 8 - hands     ([0] hand [4] otherhand)
    * 
    * 
    * SPRITE RENDERERS:
    * 0 - body
    * 1 - shirt
    * 2 - head (part of body, not headwear)
    * 3 - coat
    * 4 - eyes
    * 5 - face (facial hair)
    * 6 - hair
    * 7 - hat
    * 
    * 8 - Hand (skin)
    * 9 - Hand (shirt)
    * 10 - Hand (jacket)
    * 11 - Other Hand (skin)
    * 12 - Other Hand (shirt)
    * 13 - Other Hand (jacket)
    *
    * 
    * 
    */

    public override void UpdateEverything(int Direction)
    {
        int Length = spriteRenderers.Length;
        switch (Direction)
        {
            //right (side)
            case 0:
                for (int i = 0; i < Length; i++)
                {
                    switch (i)
                    {
                        case 2: //head - use body's color
                            spriteRenderers[2].sprite = sprites[2][1];
                            spriteRenderers[2].color = colors[0];
                            spriteRenderers[2].flipX = false;
                            break;
                        case 6: //hair - use facial hair's color
                            spriteRenderers[6].sprite = sprites[6][1];
                            spriteRenderers[6].color = colors[5];
                            spriteRenderers[6].flipX = false;
                            break;
                        case 8: //8 - 13 are hands
                            spriteRenderers[8].sprite = sprites[8][0];
                            spriteRenderers[8].color = colors[0];       //hands take color from body
                            break;
                        case 9:
                            spriteRenderers[9].sprite = sprites[1][3];
                            spriteRenderers[9].color = colors[1];
                            break;
                        case 10:
                            spriteRenderers[10].sprite = sprites[3][3];
                            spriteRenderers[10].color = colors[3];
                            break;
                        case 11:
                            spriteRenderers[11].sprite = sprites[8][1];
                            spriteRenderers[11].color = colors[0];
                            break;
                        case 12:
                            spriteRenderers[12].sprite = sprites[1][4];
                            spriteRenderers[12].color = colors[1];
                            break;
                        case 13:
                            spriteRenderers[13].sprite = sprites[3][4];
                            spriteRenderers[13].color = colors[3];
                            break;
                        default:
                            spriteRenderers[i].sprite = sprites[i][1];
                            spriteRenderers[i].color = colors[i];
                            spriteRenderers[i].flipX = false;
                            break;
                    }                    

                }
                return;
            //up (back)
            case 90:
                for (int i = 0; i < Length; i++)
                {
                    switch (i)
                    {
                        case 2: //head - use body's color
                            spriteRenderers[2].sprite = sprites[2][2];
                            spriteRenderers[2].color = colors[0];
                            spriteRenderers[2].flipX = false;
                            break;
                        case 6: //hair - use facial hair's color
                            spriteRenderers[6].sprite = sprites[6][2];
                            spriteRenderers[6].color = colors[5];
                            spriteRenderers[6].flipX = false;
                            break;
                        case 8:
                            spriteRenderers[8].sprite = sprites[8][0];
                            spriteRenderers[8].color = colors[0];
                            break;
                        case 9:
                            spriteRenderers[9].sprite = sprites[1][3];
                            spriteRenderers[9].color = colors[1];
                            break;
                        case 10:
                            spriteRenderers[10].sprite = sprites[3][3];
                            spriteRenderers[10].color = colors[3];
                            break;
                        case 11:
                            spriteRenderers[11].sprite = sprites[8][1];
                            spriteRenderers[11].color = colors[0];
                            break;
                        case 12:
                            spriteRenderers[12].sprite = sprites[1][4];
                            spriteRenderers[12].color = colors[1];
                            break;
                        case 13:
                            spriteRenderers[13].sprite = sprites[3][4];
                            spriteRenderers[13].color = colors[3];
                            break;
                        default:
                            spriteRenderers[i].sprite = sprites[i][2];
                            spriteRenderers[i].color = colors[i];
                            spriteRenderers[i].flipX = false;
                            break;
                    }
                    
                }
                return;
            //left (side + flip)
            case 180:
                for (int i = 0; i < Length; i++)
                {
                    switch (i)
                    {
                        case 2: //head - use body's color
                            spriteRenderers[2].sprite = sprites[2][1];
                            spriteRenderers[2].color = colors[0];
                            spriteRenderers[2].flipX = true;
                            break;
                        case 6: //hair - use facial hair's color
                            spriteRenderers[6].sprite = sprites[6][1];
                            spriteRenderers[6].color = colors[5];
                            spriteRenderers[6].flipX = true;
                            break;
                        case 8:
                            spriteRenderers[8].sprite = sprites[8][0];
                            spriteRenderers[8].color = colors[0];
                            break;
                        case 9:
                            spriteRenderers[9].sprite = sprites[1][3];
                            spriteRenderers[9].color = colors[1];
                            break;
                        case 10:
                            spriteRenderers[10].sprite = sprites[3][3];
                            spriteRenderers[10].color = colors[3];
                            break;
                        case 11:
                            spriteRenderers[11].sprite = sprites[8][1];
                            spriteRenderers[11].color = colors[0];
                            break;
                        case 12:
                            spriteRenderers[12].sprite = sprites[1][4];
                            spriteRenderers[12].color = colors[1];
                            break;
                        case 13:
                            spriteRenderers[13].sprite = sprites[3][4];
                            spriteRenderers[13].color = colors[3];
                            break;
                        default:
                            spriteRenderers[i].sprite = sprites[i][1];
                            spriteRenderers[i].color = colors[i];
                            spriteRenderers[i].flipX = true;
                            break;
                    }
                    
                }
                return;
            //down (front)
            case 270:
                for (int i = 0; i < Length; i++)
                {
                    switch (i)
                    {
                        case 2: //head - use body's color
                            spriteRenderers[2].sprite = sprites[2][0];
                            spriteRenderers[2].color = colors[0];
                            spriteRenderers[2].flipX = false;
                            break;
                        case 6: //hair - use facial hair's color
                            spriteRenderers[6].sprite = sprites[6][0];
                            spriteRenderers[6].color = colors[5];
                            spriteRenderers[6].flipX = false;
                            break;
                        case 8:
                            spriteRenderers[8].sprite = sprites[8][0];
                            spriteRenderers[8].color = colors[0];
                            break;
                        case 9:
                            spriteRenderers[9].sprite = sprites[1][3];
                            spriteRenderers[9].color = colors[1];
                            break;
                        case 10:
                            spriteRenderers[10].sprite = sprites[3][3];
                            spriteRenderers[10].color = colors[3];
                            break;
                        case 11:
                            spriteRenderers[11].sprite = sprites[8][1];
                            spriteRenderers[11].color = colors[0];
                            break;
                        case 12:
                            spriteRenderers[12].sprite = sprites[1][4];
                            spriteRenderers[12].color = colors[1];
                            break;
                        case 13:
                            spriteRenderers[13].sprite = sprites[3][4];
                            spriteRenderers[13].color = colors[3];
                            break;
                        default:
                            spriteRenderers[i].sprite = sprites[i][0];
                            spriteRenderers[i].color = colors[i];
                            spriteRenderers[i].flipX = false;
                            break;
                    }
                    
                }
                return;


        }
    }

}
