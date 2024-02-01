using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorkbenchDisplay : MonoBehaviour
{
    [SerializeField] CraftItemUI craftItem;

    [SerializeField] private GameObject craftListContentPanel;



    private CraftSystem craftSystem;

    public void DisplayWorkbenchWindow(CraftSystem _craftSystem)
    {
        craftSystem = _craftSystem;



        RefreshDisplay();

    }
    private void RefreshDisplay()
    {
        ClearSlots();
        DisplayWorkbenchInventory();
    }


    private void ClearSlots()
    {

        foreach (var item in craftListContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
    }
    private void DisplayWorkbenchInventory()
    {
        foreach (var item in craftSystem.WorkbenchInventory)
        {
            if (item.ItemData == null) continue;

            var craftSlot = Instantiate(craftItem, craftListContentPanel.transform);
            craftSlot.Init(item);
        }

    }
}
