using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel.Design;

// ============================================= //
//        This is the script where the game      //
//      command prompt system is processed       //
//      and controlled.                          //
//                                               //
//  Note : not fully understand this yet.        //
// ============================================= //

public class Controller : MonoBehaviour
{
    public static WindowsCommand SEND_DEBUG;
    public static WindowsCommand<int> SET_DEBUG;
    public List<object> commandList;
    [SerializeField]private float yPadding;

    string input = "";

    void Awake()
    {
        SEND_DEBUG = new WindowsCommand(
            "send_debug",
            "Sending message",
            "send_debug",
            () =>
            {
                ControllerEnd.instance.Debugs();
            });

        SET_DEBUG = new WindowsCommand<int>("set_debug", 
            "set the value of debug message", 
            "set_debug<amount>", 
            (x) =>
            {
                ControllerEnd.instance.DebugVal(x);
            });

        commandList = new List<object>
        {
            SEND_DEBUG,
            SET_DEBUG
        };
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            HandleInput();
        }
    }



    // input handler function
    void HandleInput()
    {
        string[] properties = input.Split(' ');

        if (properties.Length == 0) return;

        for(int i = 0; i < commandList.Count; i++)
        {
            WindowsCommandBase commandBase = commandList[i] as WindowsCommandBase;

            if (properties[0] == commandBase.commandID)
            {
                if (commandList[i] is WindowsCommand simpleCommand)
                {
                    simpleCommand.Invoke();
                }
                else if (commandList[i] is WindowsCommand<int> intCommand)
                {
                    if (properties.Length < 2)
                    {
                        Debug.Log("Butuh angka!");
                        return;
                    }

                    if (!int.TryParse(properties[1], out int value))
                    {
                        Debug.Log("Format angka salah!");
                        return;
                    }

                    intCommand.Invoke(value);
                }
            }
        }
        input = "";
    }




    void OnGUI()
    {
        float y = 400;

        GUI.SetNextControlName("ConsoleInput");

        // the text input goes here
        input = GUI.TextField(
            new Rect(Screen.width / 4 + 5f, y + yPadding , Screen.width / 2 - 10f , 40),
            input
        );

        // input handler call
        if (Event.current.isKey && Event.current.keyCode == KeyCode.Return) // if iskey and return pressed
        {
            HandleInput();
        }
    }
}