using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopNPC : MonoBehaviour, IInteractable
{

    [SerializeField] private ShopItemList shopItems;

    [SerializeField] private ShopSystem shopSystem;


    public static UnityAction<ShopSystem, PlayerInventoryHolder> OnShopWindowRequested;

    private void Awake()
    {
        shopSystem = new ShopSystem(shopItems.Items.Count, shopItems.BuyRate, shopItems.SellRate);

        foreach (var item in shopItems.Items)
        {
            shopSystem.AddToShop(item);
        }
    }
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        var playerInv = interactor.GetComponent<PlayerInventoryHolder>();
        if (playerInv != null)
        {
            OnShopWindowRequested?.Invoke(shopSystem, playerInv);
            interactSuccessful = true;
        }
        else
        {
            Debug.LogError("PlayerInventory NotFound");
            interactSuccessful = false;
        }

    }

}
