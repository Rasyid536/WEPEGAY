using UnityEngine;
using System.Collections.Generic;

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
    public List<WindowsCommand> commandList;
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

        commandList = new List<WindowsCommand>
        {
            SEND_DEBUG
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
        foreach (WindowsCommand command in commandList)
        {
            if (input == command.commandID) // if input data is the same as in the list
            {
                command.Invoke(); // call command
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