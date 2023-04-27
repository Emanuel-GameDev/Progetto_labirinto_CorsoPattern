using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Power : MonoBehaviour
{
    public new string name;
    public string description;
    public Tile occupiedTile;

    public virtual void Init()
    {
        PubSub.Instance.RegisterFunction("PowerPicked", ApplyPower);
    }

    protected virtual void ApplyPower(object obj)
    {

    }
}
