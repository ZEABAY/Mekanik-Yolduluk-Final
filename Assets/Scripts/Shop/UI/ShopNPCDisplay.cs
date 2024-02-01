using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopNPCDisplay : MonoBehaviour
{
    [SerializeField] private ShopSlotUI ShopItemUI;
    [SerializeField] private BasketSlotUI BasketSlotUI;


    [SerializeField] private Button buyTab;
    [SerializeField] private Button sellTab;


    [Header("Shopping Item")]
    [SerializeField] private TextMeshProUGUI basketTotalText;
    [SerializeField] private TextMeshProUGUI playerGold;
    [SerializeField] private Button buy_SellButton;
    [SerializeField] private TextMeshProUGUI buy_SellButtonText;

    [SerializeField] private GameObject shopListContentPanel;
    [SerializeField] private GameObject basketListContentPanel;



    private ShopSystem shopSystem;
    private PlayerInventoryHolder playerInventoryHolder;

    private Dictionary<ItemData, int> shoppingBasket = new Dictionary<ItemData, int>();
    private Dictionary<ItemData, BasketSlotUI> shoppingBasketUI = new Dictionary<ItemData, BasketSlotUI>();


    private bool isSelling;
    private int basketTotal;


    public void DisplayShopWindow(ShopSystem _shopSystem, PlayerInventoryHolder _playerInventoryHolder)
    {
        shopSystem = _shopSystem;
        playerInventoryHolder = _playerInventoryHolder;

        RefreshDisplay();
    }


    private void RefreshDisplay()
    {

        if (buy_SellButton != null)
        {
            buy_SellButtonText.text = isSelling ? "Sell Items" : "Buy Items";
            buy_SellButton.onClick.RemoveAllListeners();
            if (isSelling) buy_SellButton.onClick.AddListener(SellItems);
            else buy_SellButton.onClick.AddListener(buyItems);
        }

        ClearSlots();

        basketTotalText.enabled = false;
        buy_SellButton.gameObject.SetActive(false);
        basketTotal = 0;
        playerGold.text = $"Player Gold : {playerInventoryHolder.Gold}G";


        if (isSelling) DisplayPlayerInventory();
        else DisplayShopInventory();

    }

    private void ClearSlots()
    {
        shoppingBasket = new Dictionary<ItemData, int>();
        shoppingBasketUI = new Dictionary<ItemData, BasketSlotUI>();

        foreach (var item in shopListContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }

        foreach (var item in basketListContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
    }

    private void DisplayShopInventory()
    {
        foreach (var item in shopSystem.ShopInventory)
        {
            if (item.ItemData == null) continue;

            var shopSlot = Instantiate(ShopItemUI, shopListContentPanel.transform);
            shopSlot.Init(item, shopSystem.BuyRate, -1);
        }

    }
    private void DisplayPlayerInventory()
    {
        foreach (var item in playerInventoryHolder.SecondaryInventorySystem.GetAllItemsHeld())
        {
            var tempSlot = new ShopSlot();
            tempSlot.AssignItem(item.Key, item.Value);

            var shopSlot = Instantiate(ShopItemUI, shopListContentPanel.transform);
            shopSlot.Init(tempSlot, shopSystem.SellRate, item.Value);
        }
    }
    public void AddItemToBasket(ShopSlotUI shopSlotUI)
    {
        var data = shopSlotUI.AssignItemSlot.ItemData;

        var price = GetModifiedPrice(data, 1, shopSlotUI.Rate);


        if (shoppingBasket.ContainsKey(data))
        {
            shoppingBasket[data]++;
            var newString = $"{data.Displayname} ({price}G) x{shoppingBasket[data]}";
            shoppingBasketUI[data].SetItemTextAndIcon(newString, data);
        }
        else
        {
            shoppingBasket.Add(data, 1);
            var shoppingBasketTextObj = Instantiate(BasketSlotUI, basketListContentPanel.transform);
            var newString = $"{data.Displayname} ({price}G) x1";
            shoppingBasketTextObj.SetItemTextAndIcon(newString, data);
            shoppingBasketUI.Add(data, shoppingBasketTextObj);
        }


        basketTotal += price;
        basketTotalText.text = $"Total: {basketTotal}G";


        if (basketTotal > 0 && !basketTotalText.IsActive())
        {
            basketTotalText.enabled = true;
            buy_SellButton.gameObject.SetActive(true);
        }

        CheckBasketVsAvailableGold();

    }
    public void RemoveItemFromBasket(ShopSlotUI shopSlotUI)
    {
        var data = shopSlotUI.AssignItemSlot.ItemData;
        var price = GetModifiedPrice(data, 1, shopSlotUI.Rate);


        if (shoppingBasket.ContainsKey(data))
        {
            shoppingBasket[data]--;
            var newString = $"{data.Displayname} ({price}G) x{shoppingBasket[data]}";
            shoppingBasketUI[data].SetItemTextAndIcon(newString, data);

            if (shoppingBasket[data] <= 0)
            {
                shoppingBasket.Remove(data);
                var tempObj = shoppingBasketUI[data].gameObject;
                shoppingBasketUI.Remove(data);
                Destroy(tempObj);
            }
        }

        basketTotal -= price;
        basketTotalText.text = $"Total: {basketTotal}G";

        if (basketTotal <= 0 && basketTotalText.IsActive())
        {
            basketTotalText.enabled = false;
            buy_SellButton.gameObject.SetActive(false);
            return;
        }

        CheckBasketVsAvailableGold();
    }

    private void SellItems()
    {

        foreach (var item in shoppingBasket)
        {
            var price = GetModifiedPrice(item.Key, item.Value, shopSystem.SellRate);

            shopSystem.SellItem(item.Key);



            playerInventoryHolder.GainGold(price);
            playerInventoryHolder.SecondaryInventorySystem.RemoveItemsFromInventory(item.Key, item.Value);
        }

        RefreshDisplay();
    }

    private void buyItems()
    {
        if (playerInventoryHolder.Gold < basketTotal) return;

        if (!playerInventoryHolder.CheckInventoryRemaining(shoppingBasket)) return;

        foreach (var item in shoppingBasket)
        {
            shopSystem.PurchaseItem(item.Key, item.Value);

            for (int i = 0; i < item.Value; i++)
            {
                playerInventoryHolder.AddToInventory(item.Key, 1);
            }
        }

        playerInventoryHolder.SpendGold(basketTotal);

        RefreshDisplay();

    }

    private void CheckBasketVsAvailableGold()
    {
        var playerGold = playerInventoryHolder.Gold;

        basketTotalText.color = basketTotal > playerGold ? Color.red : Color.green;

        if (isSelling || playerInventoryHolder.CheckInventoryRemaining(shoppingBasket)) return;

        basketTotalText.text = "Not Enough Room In Inventory";
        basketTotalText.color = Color.red;

    }

    public static int GetModifiedPrice(ItemData data, int amount, float markup)
    {
        var baseValue = data.GoldValue * amount;

        return Mathf.FloorToInt(baseValue + baseValue * markup);
    }


    public void OnBuyTabPressed()
    {
        isSelling = false;
        RefreshDisplay();
    }
    public void OnSellTabPressed()
    {
        isSelling = true;
        RefreshDisplay();
    }

}
