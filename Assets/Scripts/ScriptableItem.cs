using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Item")]
public class ScriptableItem : ScriptableObject
{
    public Item itemPrefab;
}
