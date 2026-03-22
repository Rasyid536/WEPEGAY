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
}
