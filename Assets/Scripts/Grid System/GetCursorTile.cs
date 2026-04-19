using UnityEngine;


public static class getTilePointedCursor
{
    public static string cursorAt;
}

public class GetCursorTile : MonoBehaviour
{
    public int gridWidth = 8; 
    public int gridHeight = 8;

    void Update()
    {
        string hoveredTile = GetHoveredTileString();

        if(GridManager.instance.isGridGenerated)
            getTilePointedCursor.cursorAt = hoveredTile;

    }

    private string GetHoveredTileString()
    {
        // 1. Ambil posisi pixel mouse dulu
        Vector3 mouseScreenPos = Input.mousePosition;

        // 2. KUNCI UTAMA: Beri tahu Unity jarak dari kamera ke grid (kedalaman Z)
        // Kita ambil jarak absolut dari posisi Z kamera saat ini (misal dari -10 jadi 10)
        mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z);

        // 3. Baru konversi ke World Space
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);


        int x = Mathf.RoundToInt(mouseWorldPos.x);
        int y = Mathf.RoundToInt(mouseWorldPos.y);


        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
            return $" <color=#309898>x {x} y {y}</color>";
        else
            return "<color=#E52020>null</color>"; 
    }
}