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

    // debug function 
    public void Debugs()
    {
        Debug.Log("This is called");
    }
    public void DebugVal(int x)
    {
        Debug.Log ($"Nilai " + x);
    }
    public void SendMSG(int x, int y)
    {
        Debug.Log($"Koord {x}, {y}");
    }
}
