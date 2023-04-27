using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public new string name;
    public Tile occupiedTile;

    public virtual void Init()
    {
        PubSub.Instance.RegisterFunction("ItemPicked", Interaction);
    }

    protected virtual void Interaction(object obj)
    {

    }
}
