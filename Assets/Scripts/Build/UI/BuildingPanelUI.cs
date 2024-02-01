using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class BuildingPanelUI : MonoBehaviour
{
    public BuildingSideUI SideUI;
    public static UnityAction<BuildingData> OnPartChosen;

    public BuildingData[] KnownBuildingDatas;
    public BuildingPartUI BuildingButtonPrefab;

    public GameObject ItemWindow;

    public void OnClick(BuildingData choosenData)
    {
        OnPartChosen?.Invoke(choosenData);
        SideUI.UpdateSideDisplay(choosenData);
    }

    public void OnClickAllParts()
    {
        PopulateButton();
    }

    public void OnClickRoomParts()
    {
        Debug.Log("Room");
        PopulateButton(PartType.Room);
    }

    public void OnClickCorridorParts()
    {
        Debug.Log("Corridor");
        PopulateButton(PartType.Corridor);

    }

    public void PopulateButton()
    {
        SpawnButtons(KnownBuildingDatas);

    }

    public void PopulateButton(PartType chosenPartType)
    {
        var BuildingPieces = KnownBuildingDatas.Where(p => p.PartType == chosenPartType).ToArray();
        SpawnButtons(BuildingPieces);
    }

    public void SpawnButtons(BuildingData[] buttonData)
    {
        ClearButtons();
        foreach (var data in buttonData)
        {
            var spawnedButton = Instantiate(BuildingButtonPrefab, ItemWindow.transform);
            spawnedButton.Init(data, this);
        }
    }

    public void ClearButtons()
    {
        foreach (var button in ItemWindow.transform.Cast<Transform>())
        {
            Destroy(button.gameObject);
        }
    }

}
