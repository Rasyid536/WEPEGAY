using UnityEngine;

public partial class Controller
{
    // Bagian ini dipindah dari `Controller.cs` asli:
    // - `OnGUI()`
    // - style GUI
    // - shortcut keyboard
    // - caret dan daftar log terminal
    void OnGUI()
    {
        EnsureStyles();

        float areaWidth = Screen.width / 2 - width;
        float xPos = Screen.width / 2 + xPadding;
        float inputY = yPadding;

        HandleConsoleShortcuts(Event.current);
        DrawInputArea(xPos, inputY, areaWidth);
        DrawCaret();
        ApplyPendingCursorMove();
        DrawLogs(xPos, inputY, areaWidth);
    }

    // Setup style visual untuk input dan log.
    private void EnsureStyles()
    {
        if (style == null)
        {
            style = new GUIStyle(GUI.skin.label);
            style.fontSize = 20;
            style.normal.background = Texture2D.blackTexture;
            style.normal.textColor = HiddenTextColor;
            style.focused.textColor = HiddenTextColor;
            style.border = new RectOffset(0, 0, 0, 0);
        }

        if (textLogsStyle == null)
        {
            textLogsStyle = new GUIStyle(GUI.skin.label);
            textLogsStyle.fontSize = 20;
            textLogsStyle.normal.textColor = Color.white;
            textLogsStyle.richText = true;
        }
    }

    // Handle Enter dan autocomplete RightArrow.
    private void HandleConsoleShortcuts(Event currentEvent)
    {
        if (currentEvent.type != EventType.KeyDown)
        {
            return;
        }

        if (currentEvent.keyCode == KeyCode.Return)
        {
            if (!string.IsNullOrEmpty(input))
            {
                HandleInput();
                currentEvent.Use();
            }
        }
        else if (currentEvent.keyCode == KeyCode.RightArrow)
        {
            AcceptSuggestion(currentEvent);
        }
    }

    // Menambahkan suggestion ke input saat autocomplete dipakai.
    private void AcceptSuggestion(Event currentEvent)
    {
        if (string.IsNullOrEmpty(suggestionText))
        {
            return;
        }

        input += suggestionText + " ";
        forceCursorToEnd = true;
        currentEvent.Use();
    }

    // Render area input + update real-time highlight saat teks berubah.
    private void DrawInputArea(float xPos, float inputY, float areaWidth)
    {
        GUI.Label(new Rect(xPos, inputY, areaWidth, LineHeight), GetHighlightedInput(), textLogsStyle);

        GUI.SetNextControlName(ConsoleControlName);
        input = GUI.TextField(new Rect(xPos, inputY, areaWidth, LineHeight), input, style);
        GUI.FocusControl(ConsoleControlName);

        if (input != lastInput)
        {
            CheckRealTimeInput();
            lastInput = input;
        }
    }

    // Render caret custom supaya efek blink tetap terlihat.
    private void DrawCaret()
    {
        caretBlink += Time.deltaTime;
        bool showCaret = (int)(caretBlink * CaretBlinkSpeed) % 2 == 0;

        TextEditor textEditor = GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl) as TextEditor;
        if (textEditor == null)
        {
            return;
        }

        Vector2 cursorPos = textEditor.graphicalCursorPos;
        Rect caretRect = new Rect(
            cursorPos.x + CaretOffsetX,
            cursorPos.y + CaretOffsetY,
            CaretWidth,
            CaretHeight);

        Color previousColor = GUI.color;
        GUI.color = showCaret ? CaretVisibleColor : CaretHiddenColor;
        GUI.DrawTexture(caretRect, Texture2D.whiteTexture);
        GUI.color = previousColor;
    }

    // Memaksa cursor pindah ke akhir setelah autocomplete.
    private void ApplyPendingCursorMove()
    {
        if (!forceCursorToEnd || Event.current.type != EventType.Repaint)
        {
            return;
        }

        TextEditor textEditor = GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl) as TextEditor;
        if (textEditor != null)
        {
            textEditor.cursorIndex = input.Length;
            textEditor.selectIndex = input.Length;
        }

        forceCursorToEnd = false;
    }

    // Render semua log terminal di bawah input.
    private void DrawLogs(float xPos, float inputY, float areaWidth)
    {
        float startY = inputY + LineHeight;

        for (int i = 0; i < terminalLogs.Count; i++)
        {
            float yPos = startY + (i * LineHeight);
            GUI.Label(new Rect(xPos, yPos, areaWidth, LineHeight), terminalLogs[i], textLogsStyle);
        }
    }
}

