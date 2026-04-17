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
    public GameObject PestKiller;

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
            GlobalVariable.instance.isOccuppied[x, y] = true;
        }
        else
        {
            Debug.Log("Indeks diluar array");
        }
    }

    public void IsOccupiedGrid(int x, int y)
    {
        if(GlobalVariable.instance.isOccuppied[x, y] == true)
        {
            Debug.Log($"Grid {x}, {y} sudah occupied");
        }
        else if(GlobalVariable.instance.isOccuppied[x, y] == false)
        {
            SendMSG(x, y);
        }
    }

    public void EraseObject(int x, int y)
    {
        if(GlobalVariable.instance.isOccuppied[x, y] == true)
        {
            Vector3 pos = GlobalVariable.instance.grid[x, y].transform.position;

            foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>()) // obsolete but okay lah
            {
                if (obj.transform.position == pos && !obj.CompareTag("Pest"))
                {
                    if (transform.parent != null)
                    {
                        Destroy(obj.transform.parent.gameObject);
                    }
                    else
                    {
                        Destroy(obj); // fallback
                    }
                    GlobalVariable.instance.isOccuppied[x, y] = false;
                    return;
                }
            }
        }

        Debug.Log("Tidak ada objek ditemukan");
    }

    public void PlacePestKiller(int x, int y)
    {
        if(x < GlobalVariable.instance.grid.GetLength(0) && y < GlobalVariable.instance.grid.GetLength(1) && GlobalVariable.instance.isOccuppied[x, y] == false)
        {
            Debug.Log($" Posisi grid ke : {GlobalVariable.instance.grid[x, y].transform.position}, Length of x {GlobalVariable.instance.grid.GetLength(0)}, Length of y {GlobalVariable.instance.grid.GetLength(1)}");
            Instantiate(PestKiller, GlobalVariable.instance.grid[x, y].transform.position, quaternion.identity);
            GlobalVariable.instance.isOccuppied[x, y] = true;
        }
        else
        {
            Debug.Log("Indeks diluar array atau tempat occupied");
        }
    }
}