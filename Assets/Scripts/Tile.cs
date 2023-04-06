using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] GameObject _highlighted;

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
}
