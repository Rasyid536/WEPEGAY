using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cam;
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
        for(int x = 0; x < _width; x++)
        {
            for(int y = 0; y < _height; y++)
            {
                var spawwnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawwnedTile.name = $"{x} {y}";

                var isOffset = (x + y ) % 2 == 1;
                spawwnedTile.Init(isOffset);
            }
        }
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 2.2f, -14.59f);
    }
}
