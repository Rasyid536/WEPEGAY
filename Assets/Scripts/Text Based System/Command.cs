using System;
using UnityEngine;

// ============================================= //
//        This is the script where the game      //
//      command prompt system is processed       //
//      and controlled.                          //
//                                               //
//  Note : not fully understand this yet.        //
// ============================================= //


// encapsulation as far as i know
public class WindowsCommandBase
{
    private string _commandID;
    private string _commandDescription;
    private string _commandFormat;

    public string commandID { get { return _commandID; } }
    public string commandDescription { get { return _commandDescription; } }
    public string commandFormat { get { return _commandFormat; } }

    public WindowsCommandBase(string id, string description, string format)
    {
        _commandID = id;
        _commandDescription = description;
        _commandFormat = format;
    }
}

public class WindowsCommand : WindowsCommandBase
{
    private Action command;

    public WindowsCommand(string id, string description, string format, Action command) 
        : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}