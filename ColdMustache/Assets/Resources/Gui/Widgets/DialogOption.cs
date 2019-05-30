using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogOption : MonoBehaviour
{

    /*
        
    
    
     */

     
    SpriteRenderer[,] Srs;


    //it will "push" the next one so that if more text appears on one all following ones will align
    public Transform NextDialogOption;

    public Transform ButtonPixel;



    void Start()
    {

    }
    /*
        // Update is called once per frame
        void Update()
        {

        }
        */


    public void SetText(string text){

        //break text into lines
        List<string> lines = AlphabetManager.BreakText2_Array(text, DialogManager.TextBlockLettersPerLine);

        //store the original amount of lines
        int OriginalLineCount = lines.Count;

        //add empty (space) lines (to overwrite everything)
        for(int i = OriginalLineCount; i < Srs.GetLength(1); i++){
            lines.Add(" ");
        }

        //pad all lines with spacas (again to overwrite everything)
        for(int i = 0; i < lines.Count; i++){
            lines[i] = lines[i].PadRight(DialogManager.TextBlockLettersPerLine);
        }

        //set the text
        for(int y = 0; y < lines.Count; y++){
            for(int x = 0; x < lines[y].Length; x++){
                Srs[x,y].sprite = AlphabetManager.GetSprite(lines[y][x]);
            }
        }


        Vector3 UpperLeftCorner = new Vector3(
            transform.position.x - DialogManager.LetterWidth/2,
            transform.position.y + DialogManager.LetterHeight/2,
            0
        );

        Vector3 Size = new Vector3(
            DialogManager.TextBlockLettersPerLine * DialogManager.LetterWidth,
            OriginalLineCount * DialogManager.LetterHeight,
            1
        );

        Vector3 LowerRightCorner = new Vector3(
            UpperLeftCorner.x + Size.x,
            UpperLeftCorner.y - Size.y,
            0
        );

        //Vector3 LowerLeftCorner = new Vector3(UpperLeftCorner.x, LowerRightCorner.y);

        //Vector3 UpperRightCorner = new Vector3(LowerRightCorner.x, UpperLeftCorner.y);


        ButtonPixel.transform.position = new Vector3(UpperLeftCorner.x + Size.x/2, LowerRightCorner.y + Size.y/2, 1);

        ButtonPixel.transform.localScale = Size;

        //if move the dialogmanager that is after you (kinda like css float)
        if(NextDialogOption == null) return;
        NextDialogOption.position = new Vector3(transform.position.x,
            transform.position.y -(DialogManager.TextBlockMarginTop + (OriginalLineCount * DialogManager.LetterHeight) + DialogManager.TextBlockMarginBot)
            ,transform.position.z);

    }

    public void ini(int Width, int Height)
    {

        Srs = new SpriteRenderer[Width, Height];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                GameObject go = new GameObject();
                go.transform.parent = transform;
                go.transform.localPosition = new Vector3(x*DialogManager.LetterWidth, -y*DialogManager.LetterHeight, 0);
                Srs[x,y] = go.AddComponent<SpriteRenderer>();

            }

        }

    }

}
