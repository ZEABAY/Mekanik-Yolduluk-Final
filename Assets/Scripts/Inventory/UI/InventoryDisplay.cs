using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] MouseItemData mouseInventoryItem;

    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlot> slotDictonary; //Pair up the UI slots with the system slots


    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictonary => slotDictonary;

    protected virtual void Start()
    {

    }

    public abstract void AssignedSlot(InventorySystem invToDisplay); // Implemented in child classes


    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in SlotDictonary)
        {
            if (slot.Value == updatedSlot) // Slot value - the "under the hood" inventoryslot
            {
                slot.Key.UpdateUISlot(updatedSlot); // Slot Key - the UI representation of the value
            }
        }
    }

    public void SlotClicked(InventorySlot_UI clickedUISlot)
    {
        InventorySlot mouseAssignedSlot = mouseInventoryItem.AssignedInventorySlot;
        InventorySlot clickedAssignedSlot = clickedUISlot.AssignedInventorySlot;


        bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed;
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!//
        // Clicked slot has an item & mouse doesnt have an item -> pickup
        if (clickedAssignedSlot.ItemData != null && mouseAssignedSlot.ItemData == null)
        {
            // if player holding shift ->split
            if (isShiftPressed && clickedAssignedSlot.SplitStack(out InventorySlot halfStackSlot))
            {
                mouseInventoryItem.UpdateMouseSlot(halfStackSlot);
                clickedUISlot.UpdateUISlot();
            }
            else
            {
                // Pick up the item in clicked slot
                mouseInventoryItem.UpdateMouseSlot(clickedAssignedSlot);
                clickedUISlot.ClearSlot();
            }

            return;
        }

        // Clicked slot doesnt has an item & mouse have an item -> place to slot
        if (clickedAssignedSlot.ItemData == null && mouseAssignedSlot.ItemData != null)
        {
            clickedAssignedSlot.AssignItem(mouseAssignedSlot);
            clickedUISlot.UpdateUISlot();

            mouseInventoryItem.ClearSlot();
            return;
        }

        // Clicked slot has an item & mouse have an item ->
        if (clickedAssignedSlot.ItemData != null && mouseAssignedSlot.ItemData != null)
        {
            bool isSameItem = clickedAssignedSlot.ItemData == mouseAssignedSlot.ItemData;

            if (isSameItem && clickedAssignedSlot.EnoughRoomLeftInStack(mouseAssignedSlot.StackSize))  //// if same item combine them
            {
                clickedAssignedSlot.AssignItem(mouseAssignedSlot);
                clickedUISlot.UpdateUISlot();

                mouseInventoryItem.ClearSlot();
            }
            else if (isSameItem && !clickedAssignedSlot.EnoughRoomLeftInStack(mouseAssignedSlot.StackSize, out int leftInStack)) ////// if slot+mouse size > stacksize remain left on mouse
            {
                if (leftInStack < 1) // Slot max swap
                {
                    SwapSlots(clickedUISlot);
                }
                else
                {
                    // take what needed from mouse
                    int remainingOnMouse = mouseAssignedSlot.StackSize - leftInStack;
                    clickedAssignedSlot.AddToStack(leftInStack);

                    clickedUISlot.UpdateUISlot();

                    var newItem = new InventorySlot(mouseAssignedSlot.ItemData, remainingOnMouse);
                    mouseInventoryItem.ClearSlot();
                    mouseInventoryItem.UpdateMouseSlot(newItem);
                }
                return;
            }
            else if (!isSameItem) //// if different item swap them
            {
                SwapSlots(clickedUISlot);
                return;

            }

        }
    }

    private void SwapSlots(InventorySlot_UI clickedUISlot)
    {
        var clonedSlot = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, mouseInventoryItem.AssignedInventorySlot.StackSize);
        mouseInventoryItem.ClearSlot();

        mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);

        clickedUISlot.ClearSlot();
        clickedUISlot.AssignedInventorySlot.AssignItem(clonedSlot);
        clickedUISlot.UpdateUISlot();
    }
}
