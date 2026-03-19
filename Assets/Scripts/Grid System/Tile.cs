using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ============================== //
//    This script taken from      //
// https://pastebin.com/UD6zE467  //
// ============================== //
 
public class Tile : MonoBehaviour {
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
 
    public void Init(bool isOffset) {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }
}