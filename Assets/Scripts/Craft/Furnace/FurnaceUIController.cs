using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceUIController : MonoBehaviour
{
    [Header("Mouse Slot")]
    [SerializeField] MouseItemData mouseInventoryItem;

    [Header("Furnace Slots")]
    [SerializeField] private InventorySlot_UI input;
    [SerializeField] private InventorySlot_UI fuel;
    [SerializeField] private InventorySlot_UI output;

    private List<InventorySlot> slots;
    private InventorySlot mouseSlot;

    private void Awake()
    {
        mouseSlot = mouseInventoryItem.AssignedInventorySlot;
    }
    private void Update()
    {
        ClearSlots();
        RefreshDisplay();
    }
    public void DisplayFurnace(List<InventorySlot> _slots)
    {
        slots = _slots;

        ClearSlots();
        RefreshDisplay();
    }

    private void ClearSlots()
    {

        input.ClearSlot();
        fuel.ClearSlot();
        output.ClearSlot();

    }

    public void ClickedInputSlot()
    {

        HandleSlots(slots[0], 0);
    }
    public void ClickedFuelSlot()
    {
        HandleSlots(slots[1], 1);

    }
    public void ClickedOutputSlot()
    {
        HandleSlots(slots[2], 2);
    }

    public void HandleSlots(InventorySlot slot, int currentSlot)
    {

        if (mouseSlot.ItemData != null && slot.ItemData == null)
        {
            slot.AssignItem(mouseSlot);
            input.UpdateUISlot();
            mouseInventoryItem.ClearSlot();
            return;
        }

        if (mouseSlot.ItemData == null && slot.ItemData != null)
        {
            mouseSlot.AssignItem(slot);
            mouseInventoryItem.UpdateMouseSlot();
            slot.ClearSlot();
            return;
        }

        bool isSameItem = slot.ItemData == mouseSlot.ItemData;
        if (isSameItem && slot.EnoughRoomLeftInStack(mouseSlot.StackSize))  //// if same item combine them
        {
            slot.AssignItem(mouseSlot);
            input.UpdateUISlot();

            mouseInventoryItem.ClearSlot();
        }
        else if (isSameItem && !slot.EnoughRoomLeftInStack(mouseSlot.StackSize, out int leftInStack)) ////// if slot+mouse size > stacksize remain left on mouse
        {

            if (leftInStack < 1) // Slot max swap
            {
                SwapSlots(currentSlot);
            }
            else
            {
                // take what needed from mouse
                int remainingOnMouse = mouseSlot.StackSize - leftInStack;
                slot.AddToStack(leftInStack);

                input.UpdateUISlot();

                var newItem = new InventorySlot(mouseSlot.ItemData, remainingOnMouse);
                mouseInventoryItem.ClearSlot();
                mouseInventoryItem.UpdateMouseSlot(newItem);
            }
            return;
        }
        else if (!isSameItem) //// if different item swap them
        {
            SwapSlots(currentSlot);
            return;
        }
    }


    private void RefreshDisplay()
    {
        input.AssignedInventorySlot.AssignItem(slots[0].ItemData, slots[0].StackSize);
        input.UpdateUISlot();

        fuel.AssignedInventorySlot.AssignItem(slots[1].ItemData, slots[1].StackSize);
        fuel.UpdateUISlot();

        output.AssignedInventorySlot.AssignItem(slots[2].ItemData, slots[2].StackSize);
        output.UpdateUISlot();

    }
    private void SwapSlots(int currentSlot)
    {
        InventorySlot tempSlot = new InventorySlot(slots[currentSlot].ItemData, slots[currentSlot].StackSize);
        InventorySlot tempMouse = new InventorySlot(mouseSlot.ItemData, mouseSlot.StackSize);

        mouseSlot.ClearSlot();
        slots[currentSlot].ClearSlot();

        mouseSlot.AssignItem(tempSlot);
        mouseInventoryItem.UpdateMouseSlot();

        slots[currentSlot].AssignItem(tempMouse);
        input.UpdateUISlot();

    }

}
