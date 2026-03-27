using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System;

// ============================================= //
//        This is the script where the game      //
//      command prompt system is processed       //
//      and controlled.                          //
// ============================================= //

public class Controller : MonoBehaviour
{

    // declare command list variable 
    public static WindowsCommand SEND_DEBUG;
    public static WindowsCommand<int> SET_DEBUG;
    public static WindowsCommand<int, int> PLANT_AT;
    public static WindowsCommand<int, int> ERASE_AT;
    public static WindowsCommand<int, int> PLACEA_PESTKILL;
    // declare command list variable


    public List<object> commandList;
    [SerializeField]private float yPadding, xPadding, width; // gui margin
    GUIStyle style; string input = "";

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


        SET_DEBUG = new WindowsCommand<int>(
            "set_debug", 
            "set the value of debug message", 
            "set_debug<amount>", 
            (x) =>
            {
                ControllerEnd.instance.DebugVal(x);
            });


        PLANT_AT = new WindowsCommand<int, int>(
            "plant_atxy", 
            "send you the x and y", 
            "msgxy_at<value, value>", 
            (x, y) =>
            {
                ControllerEnd.instance.IsOccupiedGrid(x, y);
            });

        ERASE_AT = new WindowsCommand<int, int>(
            "erase_atxy", 
            "erase the object at x and y",
            "place_at<value, value>",
            (x, y) =>
            {
                ControllerEnd.instance.EraseObject(x, y);
            });

        PLACEA_PESTKILL = new WindowsCommand<int, int>(
            "antipest_atxy",
            "place the instrument to rid the pest",
            "placepestkill_atxy <value, value>",
            (x, y) =>
            {
                ControllerEnd.instance.PlacePestKiller(x, y);
            });
        


        commandList = new List<object>
        {
            SEND_DEBUG,
            SET_DEBUG,
            PLANT_AT, 
            ERASE_AT,
            PLACEA_PESTKILL
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
                if (commandList[i] is WindowsCommand simpleCommand) // handle only command
                {
                    simpleCommand.Invoke();
                }
                
                else if (commandList[i] is WindowsCommand<int> intCommand) // handle one integer input, ex : command value
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

                else if (commandList [i] is WindowsCommand<int, int> intPairCommand) // handle two integer input, ex : command val1 val2
                {
                    if (properties.Length < 3)
                    {
                        Debug.Log("Butuh angka!");
                        return;
                    }

                    if (!int.TryParse(properties[1], out int val1) || 
                    !int.TryParse(properties[2], out int val2))
                    {
                        Debug.Log("Format angka salah!");
                        return;
                    }
                    

                    intPairCommand.Invoke(val1, val2);
                }
            }
        }
        input = ""; // set input string to clear
    }


    void OnGUI()
    {

        if (style == null)
        {
            style = new GUIStyle(GUI.skin.textField);
            style.fontSize = 20;
        }

        GUI.SetNextControlName("ConsoleInput");

        // the text input goes here
        input = GUI.TextField(
            new Rect(Screen.width / 2 + xPadding, yPadding, Screen.width / 2 - width , 40),
            input, style
        );
        GUI.FocusControl("ConsoleInput");

        // input handler call
        if (Event.current.isKey && Event.current.keyCode == KeyCode.Return) // if iskey and return pressed
        {
            HandleInput();
        }
    }
}