using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPartUI : MonoBehaviour
{
    private Button button;
    private BuildingData assignedData;
    private BuildingPanelUI parentDisplay;


    public void Init(BuildingData aData, BuildingPanelUI pDisplay)
    {

        assignedData = aData;
        parentDisplay = pDisplay;

        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(OnButtonClick);
        button.GetComponent<Image>().sprite = aData.Icon;


    }

    private void OnButtonClick()
    {
        parentDisplay.OnClick(assignedData);
    }

}
