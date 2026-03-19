using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
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
        GlobalVariable.instance.grid = new Tile[_width, _height];
        for(int x = 0; x < _width; x++)
        {
            for(int y = 0; y < _height; y++)
            {
                var spawwnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawwnedTile.name = $"{x} {y}";

                var isOffset = (x + y) % 2 == 1;
                spawwnedTile.Init(isOffset);
                GlobalVariable.instance.grid[x, y] = spawwnedTile;
            }
        }
    }
}
