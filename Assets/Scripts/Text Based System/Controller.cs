using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System;

// ============================================= //
//        This is the script where the game      //
//      command prompt system is processed       //
//      and controlled.                          //
// ============================================= //

//======== color picker ========//
// #ffff      pick color on this one, dont modify the others below
// #2FA084
// #ADD8E6
// #239BA7
// #808080

public class Controller : MonoBehaviour
{

    // declare command list variable 
    public static WindowsCommand SEND_DEBUG;
    public static WindowsCommand<int> SET_DEBUG;
    public static WindowsCommand<int, int> PLANT_AT;
    public static WindowsCommand<int, int> ERASE_AT;
    public static WindowsCommand<int, int> PLACEA_PESTKILL;
    public static WindowsCommand<int> ADD_MONEY;
    public static WindowsCommand HELP, START, EXIT, ADJUSTER;
    // declare command list variable


    public List<object> commandList;
    private List<string> terminalLogs;
    private string lastInput = "";
    [SerializeField] private int maxLogs;
    GUIStyle textLogsStyle;
    [SerializeField]private float yPadding, xPadding, width; // gui margin
    GUIStyle style; string input = "";
    bool forceCursorToEnd = false;
    string suggestionText; [SerializeField] GameObject uiAdjust;
    float caretBlink; Texture2D caretTex;
    Tile lastHighlightedTile = null;

