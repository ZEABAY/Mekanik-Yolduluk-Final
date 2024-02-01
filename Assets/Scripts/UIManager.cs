using System;
using System.Collections.Generic;
using UnityEngine;
public class UIManager : MonoBehaviour
{

    public BuildingPanelUI buildPanel;
    public DynamicInventoryDisplay playerBackpack;
    public DynamicInventoryDisplay chestInventory;
    public PlayerInventoryHolder playerHolder;
    public ShopNPCDisplay shopPanel;
    public WorkbenchDisplay workbenchPanel;
    public FurnaceUIController furnacePanel;
    public MinerUIController minerPanel;


    private void Awake()
    {

    }
    private void OnEnable()
    {
        ShopNPC.OnShopWindowRequested += DisplayShopWindow;
        Workbench.OnWorkbenchWindowRequested += DisplayWorkbenchWindow;
        Furnace.FurnaceWindowRequested += DisplayFurnaceWindow;
        Miner.MinerWindowRequested += DisplayMinerWindow;
    }
    private void OnDisable()
    {
        ShopNPC.OnShopWindowRequested -= DisplayShopWindow;
        Workbench.OnWorkbenchWindowRequested -= DisplayWorkbenchWindow;
        Furnace.FurnaceWindowRequested -= DisplayFurnaceWindow;
        Miner.MinerWindowRequested -= DisplayMinerWindow;

    }
    private void DisplayShopWindow(ShopSystem shopSystem, PlayerInventoryHolder playerInv)
    {
        shopPanel.gameObject.SetActive(true);
        SetMouseCursorState(true);
        shopPanel.DisplayShopWindow(shopSystem, playerInv);
    }

    private void DisplayFurnaceWindow(List<InventorySlot> slots)
    {
        furnacePanel.gameObject.SetActive(true);
        SetMouseCursorState(true);

        furnacePanel.DisplayFurnace(slots);
        DisplayBackpack();
    }
    private void DisplayMinerWindow(List<InventorySlot> slots)
    {
        minerPanel.gameObject.SetActive(true);
        SetMouseCursorState(true);

        minerPanel.DisplayMiner(slots);
        DisplayBackpack();
    }
    private void DisplayWorkbenchWindow(CraftSystem craftSystem)
    {
        workbenchPanel.gameObject.SetActive(true);
        SetMouseCursorState(true);
        workbenchPanel.DisplayWorkbenchWindow(craftSystem);

    }
    private void Start()
    {
        buildPanel.gameObject.SetActive(false);

        SetMouseCursorState(false);
    }




    public void BuildPanelUI()
    {
        GameObject hand = GameObject.Find("Hand");
        if (hand == null)
        {
            Debug.Log("Hand obj is null ");
            return;
        }

        if (hand.GetComponentInChildren<BuildTool>())
        {

            buildPanel.gameObject.SetActive(!buildPanel.gameObject.activeInHierarchy);
            if (buildPanel.gameObject.activeInHierarchy)
            {
                buildPanel.PopulateButton();
            }

            SetMouseCursorState(buildPanel.gameObject.activeInHierarchy);

        }
    }

    public void Escape()
    {
        if (buildPanel.gameObject.activeInHierarchy)
        {
            BuildPanelUI();
        }

        if (playerBackpack.gameObject.activeInHierarchy)
        {
            DisplayBackpack();
        }

        if (chestInventory.gameObject.activeInHierarchy)
        {
            chestInventory.gameObject.SetActive(false);
            SetMouseCursorState(false);
        }

        if (shopPanel.gameObject.activeInHierarchy)
        {
            shopPanel.gameObject.SetActive(false);
            SetMouseCursorState(false);
        }

        if (workbenchPanel.gameObject.activeInHierarchy)
        {
            workbenchPanel.gameObject.SetActive(false);
            SetMouseCursorState(false);
        }

        if (furnacePanel.gameObject.activeInHierarchy)
        {
            furnacePanel.gameObject.SetActive(false);
            SetMouseCursorState(false);
        }

        if (minerPanel.gameObject.activeInHierarchy)
        {
            minerPanel.gameObject.SetActive(false);
            SetMouseCursorState(false);
        }
    }

    public void SetMouseCursorState(bool newState)
    {
        Cursor.visible = newState;
        Cursor.lockState = newState ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

    public void DisplayBackpack()
    {
        if (!playerBackpack.gameObject.activeInHierarchy)
        {
            PlayerInventoryHolder.OnPlayerBackpackDisplayRequested?.Invoke(playerHolder.SecondaryInventorySystem);
        }
        else
        {
            playerBackpack.gameObject.SetActive(false);
        }

        SetMouseCursorState(playerBackpack.gameObject.activeInHierarchy);

    }
}
