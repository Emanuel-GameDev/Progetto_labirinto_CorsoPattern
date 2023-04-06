using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid data")]
    [SerializeField] int _width;
    [SerializeField] int _height;

    [Header("Grid Tiles")]
    [SerializeField] private TerrainTile terrailTile;
    [SerializeField] private WallTile wallTile;



    private Transform _cam;
    private Dictionary<Vector2 , Tile> _tiles;

    private void Start()
    {
        _cam = Camera.main.transform;

        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Tile randomTile = Random.Range(0, 6) < 2  ? wallTile : terrailTile;
                Tile tile = Instantiate(randomTile, new Vector2(x, y), Quaternion.identity);

                tile.transform.parent = transform;
                tile.name = $"Tile ({x} - {y})";

                tile.Init(x, y);

                //_tiles[new Vector2 (x, y)] = tile;
            }
        }

        _cam.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10f);
    }

    public Tile GetTileAtPos(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out Tile tile))
            return tile;
        else
            return null;
    }
}
