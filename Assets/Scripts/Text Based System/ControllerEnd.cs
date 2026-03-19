using NUnit.Framework.Internal;
using Unity.Mathematics;
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
    public GameObject testObject;

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
        if(x < GlobalVariable.instance.grid.GetLength(0) && y < GlobalVariable.instance.grid.GetLength(1))
        {
            Debug.Log($" Posisi grid ke : {GlobalVariable.instance.grid[x, y].transform.position}, Length of x {GlobalVariable.instance.grid.GetLength(0)}, Length of y {GlobalVariable.instance.grid.GetLength(1)}");
            Instantiate(testObject, GlobalVariable.instance.grid[x, y].transform.position, quaternion.identity);
        }
        else
        {
            Debug.Log("Indeks diluar array");
        }
    }
}
