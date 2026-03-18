using UnityEngine;

// ============================================= //
//        This is the script where the game      //
//      function that called in the command      //
//      prompt of the game was called.           //
//                                               //
// ============================================= //

public class ControllerEnd : MonoBehaviour
{
    public static ControllerEnd instance;

    public void Awake()
    {
        instance = this;
    }

    public void Debugs()
    {
        Debug.Log("hbdfjk");
    }
}
