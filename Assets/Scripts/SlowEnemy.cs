using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlowEnemy : Power
{
    [SerializeField] private int turnsToSkip = 2;

    private List<BaseEnemy> _enemies;

    public override void Init()
    {
        base.Init();
    }

    protected override void ApplyPower(object obj)
    {
        if (obj == null) return;

        BaseHero hero = obj as BaseHero;
        Tile source = hero.occupiedTile;

        if (source.occupiedPower != this) return;

        Debug.Log("Power Applied");
        _enemies = FindObjectsOfType<BaseEnemy>(true).ToList();

        foreach (BaseEnemy enemy in _enemies)
        {
            enemy.slowCurse += turnsToSkip;
        }
    }
}
