using UnityEngine;

public partial class Controller
{
    // Bagian ini dipindah dari `Controller.cs` asli:
    // - highlight teks input
    // - suggestion/autocomplete
    // - validasi angka input
    // - highlight tile real-time
    private string GetHighlightedInput()
    {
        suggestionText = "";

        if (string.IsNullOrWhiteSpace(input))
        {
            return "";
        }

        string[] properties = input.Split(' ');
        string commandPart = properties[0];
        bool isValidCommand = TryResolveCommand(commandPart, properties.Length == 1);

        string result = isValidCommand
            ? $"<color=#F79A19>{commandPart}</color>"
            : $"<color=#FF5555>{commandPart}</color>";

        for (int i = 1; i < properties.Length && i <= 2; i++)
        {
            result = AppendHighlightedProperty(result, properties[i]);
        }

        if (input.EndsWith(" "))
        {
            result += " ";
        }

        if (!string.IsNullOrEmpty(suggestionText))
        {
            result += $"<color=#808080>{suggestionText}</color>";
        }

        return result;
    }

    // Cek command valid dan siapkan suggestion bila user baru mengetik sebagian.
    private bool TryResolveCommand(string commandPart, bool allowSuggestion)
    {
        bool isValidCommand = false;

        for (int i = 0; i < commandList.Count; i++)
        {
            WindowsCommandBase commandBase = commandList[i] as WindowsCommandBase;
            if (commandBase == null)
            {
                continue;
            }

            if (commandBase.commandID == commandPart)
            {
                isValidCommand = true;
                break;
            }

            if (allowSuggestion && commandBase.commandID.StartsWith(commandPart))
            {
                suggestionText = commandBase.commandID.Substring(commandPart.Length);
                break;
            }
        }

        return isValidCommand;
    }

    // Warnai argumen angka agar mudah dibaca di prompt.
    private string AppendHighlightedProperty(string currentText, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return currentText + " ";
        }

        if (!int.TryParse(value, out int num))
        {
            return currentText + $" <color=#FF5555>{value}</color>";
        }

        return num <= GridSize - 1
            ? currentText + $" <color=#ADD8E6>{value}</color>"
            : currentText + $" <color=#FF5555>{value}</color>";
    }

    // Highlight tile secara real-time saat input cocok dengan format plant_atxy x y.
    private void CheckRealTimeInput()
    {
        string[] properties = input.Split(' ');

        if (properties.Length < 3 || properties[0] != "plant_atxy" ||
            !int.TryParse(properties[1], out int x) || !int.TryParse(properties[2], out int y))
        {
            ClearHighlightedTile();
            return;
        }

        if (x < 0 || x >= GridSize || y < 0 || y >= GridSize)
        {
            ClearHighlightedTile();
            return;
        }

        Vector2Int targetPos = new Vector2Int(x, y);

        if (!Tile.AllTiles.ContainsKey(targetPos))
        {
            ClearHighlightedTile();
            return;
        }

        Tile targetTile = Tile.AllTiles[targetPos];

        if (lastHighlightedTile != null && lastHighlightedTile != targetTile)
        {
            lastHighlightedTile.unHighLight();
        }

        targetTile.highLight();
        lastHighlightedTile = targetTile;
    }
}

