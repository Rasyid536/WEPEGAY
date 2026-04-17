using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour 
{
    public static Dictionary<Vector2, Tile> AllTiles = new Dictionary<Vector2, Tile>(); 

    [SerializeField] private Color _baseColor, _offsetColor, highLightedColor;
    [SerializeField] private SpriteRenderer _renderer;
    private bool _isOffset;

    void Awake ()
    {
        Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
        AllTiles[myPos] = this;
    }
    
    public void Init(bool isOffset) {
        _isOffset = isOffset;
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    public void highLight()
    {
        _renderer.color = highLightedColor; 
    }

    public void unHighLight()
    {
        _renderer.color = _isOffset ? _offsetColor : _baseColor;
    }
}