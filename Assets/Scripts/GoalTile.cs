using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTile : Tile
{
    [SerializeField] GameObject lockGO;

    public bool locked => lockGO.activeSelf != false;

    public override void Init(int x, int y)
    {
        TriggerLock(true);
    }

    public void TriggerLock(bool mode)
    {
        lockGO.SetActive(mode); 
    }
}
