using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasketSlotUI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemText;

    public void SetItemTextAndIcon(string newString, ItemData data)
    {
        itemText.text = newString;
        itemSprite.preserveAspect = true;
        itemSprite.color = Color.white;
        itemSprite.sprite = data.Icon;
    }
}
