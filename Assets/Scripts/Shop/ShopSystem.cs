using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ShopSystem
{
    [SerializeField] private List<ShopSlot> shopInventory;
    [SerializeField] private float buyRate;
    [SerializeField] private float sellRate;

    public List<ShopSlot> ShopInventory => shopInventory;
    public float BuyRate => buyRate;
    public float SellRate => sellRate;

    public ShopSystem(int size, float buyRate, float sellRate)
    {
        this.sellRate = sellRate;
        this.buyRate = buyRate;
        SetShopSize(size);

    }

    private void SetShopSize(int size)
    {
        shopInventory = new List<ShopSlot>(size);

        for (int i = 0; i < size; i++)
        {
            shopInventory.Add(new ShopSlot());
        }
    }
    public void AddToShop(ItemData data)
    {
        var freeSlot = GetFreeSlot();
        freeSlot.AssignItem(data, 1);
    }
    private ShopSlot GetFreeSlot()
    {
        var freeSlot = shopInventory.FirstOrDefault(i => i.ItemData == null);

        if (freeSlot == null)
        {
            freeSlot = new ShopSlot();
            shopInventory.Add(freeSlot);
        }

        return freeSlot;
    }
    public bool ContainsItem(ItemData itemToAdd, out ShopSlot shopSlot)
    {
        shopSlot = shopInventory.Find(i => i.ItemData == itemToAdd);

        return shopSlot != null;
    }

    public void PurchaseItem(ItemData key, int amount)
    {
        //if (!ContainsItem(ScriptableObject.CreateInstance<ItemData>(), out ShopSlot slot)) return;
        if (!ContainsItem(new ItemData(), out ShopSlot slot)) return;

        slot.RemoveFromStack(amount);
    }

    public void SellItem(ItemData key)
    {
        AddToShop(key);
    }
}
