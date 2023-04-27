using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnCharactersState : State
{
    private GameManager gameManager;
    private List<ScriptableUnit> _units;

    public SpawnCharactersState(GameManager gameManager)
    {
        this.gameManager = gameManager; 
    }

    public override void OnEnter()
    {
        // Prendo le ref alle unit
        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();

        // Spawno gli hero
        SpawnHeroes();

        // Spawno gli enemy
        SpawnEnemies();

        // Cambio stato
        gameManager.StateMachine.SetState(GameState.HeroTurn);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    private void SpawnHeroes()
    {
        int heroCount = 1;

        for (int i=0; i < heroCount; i++)
        {
            BaseHero randomPrefab = GetRandomUnit<BaseHero>(Faction.Hero);
            BaseHero spawnedHero = Object.Instantiate(randomPrefab);
            Tile randomSpawnTile = gameManager.gridManager.GetHeroTile();

            randomSpawnTile.SetUnit(spawnedHero);
        }
    }

    private void SpawnEnemies()
    {
        int enemyCount = 3;

        for (int i = 0; i < enemyCount; i++)
        {
            BaseEnemy randomPrefab = GetRandomUnit<BaseEnemy>(Faction.Enemy);
            BaseEnemy spawnedEnemy = Object.Instantiate(randomPrefab);
            Tile randomSpawnTile = gameManager.gridManager.GetEnemyTile();

            spawnedEnemy.name = "Triangolo malvagio " + i;
            randomSpawnTile.SetUnit(spawnedEnemy);
        }
    }

    // Non sicuro di questa funz
    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit
    {
        // Tutte le unità dentro la lista di unità che sono di una certa fazione
        // Ordino queste unità secondo un numero casuale
        // Prendo la ref al prio elemento e ritorno il prefab

        return (T) _units.Where(unit => unit.faction == faction).OrderBy(o => Random.value).First().unitPrefab;
    }
}
