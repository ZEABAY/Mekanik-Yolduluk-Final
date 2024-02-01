using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;

public class Furnace : MonoBehaviour, IInteractable
{
    [Header("Slots")]
    [SerializeField] public List<InventorySlot> slots;

    [Header("ParticleSystem")]
    [SerializeField] GameObject particles;


    public GameObject targetOBJ;
    private bool isProcessing = false;
    private int count = 0;
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public static UnityAction<List<InventorySlot>> FurnaceWindowRequested;

    private void Update()
    {
        BindToo();


        if (particles.gameObject.activeInHierarchy != isProcessing)
        {
            particles.gameObject.SetActive(isProcessing);
        }
        if (slots[0].ItemData != null && !isProcessing)
        {
            StartCoroutine(StartFurnace());
        }
    }

    private void BindToo()
    {
        if (targetOBJ == null) return;
        if (slots[2] == null) return;

        if (targetOBJ.GetComponent<ChestInventory>() != null)
        {
            var target = targetOBJ.GetComponent<ChestInventory>();
            target.primaryInventorySystem.AddToInventory(slots[2].ItemData, 1);
            slots[2].AddToStack(-1);
        }
    }

    private IEnumerator StartFurnace()
    {
        isProcessing = true;

        if (slots[0].ItemData == null || slots[1].ItemData == null)
        {
            isProcessing = false;
            yield break;
        }

        if (!slots[0].ItemData.CanMelt || !slots[1].ItemData.CanBurn)
        {
            isProcessing = false;
            yield break;
        }

        if (slots[2].ItemData != null && slots[2].ItemData != slots[0].ItemData.Tier2)
        {
            isProcessing = false;
            yield break;
        }



        yield return new WaitForSeconds(3f);

        slots[0].AddToStack(-1);
        if (count >= 4)
        {
            slots[1].AddToStack(-1);
            count = 0;
        }
        else
        {
            count++;
        }
        if (slots[0].ItemData == null)
        {
            isProcessing = false;
            yield break;
        }
        slots[2].SpecifyItem(slots[0].ItemData.Tier2);
        slots[2].AddToStack(1);


        if (slots[0].StackSize <= 0)
        {
            slots[0].ClearSlot();
        }

        if (slots[1].StackSize <= 0)
        {
            slots[1].ClearSlot();
        }

        if (slots[2].StackSize <= 0)
        {
            slots[2].ClearSlot();
        }

        isProcessing = false;
    }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        FurnaceWindowRequested?.Invoke(slots);
        interactSuccessful = true;
    }

    public bool AddToTarget(ItemData itemData, int v)
    {

        if (itemData.CanBurn)
        {
            if (itemData == slots[1].ItemData)
            {
                slots[1].AddToStack(v);
            }
        }

        if (itemData.CanMelt)
        {
            if (itemData == slots[0].ItemData)
            {
                slots[0].AddToStack(v);
            }
        }

        return false;
    }
}
