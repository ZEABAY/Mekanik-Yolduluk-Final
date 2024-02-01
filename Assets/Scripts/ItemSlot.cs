using UnityEngine;
using UnityEngine.Rendering.Universal;

public abstract class ItemSlot
{

    [SerializeField] protected ItemData itemData; // Reference of the data
    [SerializeField] protected int stackSize; // Current stack size - how many of the data do we have
    public ItemData ItemData => itemData;
    public int StackSize => stackSize;

    public void ClearSlot() // Clears the slot
    {
        itemData = null;
        stackSize = -1;
    }

    public void AssignItem(InventorySlot invSlot) // Assign an item to the slot
    {
        if (itemData == invSlot.ItemData) // Does the slot contain same item
        {
            AddToStack(invSlot.stackSize); // Add to stack to
        }
        else
        {
            // Overwrite slot with the inventory slot that we're passing in
            itemData = invSlot.itemData;
            stackSize = 0;
            AddToStack(invSlot.stackSize);
        }
    }
    public void AssignItem(ItemData data, int amount)
    {
        if (itemData == data)
        {
            AddToStack(amount);
        }
        else
        {
            itemData = data;
            stackSize = 0;
            AddToStack(amount);
        }
    }

    public void SpecifyItem(ItemData data)
    {
        if (itemData == null)
        {
            itemData = data;
            stackSize = 0;
        }

    }

    public void AddToStack(int amount)
    {
        stackSize += amount;
        if (stackSize <= 0) ClearSlot();

    }

    public void RemoveFromStack(int amount)
    {

        stackSize -= amount;
        if (stackSize <= 0) ClearSlot();
    }

}
