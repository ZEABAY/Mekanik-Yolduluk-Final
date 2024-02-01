using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : MonoBehaviour
{
    [SerializeField] private List<InventorySlot_UI> inventorySlot_UI;
    [SerializeField] private GameObject hand;

    private InventorySlot_UI currentSlot;
    private InventorySlot_UI nextSlot;
    private static int selectedHotbar;
    private void Awake()
    {
        currentSlot = inventorySlot_UI[0];
        currentSlot.SelectSlot(true);
        SelectHotbar(1);
    }

    public void SelectHotbar(int hotbarNo)
    {
        selectedHotbar = hotbarNo;
        if (hotbarNo == 0)
        {
            nextSlot = inventorySlot_UI[9];
        }
        else
        {
            nextSlot = inventorySlot_UI[hotbarNo - 1];
        }

        currentSlot.SelectSlot(false);
        currentSlot = nextSlot;

        currentSlot.SelectSlot(true);

    }

    public void UpdateHotbar()
    {
        SelectHotbar(selectedHotbar);
    }

    public void RemoveSelected()
    {
        if (selectedHotbar == 0)
        {
            inventorySlot_UI[9].AssignedInventorySlot.ClearSlot();
            inventorySlot_UI[9].UpdateUISlot();
        }
        else
        {
            inventorySlot_UI[selectedHotbar - 1].AssignedInventorySlot.ClearSlot();
            inventorySlot_UI[selectedHotbar - 1].UpdateUISlot();
        }
    }
}
