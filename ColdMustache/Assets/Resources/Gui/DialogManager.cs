using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{

    //public static link to currently active dialog manager;
    public static DialogManager Curr = null;


    public DialogOption OptionContinue;
    public DialogOption OptionA;
    public DialogOption OptionB;
    public DialogOption OptionC;

    public Button BtnOptionContinue;
    public Button BtnOptionA;
    public Button BtnOptionB;
    public Button BtnOptionC;

    public DialogNodeBase NodeOptionContinue = null;
    public DialogNodeBase NodeOptionA = null;
    public DialogNodeBase NodeOptionB = null;
    public DialogNodeBase NodeOptionC = null;


    public List<Transform> TextBlocks;




    //includes spacing
    public static float LetterWidth = 7f/32f;
    public static float LetterHeight = 11f/32f;

    public static float TextBlockMarginTop = 4f/32f;
    public static float TextBlockMarginBot = 4f/32f;
    public static float TextBlockMarginLeft = 4f/32f;
    public static float TextBlockMarginRight = 4f/32f;
    public static float RightAlignedTextBlockLeftOffset = 50f/32f;
    public static float LeftAlignedTextBlockLeftOffset = -50f/32f;
    public static float TextBlockOriginY = 0f;

    public static int TextBlockLettersPerLine = 30; //how many letters per line max

    public static float TextBlockPixelWidth = TextBlockMarginLeft + (TextBlockLettersPerLine * LetterWidth) + TextBlockMarginRight;




    // Start is called before the first frame update
    void Start()
    {
        Curr = this;

        OptionContinue.ini(TextBlockLettersPerLine, 4);
        OptionA.ini(TextBlockLettersPerLine, 4);
        OptionB.ini(TextBlockLettersPerLine, 4);
        OptionC.ini(TextBlockLettersPerLine, 4);

    }

    // Update is called once per frame
    void Update()
    {

        if (BtnOptionContinue.Clicked && NodeOptionContinue != null) NodeOptionContinue.Activate();
        else if (BtnOptionA.Clicked && NodeOptionA != null) NodeOptionA.Activate();
        else if (BtnOptionB.Clicked && NodeOptionB != null) NodeOptionB.Activate();
        else if (BtnOptionC.Clicked && NodeOptionC != null) NodeOptionC.Activate();


        if(Input.GetKeyDown(KeyCode.R)){
            AddTextBlock("Response\nTest.", false);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            AddTextBlock("A\nB\nC\nD\nE", false);
        }


        if (Input.GetKeyDown(KeyCode.N))
        {
            SetOptionText(true);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SetOptionText(false, "first option", "another\none", "and one more");
        }


        /*
        if(MyButton.Clicked){
            DialogOption[0].SetText("clicked");
        }
        if(Input.GetKeyUp(KeyCode.D)){
            DialogOption[0].SetText("changed");
        }
        */
    }

    public void SetOptionText(bool AllowContinue, string OptionAtext = " ", string OptionBtext = " ", string OptionCtext = " "){
        
        if(AllowContinue) OptionContinue.SetText("(continue)");
        else OptionContinue.SetText("");

        OptionA.SetText(OptionAtext);
        OptionB.SetText(OptionBtext);
        OptionC.SetText(OptionCtext);

    }

    public void SetOptionNodes(DialogNodeBase Continue, DialogNodeBase A = null, DialogNodeBase B = null, DialogNodeBase C = null)
    {
        NodeOptionContinue = Continue;
        NodeOptionA = A;
        NodeOptionB = B;
        NodeOptionC = C;

    }

    public void AddTextBlock(string text, bool LeftAligned = true){

        int Lines = AlphabetManager.BreakText2_Array(text, TextBlockLettersPerLine).Count;
      
        text = AlphabetManager.BreakText(text, TextBlockLettersPerLine);

        GameObject TextBlockContainer = new GameObject();
        if (LeftAligned) TextBlockContainer.transform.localPosition = new Vector3(LeftAlignedTextBlockLeftOffset, TextBlockOriginY, 0);
        else TextBlockContainer.transform.localPosition = new Vector3(RightAlignedTextBlockLeftOffset, TextBlockOriginY, 0);
        TextBlocks.Add(TextBlockContainer.transform);

        foreach (Transform t in TextBlocks)
        {
            t.position += new Vector3(0,
                TextBlockMarginBot + (Lines * LetterHeight) + TextBlockMarginTop
            , 0);
        }

        GameObject go = new GameObject();
        go.transform.parent = TextBlockContainer.transform;
        go.transform.localPosition = new Vector3(LetterWidth/2, -LetterHeight/2);

        
        go.AddComponent<TextBlock>().ini(TextBlockLettersPerLine, Lines, text);
       

    }

}
