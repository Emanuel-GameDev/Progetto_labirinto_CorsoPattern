using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid data")]
    [SerializeField] int _width;
    [SerializeField] int _height;

    [Header("Grid Tiles")]
    [SerializeField] private TerrainTile terrailTile;
    [SerializeField] private WallTile wallTile;
    [SerializeField] private GoalTile goalTile1;
    [SerializeField] private GoalTile goalTile2;

    private Transform _cam;
    private Dictionary<Vector2 , Tile> _tiles = new Dictionary<Vector2, Tile>();
    private GoalTile goalTile;

    private void Awake()
    {
        _cam = Camera.main.transform;
        PubSub.Instance.RegisterFunction("GameStarted", GenerateGrid);
    }

    private void GenerateGrid(object obj)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Tile randomTile = Random.Range(0f, 6f) < 1.62  ? wallTile : terrailTile;
                Tile tile = Instantiate(randomTile, new Vector2(x, y), Quaternion.identity);

                tile.transform.parent = transform;
                tile.name = $"Tile ({x} - {y})";

                tile.Init(x, y);

                _tiles[new Vector2 (x, y)] = tile;
            }
        }

        _cam.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10f);

        CreateGoalTile();
    }

    private void CreateGoalTile()
    {
        // Genera un indice casuale
        int randomIndex = Random.Range(0, _tiles.Count);

        // Prende la chiave corrispondente all'indice casuale
        Vector2 randomKey = _tiles.Keys.ElementAt(randomIndex);

        // Rimuove l'elemento dal dizionario
        _tiles.Remove(randomKey);

        // Aggiunge un nuovo elemento con la stessa chiave ma un valore diverso
        Tile goal = Instantiate((randomIndex < (_tiles.Count/2) ? goalTile1 : goalTile2), new Vector2(randomKey.x, randomKey.y), Quaternion.identity);
        _tiles.Add(randomKey, goal);
        goal.transform.parent = transform;
        goal.name = (goalTile1 ? $"GoalTile ({(int)randomKey.x} - {(int)randomKey.y})" : $"GoalTile1 ({randomKey.x} - {randomKey.y})");
        goal.Init((int)randomKey.x, (int)randomKey.y);
        goalTile = goal as GoalTile;
    }

    public GoalTile GetGoalTile()
    {
        if (goalTile != null)
            return goalTile;

        return null;
    }

    public Tile GetHeroTile()
    {
        // Prendo un tile dalla lista di tile solo dalla metà verso sinistra solo se è camminabile
        // Ne prendo uno a caso e restituisco il valore
        return _tiles.Where(t => t.Key.x < _width/2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }

    public Tile GetEnemyTile()
    {
        return _tiles.Where(t => t.Key.x > _width/2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }

    public Tile GetTileAtPos(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out Tile tile))
            return tile;
        else
            return null;
    }

    public Tile GetRandomTile()
    {
        // Genera un indice casuale
        int randomIndex = Random.Range(0, _tiles.Count);

        // Prende la chiave corrispondente all'indice casuale
        Vector2 randomKey = _tiles.Keys.ElementAt(randomIndex);

        return _tiles[randomKey];
    }

    public Dictionary<Vector2, Tile> GetAdjacentTiles(Tile selectedTile)
    {
        if (!_tiles.ContainsValue(selectedTile)) return null;

        Dictionary<Vector2, Tile> _adjacentTiles = new Dictionary<Vector2, Tile>();

        List<string> list = new List<string>();
        list = selectedTile.name.Split(new char[] { '-', '(', ')', ' ' }, System.StringSplitOptions.RemoveEmptyEntries).ToList();
        float x = float.Parse(list[1]);
        float y = float.Parse(list[2]);

        Vector2 tmp = new Vector2(x, y);

        foreach (Vector2 key in _tiles.Keys)
        {
            if (key == new Vector2(tmp.x+1, tmp.y))
                _adjacentTiles.Add(key, _tiles[key]);
            else if (key == new Vector2(tmp.x-1, tmp.y))
                _adjacentTiles.Add(key, _tiles[key]);
            else if (key == new Vector2(tmp.x, tmp.y+1))
                _adjacentTiles.Add(key, _tiles[key]);
            else if (key == new Vector2(tmp.x, tmp.y-1))
                _adjacentTiles.Add(key, _tiles[key]);

        }

        return _adjacentTiles;

    }
}
