using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Inventory System/Item")]
public class ItemData : ScriptableObject
{
    public int ID;
    public string Displayname;
    [TextArea(4, 4)] public string Description;
    public Sprite Icon;
    public int MaxStackSize;
    public int GoldValue;
    public GameObject OnGroundItemPrefab;

    [Header("ItemCapabilities")]
    public bool Usable;
    public bool CanMelt;
    public bool CanBurn;
    public bool Craftable;


    [Header("Use Item")]
    public GameObject UsableItemPrefab;

    [Header("Craft Item")]
    public ItemData Requirement1;
    public int Requirement1Amount;
    public ItemData Requirement2;
    public int Requirement2Amount;

    [Header("ItemTiers")]
    public ItemData Tier2;
    public ItemData Tier3;



}


#if UNITY_EDITOR
[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        var data = (ItemData)target;

        if (data == null || data.Icon == null) return null;

        var texture = new Texture2D(width, height);
        EditorUtility.CopySerialized(data.Icon.texture, texture);
        return texture;
    }
}
#endif
