using UnityEngine;

[System.Serializable]
public class InventorySlot : ItemSlot
{

    public InventorySlot(ItemData source, int amount) // Constructer to make a occupied inventory slot
    {
        itemData = source;
        stackSize = amount;
    }

    public InventorySlot() // Constructer to make a empty inventory slot
    {
        ClearSlot();
    }

    public void UpdateInventorySlot(ItemData data, int amount) // Updates slot directly
    {
        itemData = data;
        stackSize = amount;
    }

    public bool EnoughRoomLeftInStack(int amountToAdd, out int amountRemaining) // Would the be enuff room in the stack for the amount we're trying to add
    {
        amountRemaining = ItemData.MaxStackSize - stackSize;
        return EnoughRoomLeftInStack(amountToAdd);
    }

    public bool EnoughRoomLeftInStack(int amountToAdd)
    {
        return itemData == null || stackSize + amountToAdd <= ItemData.MaxStackSize;
    }


    public bool SplitStack(out InventorySlot splitStack)
    {
        if (StackSize <= 1) // Is there enough to split
        {
            // Return false
            splitStack = null;
            return false;
        }

        int halfStack = Mathf.RoundToInt(stackSize / 2); // Get the half the stack
        RemoveFromStack(halfStack);

        splitStack = new InventorySlot(itemData, halfStack); // Creates a copy of this slot with half the stack size
        return true;
    }
}
