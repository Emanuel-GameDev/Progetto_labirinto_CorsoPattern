using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : Item
{
    public int value;

    public override void Init()
    {
        base.Init();
    }

    protected override void Interaction(object obj)
    {
        if (obj == null) return;
        if (obj is not Point) return;

        Point point = obj as Point;
        Tile source = point.occupiedTile;

        if (point != this) return;
        if (source.occupiedUnit is not BaseHero) return;
        BaseHero hero = source.occupiedUnit as BaseHero;

        hero.points += point.value;
    }
}
