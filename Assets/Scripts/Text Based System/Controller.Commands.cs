using UnityEngine;
using System.Collections.Generic;

public partial class Controller
{
    // Bagian ini dipindah dari `Controller.cs` asli:
    // - registrasi semua command
    // - parsing input
    // - eksekusi command berdasarkan jumlah argumen
    private void InitializeCommands()
    {
        ADJUSTER = new WindowsCommand(
            "adjusteron",
            "adjust the visual",
            "adjsuteron(no more arg)",
            () =>
            {
                uiAdjust.SetActive(true);
            });

        ADD_MONEY = new WindowsCommand<int>(
            "add_money",
            "Adding money",
            "add_money <Amount>",
            (x) =>
            {
                AddTextLogs($"<color=green>ad money with value : {x}</color>");
                GlobalData.money += x;
            });

        HELP = new WindowsCommand(
            "help",
            "show command available",
            "help",
            () =>
            {
                AddTextLogs("help : for cheat type 'add_money <color=#ADD8E6>v</color>', to set money at v much amount");
                AddTextLogs("help : for example 'plant_atxy 3 3' will place plant at grid column 3 and grid line 3");
                AddTextLogs("help : the x and y is correspond to the coordinate respectively");
                AddTextLogs("help : type 'erase_atxy x y' to erase object, you could harvest with this command");
                AddTextLogs("help : type 'antipest_atxy x y' to spawn pest killer, pest around this object would destroy");
                AddTextLogs("help : type 'plant_atxy x y' to spawn plant you can use to get money ");
            });

        START = new WindowsCommand(
            "start",
            "start the game",
            "start(no more arg)",
            () =>
            {
                Time.timeScale = 1;
            });

        EXIT = new WindowsCommand(
            "exit",
            "exit the game",
            "exit(no more arg)",
            () =>
            {
                Application.Quit();
            });

        SEND_DEBUG = new WindowsCommand(
            "send_debug",
            "Sending message",
            "send_debug",
            () =>
            {
                ControllerEnd.instance.Debugs();
                AddTextLogs("<color=orange>debug sent</color>");
            });

        SET_DEBUG = new WindowsCommand<int>(
            "set_debug",
            "set the value of debug message",
            "set_debug<amount>",
            (x) =>
            {
                ControllerEnd.instance.DebugVal(x);
                AddTextLogs("<color=orange>debug at val </color><color=#239BA7>" + x + "</color>");
            });

        PLANT_AT = new WindowsCommand<int, int>(
            "plant_atxy",
            "send you the x and y",
            "msgxy_at<value, value>",
            (x, y) =>
            {
                if (GlobalData.money >= 1)
                {
                    ControllerEnd.instance.IsOccupiedGrid(x, y);
                    AddTextLogs($"<color=#2FA084>command received :</color> plant_atxy<color=#239BA7> {x} {y}, retrieving data for {UnityEngine.Random.Range(0.023f, 0.098f)}s");
                }
            });

        ERASE_AT = new WindowsCommand<int, int>(
            "erase_atxy",
            "erase the object at x and y",
            "place_at<value, value>",
            (x, y) =>
            {
                ControllerEnd.instance.EraseObject(x, y);
                AddTextLogs($"<color=#2FA084>command received :</color> erase_atxy<color=#239BA7> {x} {y}, retrieving data for {UnityEngine.Random.Range(0.023f, 0.098f)}s");
            });

        PLACEA_PESTKILL = new WindowsCommand<int, int>(
            "antipest_atxy",
            "place the instrument to rid the pest",
            "placepestkill_atxy <value, value>",
            (x, y) =>
            {
                if (GlobalData.money >= 3)
                {
                    ControllerEnd.instance.PlacePestKiller(x, y);
                    GlobalData.money -= 3;
                    AddTextLogs($"<color=#2FA084>command received :</color> antipest_atxy<color=#239BA7> {x} {y}, retrieving data for {UnityEngine.Random.Range(0.023f, 0.098f)}s");
                }
            });

        terminalLogs = new List<string>();
        commandList = new List<object>
        {
            SEND_DEBUG,
            SET_DEBUG,
            PLANT_AT,
            ERASE_AT,
            PLACEA_PESTKILL,
            ADD_MONEY,
            HELP,
            START,
            EXIT,
            ADJUSTER
        };
    }

    // Dipanggil saat user menekan Enter di `Controller.Gui.cs`.
    private void HandleInput()
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            ResetInputState();
            return;
        }

        string[] properties = input.Split(' ');
        bool commandHandled = TryExecuteCommand(properties);

        if (!commandHandled)
        {
            AddTextLogs($"<color=red>no command such : {properties[0]}, u may be refer into something else</color>");
        }

        ResetInputState();
    }

    // Mengecek apakah command valid dan menjalankan command yang cocok.
    private bool TryExecuteCommand(string[] properties)
    {
        if (properties == null || properties.Length == 0)
        {
            return false;
        }

        string commandId = properties[0];

        for (int i = 0; i < commandList.Count; i++)
        {
            WindowsCommandBase commandBase = commandList[i] as WindowsCommandBase;
            if (commandBase == null || commandBase.commandID != commandId)
            {
                continue;
            }

            if (commandList[i] is WindowsCommand simpleCommand)
            {
                simpleCommand.Invoke();
                return true;
            }

            if (commandList[i] is WindowsCommand<int> intCommand)
            {
                if (properties.Length < 2)
                {
                    AddTextLogs("<color=red>Butuh angka!</color>");
                    return true;
                }

                if (!int.TryParse(properties[1], out int value))
                {
                    AddTextLogs("<color=red>Format angka salah!</color>");
                    return true;
                }

                intCommand.Invoke(value);
                return true;
            }

            if (commandList[i] is WindowsCommand<int, int> intPairCommand)
            {
                if (properties.Length < 3)
                {
                    AddTextLogs("<color=red>Butuh angka!</color>");
                    return true;
                }

                if (!int.TryParse(properties[1], out int val1) || !int.TryParse(properties[2], out int val2))
                {
                    AddTextLogs("<color=red>Format angka salah!</color>");
                    return true;
                }

                intPairCommand.Invoke(val1, val2);
                return true;
            }

            return false;
        }

        return false;
    }
}

