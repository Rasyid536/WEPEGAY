using UnityEngine;
using System.Collections.Generic;
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
public partial class Controller : MonoBehaviour
{
    // Struktur split Controller:
    // - File ini: state inti, lifecycle (`Awake`, `Start`, `Update`), dan helper bersama.
    // - `Controller.Commands.cs`: inisialisasi command + proses input command.
    // - `Controller.Gui.cs`: tampilan `OnGUI`, caret, dan shortcut keyboard.
    // - `Controller.Autocomplete.cs`: highlight input, autocomplete, dan highlight tile.
    private const string ConsoleControlName = "ConsoleInput";
    private const int GridSize = 8;
    private const float LineHeight = 30f;
    private const float CaretWidth = 5f;
    private const float CaretHeight = 24f;
    private const float CaretOffsetX = -1f;
    private const float CaretOffsetY = -22f;
    private const float CaretBlinkSpeed = 2f;
    private static readonly Color HiddenTextColor = new Color(1f, 1f, 1f, 0f);
    private static readonly Color CaretVisibleColor = new Color(1f, 0.5f, 0f, 1f);
    private static readonly Color CaretHiddenColor = Color.black;
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
    private GUIStyle textLogsStyle;
    [SerializeField] private float yPadding, xPadding, width; // gui margin
    private GUIStyle style;
    private string input = "";
    private bool forceCursorToEnd = false;
    private string suggestionText;
    [SerializeField] private GameObject uiAdjust;
    private float caretBlink;
    private Tile lastHighlightedTile = null;
    void Awake()
    {
        InitializeCommands();
    }
    void Start()
    {
        AddTextLogs("type<color=yellow> 'help'</color> for list of command, and type<color=yellow> 'start' </color>to start the game");
    }
    void Update()
    {
        // Input diproses di OnGUI supaya tidak dobel saat event keyboard berjalan.
    }
    private void ResetInputState()
    {
        input = "";
        lastInput = "";
        suggestionText = "";
        ClearHighlightedTile();
    }
    private void ClearHighlightedTile()
    {
        if (lastHighlightedTile == null)
        {
            return;
        }
        lastHighlightedTile.unHighLight();
        lastHighlightedTile = null;
    }
    private void AddTextLogs(string message)
    {
        message = FormatLogMessage(message);
        terminalLogs.Insert(0, message);
        if (terminalLogs.Count >= maxLogs)
        {
            terminalLogs.RemoveAt(0);
        }
    }
    private string FormatLogMessage(string message)
    {
        message = message.Replace("plant_atxy", "<color=orange>plant_atxy</color>");
        message = message.Replace("erase_atxy", "<color=orange>erase_atxy</color>");
        message = message.Replace("antipest_atxy", "<color=orange>antipest_atxy</color>");
        message = message.Replace("add_money", "<color=orange>add_money</color>");
        message = message.Replace("3 3", "<color=#ADD8E6>3 3</color>");
        message = message.Replace("x y", "<color=#ADD8E6>x y</color>");
        message = message.Replace("help :", "<color=#8CC7C4>help :</color>");
        message = message.Replace("'", "<color=#F79A19>'</color>");
        return message;
    }
}
