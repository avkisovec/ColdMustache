using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour {

    /*
     *          THE CONCEPT
     * 
     * this registers key strokes and recordes them in "currentString"
     * for example pressing [T] [E] [S] and [T] would result in "TEST" being stored in currentString
     * 
     * THIS SUPPORTS ONLY LETTERS A-Z AND [SPACE]. other characters will not be registered (and won't interrupt the sequence)
     * 
     * after pressing [ENTER], the sequence is verified
     * if the sequence starts with "IMADEV " (note the space at the end), the following text will be considered a cheat code
     * the cheat code (everything after the opening sequence IMADEV[space]) will be stored for one frame in LastCheat
     * 
     * it is recommended to press [ENTER] before inputting a code, otherwise your input string will be full of "WASDWASDWASDWASDWASD"
     * 
     * 
     *          USAGE
     * 
     *  if(CheatManager.LastCheat == "GODMODE") //you can write any custom made up phrase here - cheat manager doesnt verify if such a cheat exists, it just tells other classes what was typed
     *  {
     *      //stuff
     *  }
     *  
     * in game, then press [enter] I M A D E V [space] G O D M O D E [enter]
     * 
     * 
     * there is no visual feedback, you dont see typing or anything
     * 
     * THE CHEAT CODES HAVE TO BE IN FULL CAPS AND CAN ONLY CONTAIN A-Z AND [SPACE]
     * 
     * 
     */

    //visible for a single frame, then deletes self
    public static string LastCheat = "";

    string CurrentString = "";

    //the phrase that cheats have to start with
    public const string MagicPhrase = "IMADEV ";

    private void Awake()
    {
        LastCheat = "";
    }

    // Update is called once per frame
    void Update () {

        LastCheat = "";

        CheckInput();

        if(CurrentString.Length > 100)
        {
            StringReset();
        }

        if (Input.GetKeyUp(KeyCode.Return))
        {
            if(CurrentString.Length > MagicPhrase.Length)
            {
                if(CurrentString.Substring(0, MagicPhrase.Length) == MagicPhrase)
                {
                    CurrentString = CurrentString.Substring(MagicPhrase.Length);

                    LastCheat = CurrentString;

                }
            }
            StringReset();
        }

    }

    public void StringReset()
    {
        CurrentString = "";
    }

    public void CheckInput()
    {
        CheckAlphaChars();

        if (Input.GetKeyUp(KeyCode.Space))
        {
            CurrentString += " ";
        }
    }

    public void CheckAlphaChars()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            CurrentString += "A";
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            CurrentString += "B";
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            CurrentString += "C";
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            CurrentString += "D";
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            CurrentString += "E";
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            CurrentString += "F";
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            CurrentString += "G";
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            CurrentString += "H";
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            CurrentString += "I";
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            CurrentString += "J";
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            CurrentString += "K";
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            CurrentString += "L";
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            CurrentString += "M";
        }
        if (Input.GetKeyUp(KeyCode.N))
        {
            CurrentString += "N";
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            CurrentString += "O";
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            CurrentString += "P";
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            CurrentString += "Q";
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            CurrentString += "R";
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            CurrentString += "S";
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            CurrentString += "T";
        }
        if (Input.GetKeyUp(KeyCode.U))
        {
            CurrentString += "U";
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            CurrentString += "V";
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            CurrentString += "W";
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            CurrentString += "X";
        }
        if (Input.GetKeyUp(KeyCode.Y))
        {
            CurrentString += "Y";
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            CurrentString += "Z";
        }
    }

}
