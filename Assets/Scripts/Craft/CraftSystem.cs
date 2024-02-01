using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class CraftSystem
{
    [SerializeField] private List<CraftSlot> workbenchInventory;
    public List<CraftSlot> WorkbenchInventory => workbenchInventory;

    public CraftSystem(int size)
    {
        SetWorkbenchSize(size);
    }

    private void SetWorkbenchSize(int size)
    {
        workbenchInventory = new List<CraftSlot>(size);

        for (int i = 0; i < size; i++)
        {
            workbenchInventory.Add(new CraftSlot());
        }
    }
    public void AddToWorkbench(ItemData data)
    {
        var freeSlot = GetFreeSlot();
        freeSlot.AssignItem(data, 1);
    }

    private CraftSlot GetFreeSlot()
    {
        var freeSlot = workbenchInventory.FirstOrDefault(i => i.ItemData == null);

        if (freeSlot == null)
        {
            freeSlot = new CraftSlot();
            workbenchInventory.Add(freeSlot);
        }

        return freeSlot;
    }

    public bool ContainsItem(ItemData itemToAdd, out CraftSlot craftSlot)
    {
        craftSlot = workbenchInventory.Find(i => i.ItemData == itemToAdd);

        return craftSlot != null;
    }
}
