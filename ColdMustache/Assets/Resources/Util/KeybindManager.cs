using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindManager : MonoBehaviour {

    /*
     * instead of using Keycode.Something, use KeybindManager.Something
     * 
     * if you need shift/ctrl/alt interaction, you can check press of those keys in Get Set, or turn variables into methods
     * 
     */

    //movement
    public static KeyCode MoveUp = KeyCode.W;
    public static KeyCode MoveDown = KeyCode.S;
    public static KeyCode MoveLeft = KeyCode.A;
    public static KeyCode MoveRight = KeyCode.D;

    //combat
    public static KeyCode MousePrimary = KeyCode.Mouse0; //this could be renamed to "shootPrimary", but since there is a whole MouseInterceptor thing, ill keep calling it "mouse"
    public static KeyCode AltFire = KeyCode.Q;
    public static KeyCode Reload = KeyCode.R;
    public static KeyCode UseItem = KeyCode.F;

    //menus
    public static KeyCode OpenInventory = KeyCode.E;
    public static KeyCode Interaction = KeyCode.E;
    public static KeyCode CloseMenus = KeyCode.Escape;


}
