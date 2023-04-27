using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SetupState : State
{
    private GameManager gameManager;

    private List<ScriptableItem> _items;

    public SetupState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public override void OnEnter()
    {
        // Devo mandare il segnale per l'inizio gioco
        PubSub.Instance.Notify("GameStarted", gameManager);
        gameManager.StateMachine.SetState(GameState.Spawn);
        SpawnItems();

    }

    private void SpawnItems()
    {
        // Prendo ref agli item
        _items = Resources.LoadAll<ScriptableItem>("Items").ToList();

        Tile prevTile = null;
        Tile randomTile;

        for (int i = 0; i < _items.Count; i++)
        {
            // Prendo tile random
            do
            {
                randomTile = gameManager.gridManager.GetRandomTile();

            } while (prevTile == randomTile || !randomTile.Walkable || randomTile is GoalTile);

            Item item = _items[i].itemPrefab;
            Item spawnedItem = Object.Instantiate(item);

            randomTile.SetItem(spawnedItem);

            prevTile = randomTile;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
