using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogNodeBranching : DialogNodeBase
{

    public string Text = null; //branching node doesnt have to have text
    public bool AlignLeft = false; //doesnt matter if it doesnt have text


    public string OptionA = " ";
    public string OptionB = " ";
    public string OptionC = " ";

    public DialogNodeBase NextA = null;
    public DialogNodeBase NextB = null;
    public DialogNodeBase NextC = null;


    public override void Activate()
    {
        if(Text != null){
            DialogManager.Curr.AddTextBlock(Text, AlignLeft);
        }

        DialogManager.Curr.SetOptionText(false, OptionA, OptionB, OptionC);
        DialogManager.Curr.SetOptionNodes(null, NextA, NextB, NextC);


    }

    public DialogNodeBranching(string Text = "", bool AlignLeft = false, string OptionA = "", string OptionB = "", string OptionC = "")
    {
        this.Text = Text;
        this.AlignLeft = AlignLeft;
        this.OptionA = OptionA;
        this.OptionB = OptionB;
        this.OptionC = OptionC;
    }

}
