using Unity.Collections;
using UnityEngine;

public class GlobalVariable : MonoBehaviour
{
    public static GlobalVariable instance;
    public Tile[, ] grid;
    public bool[, ] isOccuppied;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
}

public static class GlobalData
{
    public static int money = 10;
    public static int data;
}