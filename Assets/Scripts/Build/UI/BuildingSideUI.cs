using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingSideUI : MonoBehaviour
{
    public Image BuildingImage;
    public TMP_Text BuildingText;


    private void Start()
    {
        BuildingImage.sprite = null;
        BuildingImage.color = Color.clear;
        BuildingText.text = string.Empty;

    }

    public void UpdateSideDisplay(BuildingData data)
    {
        BuildingImage.sprite = data.Icon;
        BuildingImage.color = Color.white;
        BuildingText.text = data.DisplayName;
    }

}
