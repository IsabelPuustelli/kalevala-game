using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Default,
    Weapon,
    Food,
}
public abstract class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public int weight;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    public abstract void Use();
}
