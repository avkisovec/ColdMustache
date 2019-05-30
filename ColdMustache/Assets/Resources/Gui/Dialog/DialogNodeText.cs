using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogNodeText : DialogNodeBase
{
    
    public string Text;

    public bool AlignLeft = false;


    public DialogNodeBase Next = null;


    public override void Activate(){

        DialogManager.Curr.AddTextBlock(Text, AlignLeft);

        DialogManager.Curr.SetOptionText(true);
        DialogManager.Curr.SetOptionNodes(Next);


    }

    public DialogNodeText(string Text, bool AlignLeft = false, DialogNodeBase Next = null){
        this.Text = Text;
        this.AlignLeft = AlignLeft;
        if(Next!=null) this.Next = Next;
    }

    
}
