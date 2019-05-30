using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTest : DialogBase
{
    void Start()
    {


        DialogNodeText dstart = new DialogNodeText("", false);
        DialogNodeText d0 = new DialogNodeText("First sentence", false);
        DialogNodeText d1 = new DialogNodeText("Some other one", false);
        DialogNodeText d2 = new DialogNodeText("And\na\nBig\nWall", true);

        DialogNodeBranching b0 = new DialogNodeBranching("Your choice?", false, "I Choose A", "B\nseems\nnice", "Gonna go for C");

        DialogNodeText ba0 = new DialogNodeText("You chose A", false);
        DialogNodeText bb0 = new DialogNodeText("You chose B", false);
        DialogNodeText bc0 = new DialogNodeText("You chose C", false);

        DialogNodeText d3 = new DialogNodeText("Yea", false);
        DialogNodeText d4 = new DialogNodeText("And next one", false);
        DialogNodeText dend = new DialogNodeText("Is final", false);

        dstart.Next = d0;
        d0.Next = d1;
        d1.Next = d2;
        d2.Next = b0;

        b0.NextA = ba0;
        b0.NextB = bb0;
        b0.NextC = bc0;

        ba0.Next = d3;
        bb0.Next = d3;
        bc0.Next = d3;

        d3.Next = d4;
        d4.Next = dend;
        dend.Next = null;

        dstart.Activate();

    }

}