    void Awake()
    {

        ADJUSTER = new WindowsCommand(
            "adjusteron",
            "adjust the visual",
            "adjsuteron(no more arg)",
            () =>
            {
                uiAdjust.SetActive(true);
            });

        ADD_MONEY = new WindowsCommand<int>( // Developer test feature, don't use for cheating dawg >:O
            "add_money",
            "Adding money",
            "add_money <Amount>",
            (x) =>
            {
                AddTextLogs ($"<color/=green>ad money with value : {x}");
                GlobalData.money += x;
            });

        HELP = new WindowsCommand(
            "help",
            "show command available",
            "help",
            () =>
            {
                AddTextLogs ("help : for cheat type 'add_money <color=#ADD8E6>v</color>', to set money at v much amount");
                AddTextLogs ("help : for example 'plant_atxy 3 3' will place plant at grid column 3 and grid line 3");
                AddTextLogs ("help : the x and y is correspond to the coordinate respectively");
                AddTextLogs ("help : type 'erase_atxy x y' to erase object, you could harvest with this command");
                AddTextLogs ("help : type 'antipest_atxy x y' to spawn pest killer, pest around this object would destroy");
                AddTextLogs ("help : type 'plant_atxy x y' to spawn plant you can use to get money ");
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
                AddTextLogs("<color=orange>debug at val </color><color=#239BA7>" + x);
            });


        PLANT_AT = new WindowsCommand<int, int>(
            "plant_atxy", 
            "send you the x and y", 
            "msgxy_at<value, value>", 
            (x, y) =>
            {
                if(GlobalData.money >= 1)
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
                if(GlobalData.money >= 3)
                {
                    ControllerEnd.instance.PlacePestKiller(x, y);
                    GlobalData.money -= 3;
                    AddTextLogs($"<color=#2FA084>command received :</color> antipest_atxy<color=#239BA7> {x} {y}, retrieving data for {UnityEngine.Random.Range(0.023f, 0.098f)}s");
                }
            });
        
        terminalLogs = new List<string>{};


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

    void Start()
    {
        AddTextLogs("type<color=yellow> 'help'</color> for list of command, and type<color=yellow> 'start' </color>to start the game");
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
                break;
            }

            if(i == commandList.Count - 1 && properties[0] != commandBase.commandID)
            {
                AddTextLogs($"<color=red>no command such : {properties[0]}, u may be refer into something else</color>");
            }
        }
        input = "";
        if (lastHighlightedTile != null)
        {
            lastHighlightedTile.unHighLight();
            lastHighlightedTile = null;
        }

    }

    void AddTextLogs(string message)
    {
        message = message.Replace("plant_atxy", "<color=orange>plant_atxy</color>");
        message = message.Replace("erase_atxy", "<color=orange>erase_atxy</color>");
        message = message.Replace("antipest_atxy", "<color=orange>antipest_atxy</color>");
        message = message.Replace("add_money", "<color=orange>add_money</color>");
        message = message.Replace("3 3", "<color=#ADD8E6>3 3</color>");
        message = message.Replace("x y", "<color=#ADD8E6>x y</color>");
        message = message.Replace("help :", "<color=#8CC7C4>help :</color>");
        message = message.Replace("'", "<color=#F79A19>'</color>");
        terminalLogs.Insert(0, message);
        if (terminalLogs.Count >= maxLogs)
        {
            terminalLogs.RemoveAt(0);
        }
    }

    void OnGUI() 
    {
        // ================= STYLE =================
        if (style == null)
        {
            // UBAH: Pakai textField agar behavior ketikannya (kursor, spasi) normal
            style = new GUIStyle(GUI.skin.label); // style? what for? idk honestly
            style.fontSize = 20;

            style.normal.background = Texture2D.blackTexture;
            
            // UBAH: Ini trik transparannya. Angka 0 di akhir adalah Alpha (A).
            // Ini membuat teks asli yang kita ketik menghilang, tapi kursor tetap ada.
            style.normal.textColor = new Color(1, 1, 1, 0); 
            style.focused.textColor = new Color(1, 1, 1, 0);

            style.border = new RectOffset(0, 0, 0, 0);
        }

        if (textLogsStyle == null)
        {
            textLogsStyle = new GUIStyle(GUI.skin.label);
            textLogsStyle.fontSize = 20;
            textLogsStyle.normal.textColor = Color.white;
        
            textLogsStyle.richText = true; 
        }

        float areaWidth = Screen.width / 2 - width;
        float xPos = Screen.width / 2 + xPadding;

        // ================= INPUT (FIXED DI ATAS) =================
        float inputY = yPadding;

        if (Event.current.type == EventType.KeyDown)
        {
            if (Event.current.keyCode == KeyCode.Return)
            {
                if (!string.IsNullOrEmpty(input)) 
                {
                    HandleInput();
                    Event.current.Use(); // Konsumsi event
                }
            }
            else if (Event.current.keyCode == KeyCode.RightArrow)
            {
                if (!string.IsNullOrEmpty(suggestionText))
                {
                    input += suggestionText + " "; 
                    
                    // UBAH: Cukup nyalakan benderanya, urusan pindah kursor belakangan
                    forceCursorToEnd = true; 
                    
                    Event.current.Use(); 
                }
            }
        }

        // rect GUI label
        GUI.Label(new Rect(xPos, inputY, areaWidth, 30), GetHighlightedInput(), textLogsStyle);

        // TETAP: Ini adalah "Layer Atas". TextField ini teksnya transparan (karena Langkah 2),
        // dia bertugas murni untuk menangkap ketikan keyboard saja.
        GUI.SetNextControlName("ConsoleInput");
        input = GUI.TextField(
            new Rect(xPos, inputY, areaWidth, 30),
            input,
            style
        );

        GUI.SetNextControlName("ConsoleInput");

        input = GUI.TextField(
            new Rect(xPos, inputY, areaWidth, 30),
            input,
            style
        );

        GUI.FocusControl("ConsoleInput");

        if (input != lastInput) 
        {
            CheckRealTimeInput(); // Panggil fungsi pengecek angka
            lastInput = input;    // Simpan ketikan terakhir
        }

        TextEditor textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
        caretBlink += Time.deltaTime;
        bool showCaret = (int)(caretBlink * 2) % 2 == 0;
        Vector2 cursorPos = textEditor.graphicalCursorPos;
        
        Rect caretRect = new Rect(
                cursorPos.x - 1,
                cursorPos.y - 22,
                5, // caret thickness
                24
            );
        
        if (!showCaret)
        {
            caretTex = new Texture2D(1, 1);
            caretTex.SetPixel(0, 0, new Color(0f, 0f, 0f)); // orange
            caretTex.Apply();
            GUI.DrawTexture(caretRect, caretTex);
        }
        else
        {
            caretTex = new Texture2D(1, 1);
            caretTex.SetPixel(0, 0, new Color(1f, 0.5f, 0f)); // black
            caretTex.Apply();
            GUI.DrawTexture(caretRect, caretTex);
        }


        if (forceCursorToEnd && Event.current.type == EventType.Repaint)
        {
            TextEditor te = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
            if (te != null)
            {
                te.cursorIndex = input.Length;  // force cursor to end of the text
                te.selectIndex = input.Length;  // Paksa ekor kursor ke ujung (lepas blok)
            }
            forceCursorToEnd = false; // Matikan bendera setelah berhasil
        }

        // ================= LOGS (DI BAWAH INPUT) =================
        float startY = inputY + 30; // jarak dari input

        for (int i = 0; i < terminalLogs.Count; i++)
        {
            float yPos = startY + (i * 30);
            GUI.Label(new Rect(xPos, yPos, areaWidth, 30), terminalLogs[i], textLogsStyle);
        }

        // ================= ENTER =================
        if (Event.current.isKey && Event.current.keyCode == KeyCode.Return)
        {
            if (!string.IsNullOrEmpty(input)) 
            {
                HandleInput();
                Event.current.Use(); 
            }
        }
        
        // autocomplete system
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.RightArrow)
        {
            if (!string.IsNullOrEmpty(suggestionText))
            {
                input += suggestionText + " "; 
                
                TextEditor editorText = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
                
                if (editorText != null)
                {
                    editorText.cursorIndex = input.Length;
                    editorText.selectIndex = input.Length; // Pastikan tidak ada teks yang ter-blok
                }

                Event.current.Use(); 
            }
        }

    }

    string GetHighlightedInput()
    {
        suggestionText = ""; 

        if (string.IsNullOrEmpty(input)) return "";

        string[] properties = input.Split(' ');
        string result = "";

        string commandPart = properties[0];
        bool isValidCommand = false;
    
        if (properties.Length == 1) // Hanya cari suggestion kalau user baru ngetik 1 kata
        {
            foreach (var cmd in commandList)
            {
                string cmdID = (cmd as WindowsCommandBase).commandID;
                
                if (cmdID == commandPart)
                {
                    isValidCommand = true;
                    break;
                }
                // Jika user ngetik "pla" dan ada command "plant_atxy"
                else if (cmdID.StartsWith(commandPart)) 
                {
                    // Ambil sisa hurufnya saja (misal: "nt_atxy")
                    suggestionText = cmdID.Substring(commandPart.Length); 
                    break; // Ambil suggestion pertama yang ketemu
                }
            }
        }
        else // Kalau user sudah spasi dan masukin argumen, asumsikan command (kata pertama) sudah valid (atau setidaknya tidak di-autocomplete lagi)
        {
            foreach (var cmd in commandList)
            {
                if ((cmd as WindowsCommandBase).commandID == commandPart)
                {
                    isValidCommand = true;
                    break;
                }
            }
        }
        
        // #F79A19
        if (isValidCommand) result += $"<color=#F79A19>{commandPart}</color>";
        // #FF5555
        else result += $"<color=#FF5555>{commandPart}</color>";

        for (int i = 1; i < properties.Length; i++)
        {
            if (properties[i] == "") { result += " "; continue; }
            
            if(i <= 2)
            {
                if (int.TryParse(properties[i], out int num))
                    if(num <= 7) // #ADD8E6
                        result += $" <color=#ADD8E6>{properties[i]}</color>"; // num color
                else // #FF5555
                    result += $" <color=#FF5555>{properties[i]}</color>";  // error color
            }  
        }

        if (input.EndsWith(" ")) result += " ";

        if (!string.IsNullOrEmpty(suggestionText))
        {
            // Hex #808080 gray
            result += $"<color=#808080>{suggestionText}</color>"; 
        }
        return result;
    }


    void CheckRealTimeInput()
    {
        string[] properties = input.Split(' ');

        if (properties.Length >= 3 && properties[0] == "plant_atxy" && 
            int.TryParse(properties[1], out int x) && int.TryParse(properties[2], out int y))
        {
            // Cek apakah angka x dan y di dalam batas map (0 sampai 7)
            if (x >= 0 && x < 8 && y >= 0 && y < 8)
            {
                Vector2Int targetPos = new Vector2Int(x, y);

                if (Tile.AllTiles.ContainsKey(targetPos))
                {
                    Tile targetTile = Tile.AllTiles[targetPos];

                    if (lastHighlightedTile != null && lastHighlightedTile != targetTile)
                    {
                        lastHighlightedTile.unHighLight(); 
                    }

                    targetTile.highLight();
                    lastHighlightedTile = targetTile; 
                }
            }
            else
            {
                // Jika angka 8 atau lebih, matikan highlight!
                if (lastHighlightedTile != null)
                {
                    lastHighlightedTile.unHighLight();
                    lastHighlightedTile = null;
                }
            }
        }
        else 
        {
            // Jika format salah atau teks dihapus (backspace)
            if (lastHighlightedTile != null)
            {
                lastHighlightedTile.unHighLight();
                lastHighlightedTile = null;
            }
        }
    }
}
