using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Miner : MonoBehaviour, IInteractable
{
    [Header("ParticleSystem")]
    [SerializeField] GameObject particles;
    [Header("Slots")]
    [SerializeField] private Transform raycastPoint;
    [SerializeField] List<InventorySlot> slots;
    [SerializeField] public GameObject targetOBJ;
    private bool isProcessing = false;
    private int count = 0;
    private Lootable Lootable;
    private ItemData drop;
    private RaycastHit hit;
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public static UnityAction<List<InventorySlot>> MinerWindowRequested;
    private void Update()
    {
        BindToo();
        if (particles.gameObject.activeInHierarchy != isProcessing)
        {
            particles.gameObject.SetActive(isProcessing);
        }

        if (Physics.Raycast(raycastPoint.position, Vector3.down, out hit))
        {
            GameObject go = hit.collider.gameObject;
            Lootable = go.GetComponent<Lootable>();

            if (Lootable == null) return;
            drop = Lootable.Drop;

        }


        if (isProcessing) return;

        if (slots[0].ItemData == null) return;

        if (!slots[0].ItemData.CanBurn) return;

        if (slots[1].ItemData != null && slots[1].ItemData != drop) return;

        StartCoroutine(StartMiner());

    }

    private void BindToo()
    {
        if (targetOBJ == null) return;
        if (slots[1].ItemData == null) return;
        var target = targetOBJ.GetComponent<Furnace>();

        if (slots[1].ItemData.CanMelt)
        {
            if (target.slots[0].ItemData == null || target.slots[0].ItemData == slots[1].ItemData)
            {
                target.slots[0].AssignItem(slots[1].ItemData, 1);
                slots[1].AddToStack(-1);
            }
        }
        else if (slots[1].ItemData.CanBurn)
        {
            if (target.slots[1].ItemData == null || target.slots[1].ItemData == slots[1].ItemData)
            {
                target.slots[1].AssignItem(slots[1].ItemData, 1);
                slots[1].AddToStack(-1);
            }
        }

    }

    private IEnumerator StartMiner()
    {
        isProcessing = true;

        yield return new WaitForSeconds(3f);

        if (count >= 4)
        {
            slots[0].AddToStack(-1);
            count = 0;
        }
        else
        {
            count++;
        }

        slots[1].SpecifyItem(drop);
        slots[1].AddToStack(1);

        if (slots[0].StackSize <= 0)
        {
            slots[0].ClearSlot();
        }

        if (slots[1].StackSize <= 0)
        {
            slots[1].ClearSlot();
        }

        Lootable.GetMining();
        isProcessing = false;

    }
    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        MinerWindowRequested?.Invoke(slots);
        interactSuccessful = true;
    }

}
