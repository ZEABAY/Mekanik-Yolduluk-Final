using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Craft System/Craft Item List")]
public class CraftItemList : ScriptableObject
{
    [SerializeField] private List<ItemData> items;

    public List<ItemData> Items => items;
}
