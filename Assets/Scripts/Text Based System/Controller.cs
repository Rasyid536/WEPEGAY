using UnityEngine;
using System.Collections.Generic;

public class Controller : MonoBehaviour
{
    public static WindowsCommand SEND_DEBUG;
    public List<WindowsCommand> commandList;

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

    void HandleInput()
    {
        foreach (WindowsCommand command in commandList)
        {
            if (input == command.commandID)
            {
                command.Invoke();
            }
        }

        input = "";
    }

    void OnGUI()
    {
        float y = 400;

        //GUI.Box(new Rect(Screen.width / 4, y, Screen.width / 2, 70), "");

        GUI.SetNextControlName("ConsoleInput");

        input = GUI.TextField(
            new Rect(Screen.width / 4 + 5f, y + 18, Screen.width / 2 - 10f, 40),
            input
        );

        if (Event.current.isKey && Event.current.keyCode == KeyCode.Return)
        {
            HandleInput();
        }
    }
}