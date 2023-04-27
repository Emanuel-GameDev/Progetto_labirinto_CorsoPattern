using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTurnState : State
{
    private GameManager gameManager;

    private List<BaseEnemy> _enemies = new List<BaseEnemy>();
    private int turnNum = 0;

    public EnemyTurnState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public override void OnEnter()
    {
        Debug.Log($"Enemy turn --- {turnNum}");

        // Prendo le ref a tutti gli enemy
        _enemies = GameObject.FindObjectsOfType<BaseEnemy>(true).ToList();

        PerformTurn();
    }

    public override void OnExit()
    {
        turnNum++;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    private void MoveEnemy(BaseEnemy enemy, Tile dest)
    {
        if (dest is WallTile) return;

        // Mi muovo
        enemy.transform.position = dest.transform.position;

        // Controllo hero
        CheckHero(enemy, dest);

        // Setup tile e unità occupate
        enemy.occupiedTile.occupiedUnit = null;
        enemy.occupiedTile = dest;
        dest.occupiedUnit = enemy;

    }

    private void CheckHero(BaseEnemy enemy, Tile dest)
    {
        if (dest.occupiedUnit != null && dest.occupiedUnit is BaseHero)
        {
            Debug.Log("You are DEAD");
            gameManager.Respawn();
        }
    }

    internal void PerformTurn()
    {
        // Faccio il turno per ogni enemy
        foreach (BaseEnemy enemy in _enemies)
        {
            // Chiamo funzione per avere tile adiacenti
            Dictionary<Vector2, Tile> adjacentTiles = new Dictionary<Vector2, Tile>();
            adjacentTiles = gameManager.gridManager.GetAdjacentTiles(enemy.occupiedTile);

            // Filtro i tile adiacenti in base a quelli
            foreach (Vector2 key in adjacentTiles.Keys.ToList())
            {
                // Rimuovo se è un muro, non se è un nemico
                if (adjacentTiles[key] is WallTile)
                {
                    adjacentTiles.Remove(key);
                }
            }

            // Faccio muovere enemy
            if (adjacentTiles.Count >= 1)
            {
                List<Tile> values = new List<Tile>(adjacentTiles.Values);
                MoveEnemy(enemy, values[Random.Range(0, values.Count)]);
            }
        }

        // Cambio stato
        gameManager.StateMachine.SetState(GameState.HeroTurn);
    }
}
