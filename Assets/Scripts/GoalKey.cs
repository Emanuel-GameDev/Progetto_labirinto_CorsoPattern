using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalKey : Item
{
    public Color color;

    public override void Init()
    {
        base.Init();
    }

    protected override void Interaction(object obj)
    {
        if (obj == null) return;
        if (obj is not GoalKey) return;

        GoalKey goalKey = obj as GoalKey;

        if (goalKey != this) return;

        State tmpState = GameManager.instance.StateMachine.GetCurrentState();
        if (tmpState == null) return;
        HeroTurnState currentState = tmpState as HeroTurnState;

        GoalTile goalTile = currentState.gameManager.gridManager.GetGoalTile();
        Color goalColor = goalTile.gameObject.GetComponent<SpriteRenderer>().color;

        if (Math.Round(goalKey.color.r, 2) == Math.Round(goalColor.r, 2)
            && Math.Round(goalKey.color.g, 2) == Math.Round(goalColor.g, 2)
            && Math.Round(goalKey.color.b, 2) == Math.Round(goalColor.b, 2))
        {
            goalTile.TriggerLock(false);
        }
    }
}
