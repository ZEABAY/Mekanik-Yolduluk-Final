using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] protected int secondaryInventorySize;
    [SerializeField] protected InventorySystem secondaryInventorySystem;
    [SerializeField] private int gold;

    public int Gold => gold;

    public InventorySystem SecondaryInventorySystem => secondaryInventorySystem;

    public static UnityAction<InventorySystem> OnPlayerBackpackDisplayRequested;


    protected override void Awake()
    {
        base.Awake();

        secondaryInventorySystem = new InventorySystem(secondaryInventorySize);
    }

    private void Update()
    {

    }
    public bool AddToInventory(ItemData data, int amount)
    {
        if (secondaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }
        else if (primaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }
        return false;
    }

    public bool CheckInventoryRemaining(Dictionary<ItemData, int> shoppingCart)
    {
        var clonedSystem = new InventorySystem(this.secondaryInventorySize);

        for (int i = 0; i < secondaryInventorySize; i++)
        {
            clonedSystem.InventorySlots[i].AssignItem(this.secondaryInventorySystem.InventorySlots[i].ItemData,
                                                      this.secondaryInventorySystem.InventorySlots[i].StackSize);
        }

        foreach (var kvp in shoppingCart)
        {
            for (int i = 0; i < kvp.Value; i++)
            {
                if (!clonedSystem.AddToInventory(kvp.Key, 1)) return false;
            }
        }

        return true;
    }
    public void SpendGold(int basketTotal)
    {
        gold -= basketTotal;
    }
    public void GainGold(int price)
    {
        gold += price;
    }

}
