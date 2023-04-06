using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTile : Tile
{
    [SerializeField] private Color baseColor;
    [SerializeField] private Color offsetColor;

    public override void Init(int x, int y)
    {
        bool isOffset = (x + y) % 2 == 1;
        _renderer.color = isOffset ? offsetColor : baseColor;
    }
}
