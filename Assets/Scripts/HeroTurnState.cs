using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroTurnState : State
{
    public GameManager gameManager;
    private int turnNum = 0;

    public BaseHero selectedHero;

    public HeroTurnState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public override void OnEnter()
    {
        Debug.Log($"Hero turn --- {turnNum}");
        // Dare indicazione al player su cosa può fare
    }

    public override void OnExit()
    {
        turnNum++;
        if (selectedHero != null)
            Debug.Log($"Hero points ---------- {selectedHero.points}");
    }

    public override void OnUpdate()
    {

    }

    private void CheckHeroWin()
    {
        if (selectedHero == null) return;
        if (selectedHero.occupiedTile is not GoalTile) return;

        GoalTile tile = selectedHero.occupiedTile as GoalTile;

        if (!tile.locked)
        {
            PubSub.Instance.Notify("GoalReached", GameManager.instance);
        }
    }

    public void PerformTurn(Tile selectedTile)
    {
        if (selectedTile.occupiedUnit != null)
        {
            // Clicco su un tile occupato

            if (selectedTile.occupiedUnit.faction == Faction.Hero && selectedHero == null)
            {
                SetSelectedHero((BaseHero)selectedTile.occupiedUnit);
                Debug.Log("Hero selected");
            }
            else
            {
                // Clicco su un nemico

                //if (currentState.selectedHero != null)
                //{
                //    BaseEnemy enemy = (BaseEnemy)occupiedUnit;
                //    Destroy(enemy.gameObject);
                //    currentState.SetSelectedHero(null);
                //}
            }
        }
        else
        {
            // Clicco su un tile vuoto

            if (selectedHero == null) return;

            // Chiamo funzione per avere tile adiacenti
            Dictionary<Vector2, Tile> adjacentTiles = new Dictionary<Vector2, Tile>();
            adjacentTiles = gameManager.gridManager.GetAdjacentTiles(selectedHero.occupiedTile);

            // Faccio muovere hero
            if (adjacentTiles.ContainsValue(selectedTile))
                MoveHero(selectedTile);


            // Cambio stato
            gameManager.StateMachine.SetState(GameState.EnemyState);

            // Tolgo selezione hero
            selectedHero = null;
        }
    }

    public void SetSelectedHero(BaseHero hero)
    {
        selectedHero = hero;
    }

    private void MoveHero(Tile dest)
    {
        if (!dest.Walkable) return;

        // Mi muovo
        selectedHero.transform.position = dest.transform.position;

        // Controllo nemico
        CheckEnemy(dest);

        // Setup tile e unità occupate
        selectedHero.occupiedTile.occupiedUnit = null;
        selectedHero.occupiedTile = dest;
        dest.occupiedUnit = selectedHero;

        // Controllo item
        CheckItem(dest);

        // Controllo power
        CheckPower(dest);

        // Controllo vittoria
        CheckHeroWin();

    }

    private void CheckPower(Tile dest)
    {
        if (dest.occupiedPower != null)
        {
            // Setup item su hero
            dest.occupiedPower.gameObject.SetActive(false);

            // Chiamo effetto Power
            PubSub.Instance.Notify("PowerPicked", selectedHero);

            // Tolgo item da tile
            dest.occupiedPower.occupiedTile = null;
            dest.occupiedPower = null;
        }
    }

    private void CheckItem(Tile dest)
    {
        if (dest.occupiedItem != null)
        {
            // Setup item su hero
            selectedHero.itemsCarried.Add(dest.occupiedItem);
            dest.occupiedItem.gameObject.transform.parent = selectedHero.transform;
            dest.occupiedItem.gameObject.transform.position = selectedHero.transform.position;
            dest.occupiedItem.gameObject.SetActive(false);
            Item currentItem = selectedHero.itemsCarried.Last();

            // Chiamo effetto item
            PubSub.Instance.Notify("ItemPicked", currentItem);

            // Tolgo item da tile
            dest.occupiedItem.occupiedTile = null;
            dest.occupiedItem = null;
        }
    }

    private void CheckEnemy(Tile dest)
    {
        // C'è un nemico nel tile di destinazione
        if (dest.occupiedUnit != null)
        {
            Debug.Log("YOU DIED");
            gameManager.Respawn();
        }
    }
}
