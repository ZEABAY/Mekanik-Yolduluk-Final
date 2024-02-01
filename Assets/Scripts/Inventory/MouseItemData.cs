using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class MouseItemData : MonoBehaviour
{
    public Image ItemSprite;
    public TextMeshProUGUI ItemCount;
    public InventorySlot AssignedInventorySlot;

    [SerializeField] private float dropOffset = 3f;

    private Transform playerTransform;

    private void Awake()
    {
        ItemSprite.color = Color.clear;
        ItemSprite.preserveAspect = true;
        ItemCount.text = string.Empty;

        playerTransform = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
        if (playerTransform == null)
        {
            Debug.Log("Player Not Found");
        }
    }

    private void Update()
    {
        // TODO: Add controller support
        if (AssignedInventorySlot.ItemData != null) // If has an item follow the mouse position
        {
            transform.position = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
            {
                if (AssignedInventorySlot.ItemData.OnGroundItemPrefab != null)
                {
                    for (int i = 0; i < AssignedInventorySlot.StackSize; i++)
                    {
                        Instantiate(AssignedInventorySlot.ItemData.OnGroundItemPrefab, playerTransform.position + playerTransform.forward * dropOffset, Quaternion.identity);
                    }
                    ClearSlot();
                }
            }

            if (Mouse.current.rightButton.wasPressedThisFrame && !IsPointerOverUIObject())
            {
                if (AssignedInventorySlot.ItemData.OnGroundItemPrefab != null)
                {
                    Instantiate(AssignedInventorySlot.ItemData.OnGroundItemPrefab, playerTransform.position + playerTransform.forward * dropOffset, Quaternion.identity);
                }
                if (AssignedInventorySlot.StackSize > 1)
                {
                    AssignedInventorySlot.AddToStack(-1);
                    UpdateMouseSlot();
                }
                else
                {
                    ClearSlot();
                }
            }

        }
    }


    public void UpdateMouseSlot(InventorySlot invSlot)
    {
        AssignedInventorySlot.AssignItem(invSlot);
        UpdateMouseSlot();

    }

    public void UpdateMouseSlot()
    {
        ItemSprite.sprite = AssignedInventorySlot.ItemData.Icon;
        ItemSprite.color = Color.white;
        ItemCount.text = AssignedInventorySlot.StackSize.ToString();
        if (AssignedInventorySlot.StackSize == 1)
        {
            ItemCount.text = string.Empty;
        }

    }

    public void ClearSlot()
    {
        AssignedInventorySlot.ClearSlot();
        ItemSprite.color = Color.clear;
        ItemCount.text = string.Empty;
        ItemSprite.sprite = null;

        var hotbar = GameObject.FindWithTag("Player").GetComponent<Hotbar>();
        hotbar.UpdateHotbar();
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
