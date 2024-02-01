using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Shop System/Shop Item List")]
public class ShopItemList : ScriptableObject
{
    [SerializeField] private List<ItemData> items;
    [SerializeField] private float buyRate;
    [SerializeField] private float sellRate;

    public List<ItemData> Items => items;
    public float BuyRate => buyRate;
    public float SellRate => sellRate;

}
