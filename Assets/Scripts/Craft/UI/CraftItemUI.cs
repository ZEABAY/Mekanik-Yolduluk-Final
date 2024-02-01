using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftItemUI : MonoBehaviour
{
    [Header("Craft Item")]
    [SerializeField] private Image craftItemSprite;
    [SerializeField] private TextMeshProUGUI craftItemName;
    [SerializeField] private Button craft;
    [SerializeField] private CraftSlot assignItemSlot;

    public CraftSlot AssignItemSlot => assignItemSlot;

    [Header("Requariment Item 1")]
    [SerializeField] private Image requariment1ItemSprite;
    [SerializeField] private TextMeshProUGUI requariment1ItemCount;

    [Header("Requariment Item 2")]
    [SerializeField] private Image requariment2ItemSprite;
    [SerializeField] private TextMeshProUGUI requariment2ItemCount;


    private PlayerInventoryHolder playerInventoryHolder;
    private void Awake()
    {
        playerInventoryHolder = GameObject.FindWithTag("Player").GetComponent<PlayerInventoryHolder>();


        craftItemSprite.sprite = null;
        craftItemSprite.preserveAspect = true;
        craftItemSprite.color = Color.clear;
        craftItemName.text = string.Empty;


        requariment1ItemSprite.sprite = null;
        requariment1ItemSprite.preserveAspect = true;
        requariment1ItemSprite.color = Color.clear;
        requariment1ItemCount.text = string.Empty;


        requariment2ItemSprite.sprite = null;
        requariment2ItemSprite.preserveAspect = true;
        requariment2ItemSprite.color = Color.clear;
        requariment2ItemCount.text = string.Empty;


        craft?.onClick.AddListener(Craft);

    }

    public void Init(CraftSlot slot)
    {
        assignItemSlot = slot;
        UpdateUISlot();
    }

    private void UpdateUISlot()
    {

        craftItemSprite.sprite = assignItemSlot.ItemData.Icon;
        craftItemSprite.color = Color.white;
        craftItemName.text = assignItemSlot.ItemData.Displayname;


        requariment1ItemSprite.sprite = assignItemSlot.ItemData.Requirement1.Icon;
        requariment1ItemSprite.color = Color.white;
        requariment1ItemCount.text = $"x{assignItemSlot.ItemData.Requirement1Amount}";


        requariment2ItemSprite.sprite = assignItemSlot.ItemData.Requirement2.Icon; ;
        requariment2ItemSprite.color = Color.white;
        requariment2ItemCount.text = $"x{assignItemSlot.ItemData.Requirement2Amount}";

    }

    private void Craft()
    {
        if (playerInventoryHolder == null) return;
        if (assignItemSlot == null) return;
        if (!playerInventoryHolder.SecondaryInventorySystem.HasFreeSlot(out InventorySlot freeSlot)) return;

        //Check if requariments
        if (!CheckRequariments(assignItemSlot.ItemData.Requirement1, assignItemSlot.ItemData.Requirement1Amount)) return;
        if (!CheckRequariments(assignItemSlot.ItemData.Requirement2, assignItemSlot.ItemData.Requirement1Amount)) return;


        //Craft
        playerInventoryHolder.SecondaryInventorySystem.AddToInventory(assignItemSlot.ItemData, 1);
        playerInventoryHolder.SecondaryInventorySystem.RemoveItemsFromInventory(assignItemSlot.ItemData.Requirement1, assignItemSlot.ItemData.Requirement1Amount);
        playerInventoryHolder.SecondaryInventorySystem.RemoveItemsFromInventory(assignItemSlot.ItemData.Requirement2, assignItemSlot.ItemData.Requirement2Amount);
    }
    private bool CheckRequariments(ItemData requariment, int amount)
    {
        var distincItems = playerInventoryHolder.SecondaryInventorySystem.GetAllItemsHeld();
        bool contains = false;
        foreach (var item in distincItems)
        {
            if (item.Key == requariment && item.Value >= amount)
            {
                contains = true;
                Debug.Log(item.Key + " " + item.Value);
            }
        }

        return contains;

    }
}
