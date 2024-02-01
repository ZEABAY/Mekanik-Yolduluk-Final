using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPart : MonoBehaviour
{
    private Button button;
    private BuildingData assignedData;
    private BuildingPanelUI parentDisplay;


    private void Awake()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(OnButtonClick);
    }


    public void Init(BuildingData aData, BuildingPanelUI pDisplay)
    {
        assignedData = aData;
        button.GetComponent<Image>().sprite = aData.Icon;
        parentDisplay = pDisplay;

    }

    private void OnButtonClick()
    {
        parentDisplay.OnClick(assignedData);
    }

}
