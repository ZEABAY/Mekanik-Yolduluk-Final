using UnityEngine;
using UnityEngine.Events;

public class ChestInventory : InventoryHolder, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    private UIManager uiManager;

    public void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("Canvas").gameObject.GetComponent<UIManager>();
    }
    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        OnDynamicInventoryDisplayRequested?.Invoke(primaryInventorySystem);
        uiManager.DisplayBackpack();
        interactSuccessful = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
