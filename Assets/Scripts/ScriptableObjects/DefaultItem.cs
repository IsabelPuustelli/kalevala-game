using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DefaultItem", menuName = "Inventory System/Items/Default")]
public class DefaultItem : ItemObject
{

    public void Awake()
    {
        type = ItemType.Default;
    }

    public override void Use()
    {
        Debug.Log("Using " + name);
    }

}
