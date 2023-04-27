using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] GameObject _highlighted;

    [SerializeField] bool _isWalkable;
    public BaseUnit occupiedUnit;
    public Item occupiedItem;
    public Power occupiedPower;

    // Controllo che nel tile non ci sia una unit oppure sia erba
    public bool Walkable => _isWalkable && occupiedUnit == null;

    public virtual void Init(int x, int y)
    {

    }

    private void OnMouseEnter()
    {
        _highlighted.SetActive(true);
    }

    private void OnMouseExit()
    {
        _highlighted.SetActive(false);
    }

    public void SetUnit(BaseUnit unit)
    {
        if (unit.occupiedTile != null)
            unit.occupiedTile.occupiedUnit = null;

        unit.transform.position = transform.position;
        occupiedUnit = unit;
        unit.occupiedTile = this;
    }

    public void SetItem(Item item)
    {
        if(item.occupiedTile != null)
            item.occupiedTile.occupiedItem = null;
        
        item.transform.parent = transform;
        item.transform.position = transform.position;
        occupiedItem = item;
        item.occupiedTile = this;

        item.Init();
    }

    internal void SetPower(Power power)
    {
        if (power.occupiedTile != null)
            power.occupiedTile.occupiedItem = null;

        power.transform.parent = transform;
        power.transform.position = transform.position;
        occupiedPower = power;
        power.occupiedTile = this;

        power.Init();
    }

    private void OnMouseDown()
    {
        State tmpState = GameManager.instance.StateMachine.GetCurrentState();
        if (tmpState == null) return;

        if (tmpState is HeroTurnState)
        {
            HeroTurnState currentState = (HeroTurnState)tmpState;
            currentState.PerformTurn(this);
        }

    }

}
