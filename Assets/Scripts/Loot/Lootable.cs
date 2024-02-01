using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class Lootable : MonoBehaviour
{
    [SerializeField] private ItemData drop;
    [SerializeField] private float health;
    [SerializeField] private int dropAmount;
    [SerializeField] private Tools lootTool;

    public ItemData Drop => drop;


    public void Loot(Tools tool, PlayerInventoryHolder invHolder)
    {

        if (lootTool == tool)
        {
            health--;
            if (health > 0) return;
            Destroy(gameObject);
            invHolder.AddToInventory(drop, dropAmount);
        }
    }

    public void GetMining()
    {
        health -= 0.1f;
        if (health > 0) return;
        Destroy(gameObject);
    }
}
public enum Tools
{
    Axe,
    PickAxe,
    Shovel
}
