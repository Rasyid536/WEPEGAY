using UnityEngine;

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
