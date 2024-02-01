using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemPrice;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private ShopSlot assignItemSlot;

    public ShopSlot AssignItemSlot => assignItemSlot;


    [SerializeField] private Button addItemToBasket;
    [SerializeField] private Button removeItemFromBasket;


    public ShopNPCDisplay ParentDisplay { get; private set; }
    public float Rate { get; private set; }

    private void Awake()
    {
        itemSprite.sprite = null;
        itemSprite.preserveAspect = true;
        itemSprite.color = Color.clear;
        itemName.text = string.Empty;
        itemCount.text = string.Empty;


        addItemToBasket?.onClick.AddListener(AddItemToBasket);
        removeItemFromBasket.onClick.AddListener(RemoveItemFromBasket);
        ParentDisplay = transform.parent.GetComponentInParent<ShopNPCDisplay>();
    }
    public void Init(ShopSlot slot, float rate, int _itemCount)
    {
        assignItemSlot = slot;
        Rate = rate;
        if (_itemCount == -1)
        {
            itemCount.text = "";
        }
        else
        {
            itemCount.text = _itemCount.ToString();
        }
        UpdateUISlot();
    }

    private void UpdateUISlot()
    {
        if (assignItemSlot.ItemData != null)
        {
            itemSprite.sprite = assignItemSlot.ItemData.Icon;
            itemSprite.color = Color.white;

            var modifiedPrice = ShopNPCDisplay.GetModifiedPrice(assignItemSlot.ItemData, 1, Rate);

            itemName.text = $"{assignItemSlot.ItemData.Displayname}";
            itemPrice.text = $"{modifiedPrice}G";
            int a = itemCount.text.Length;
            if (a != 0)
            {
                itemCount.text = $"x {itemCount.text}";
            }
        }
        else
        {
            itemSprite.sprite = null;
            itemSprite.color = Color.clear;
            itemName.text = string.Empty;
        }
    }

    public void AddItemToBasket()
    {
        if (itemCount.text != "")
        {
            string countWithoutX = itemCount.text.Replace("x ", "");

            int currentCount;
            if (int.TryParse(countWithoutX, out currentCount))
            {
                if (currentCount > 0)
                {
                    currentCount--;
                    itemCount.text = "x " + currentCount.ToString();
                    ParentDisplay.AddItemToBasket(this);

                }
            }
        }
        else
        {
            ParentDisplay.AddItemToBasket(this);
        }
    }
    public void RemoveItemFromBasket()
    {
        ParentDisplay.RemoveItemFromBasket(this);
    }
}
