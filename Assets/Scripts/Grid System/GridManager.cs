using UnityEngine;


public class GridManager : MonoBehaviour
{
    public int width, height;
    [SerializeField] private Tile _tilePrefab;
    public static GridManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        

        GlobalVariable.instance.grid = new Tile[width, height];
        GlobalVariable.instance.isOccuppied = new bool[width, height];
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                var spawwnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawwnedTile.name = $"{x} {y}";

                var isOffset = (x + y) % 2 == 1;
                spawwnedTile.Init(isOffset);
                GlobalVariable.instance.isOccuppied[x, y] = false;
                GlobalVariable.instance.grid[x, y] = spawwnedTile;
            }
        }
    }

    void DrawLabel(Vector3 worldPos, string text)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        // kalau di belakang kamera, skip
        if (screenPos.z < 0) return;

        float x = screenPos.x;
        float y = Screen.height - screenPos.y;

        GUI.Label(new Rect(x - 10, y - 10, 40, 20), text);
    }

    void OnGUI()
    {
        if (GlobalVariable.instance.grid == null) return;

        float offset = 0f;   // biar center tile
        float outside = 1f;    // jarak keluar grid

        // Label X (bawah grid)
        for (int x = 0; x < width; x++)
        {
            Vector3 worldPos = new Vector3(x + offset, -outside, 0);
            DrawLabel(worldPos, x.ToString());
        }

        // Label Y (kiri grid)
        for (int y = 0; y < height; y++)
        {
            Vector3 worldPos = new Vector3(-outside, y + offset, 0);
            DrawLabel(worldPos, y.ToString());
        }
    }
}