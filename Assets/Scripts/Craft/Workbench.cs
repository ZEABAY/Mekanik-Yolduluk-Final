using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Workbench : MonoBehaviour, IInteractable
{
    [SerializeField] private CraftItemList craftItems;
    [SerializeField] private CraftSystem craftSystem;


    public static UnityAction<CraftSystem> OnWorkbenchWindowRequested;

    private void Awake()
    {
        craftSystem = new CraftSystem(craftItems.Items.Count);

        foreach (var item in craftItems.Items)
        {
            craftSystem.AddToWorkbench(item);
        }

    }



    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        var playerInv = interactor.GetComponent<PlayerInventoryHolder>();
        if (playerInv != null)
        {
            OnWorkbenchWindowRequested?.Invoke(craftSystem);
            interactSuccessful = true;
        }
        else
        {
            Debug.LogError("PlayerInventory NotFound");
            interactSuccessful = false;
        }
    }
}
