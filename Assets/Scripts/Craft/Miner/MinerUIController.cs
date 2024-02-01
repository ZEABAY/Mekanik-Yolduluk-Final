using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class MinerUIController : MonoBehaviour
{
    [Header("Mouse Slot")]
    [SerializeField] MouseItemData mouseInventoryItem;

    [Header("Furnace Slots")]
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

    private void ClearSlots()
    {
        fuel.ClearSlot();
        output.ClearSlot();

    }

    private void RefreshDisplay()
    {
        fuel.AssignedInventorySlot.AssignItem(slots[0].ItemData, slots[0].StackSize);
        fuel.UpdateUISlot();

        output.AssignedInventorySlot.AssignItem(slots[1].ItemData, slots[1].StackSize);
        output.UpdateUISlot();
    }

    public void DisplayMiner(List<InventorySlot> _slots)
    {
        slots = _slots;

        ClearSlots();
        RefreshDisplay();
    }

    public void ClickedFuelSlot()
    {
        HandleSlots(slots[0], 0);

    }
    public void ClickedOutputSlot()
    {
        HandleSlots(slots[1], 1);
    }

    public void HandleSlots(InventorySlot slot, int currentSlot)
    {

        if (mouseSlot.ItemData != null && slot.ItemData == null)
        {
            slot.AssignItem(mouseSlot);
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



    private void SwapSlots(int currentSlot)
    {
        InventorySlot tempSlot = new InventorySlot(slots[currentSlot].ItemData, slots[currentSlot].StackSize);
        InventorySlot tempMouse = new InventorySlot(mouseSlot.ItemData, mouseSlot.StackSize);

        mouseSlot.ClearSlot();
        slots[currentSlot].ClearSlot();

        mouseSlot.AssignItem(tempSlot);
        mouseInventoryItem.UpdateMouseSlot();

        slots[currentSlot].AssignItem(tempMouse);

    }

}
