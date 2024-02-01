using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;

public class InventorySlot_UI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private Image activeImage;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private InventorySlot assignedInventorySlot;

    //Switch it to private
    [SerializeField] protected bool selectedSlot;

    private Button button;

    public InventorySlot AssignedInventorySlot => assignedInventorySlot;
    public InventoryDisplay ParentDisplay { get; private set; }


    private void Awake()
    {
        ClearSlot();

        button = GetComponent<Button>();
        button?.onClick.AddListener(OnUISlotClick);

        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();

    }

    public void Init(InventorySlot slot)
    {
        assignedInventorySlot = slot;
        UpdateUISlot(slot);
    }

    public void UpdateUISlot(InventorySlot slot)
    {
        if (slot.ItemData != null)
        {
            itemSprite.sprite = slot.ItemData.Icon;
            itemSprite.color = Color.white;


            if (slot.StackSize > 1)
            {
                itemCount.text = slot.StackSize.ToString();
            }
            else
            {
                itemCount.text = string.Empty;
            }
        }
        else
        {
            ClearSlot();
        }
    }

    public void UpdateUISlot()
    {
        if (assignedInventorySlot != null)
        {
            UpdateUISlot(assignedInventorySlot);
        }
    }

    public void ClearSlot()
    {
        assignedInventorySlot?.ClearSlot();
        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = string.Empty;
    }

    public void OnUISlotClick()
    {
        ParentDisplay?.SlotClicked(this);
    }

    public void SelectSlot(bool selected)
    {
        activeImage.gameObject.SetActive(selected);
        selectedSlot = selected;

        GameObject handObjesi = GameObject.Find("Hand");

        if (handObjesi.transform.childCount > 0)
        {
            for (int i = 0; i < handObjesi.transform.childCount; i++)
            {
                GameObject childObj = handObjesi.transform.GetChild(i).gameObject;
                Destroy(childObj);
            }
        }

        if (assignedInventorySlot.ItemData == null)
        {
            return;
        }


        if (assignedInventorySlot.ItemData.Usable)
        {
            GameObject gameObjToHand = Instantiate(assignedInventorySlot.ItemData.UsableItemPrefab, handObjesi.transform.position, Quaternion.identity);

            gameObjToHand.transform.parent = handObjesi.transform;
            gameObjToHand.transform.position = handObjesi.transform.position;
            gameObjToHand.transform.rotation = handObjesi.transform.rotation;
            gameObjToHand.transform.localScale = handObjesi.transform.localScale;


        }
    }

}
