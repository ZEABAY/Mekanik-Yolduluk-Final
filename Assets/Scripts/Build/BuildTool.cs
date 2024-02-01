using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildTool : MonoBehaviour
{
    [SerializeField] private float rotateSnapAngle;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask buildModeLayerMask;
    [SerializeField] private LayerMask deleteModeLayerMask;
    [SerializeField] private int defaultLayerInt = 11;
    [SerializeField] private Material buildingMatPositive;
    [SerializeField] private Material buildingMatNegative;

    //private Transform rayOrigin;


    private bool deleteModeEnabled;

    private Camera cam;

    private Building spawnedBuilding;
    private Building targetBuilding;
    private Quaternion lastRotation;


    private void Start()
    {
        cam = Camera.main;
    }



    private void OnEnable()
    {
        BuildingPanelUI.OnPartChosen += ChoosePart;
    }

    private void OnDisable()
    {
        BuildingPanelUI.OnPartChosen -= ChoosePart;
    }

    private void OnDestroy()
    {
        var buildPreviewObject = GameObject.Find("Build Preview");
        if (buildPreviewObject == null) return;
        Destroy(buildPreviewObject);
    }

    private void ChoosePart(BuildingData data)
    {
        if (deleteModeEnabled)
        {

            if (targetBuilding != null && targetBuilding.FlaggedForDelete)
            {
                targetBuilding.RemoveDeleteFlag();
            }
            targetBuilding = null;
            deleteModeEnabled = false;
        }

        DeleteObjectPreview();

        var newGmObj = new GameObject()
        {
            layer = defaultLayerInt,
            name = "Build Preview"
        };

        spawnedBuilding = newGmObj.AddComponent<Building>();
        spawnedBuilding.Init(data);
        spawnedBuilding.transform.rotation = lastRotation;

    }

    private void Update()
    {


        Ray rayToPrint = new Ray(cam.transform.position, cam.transform.forward * rayDistance);
        Debug.DrawRay(rayToPrint.origin, rayToPrint.direction * rayDistance, Color.red);

        if (spawnedBuilding && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            DeleteObjectPreview();
        }

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            deleteModeEnabled = !deleteModeEnabled;
        }

        if (deleteModeEnabled)
        {
            DeleteModeLogic();
        }
        else
        {
            BuildModeLegic();
        }
    }

    private void DeleteObjectPreview()
    {
        if (spawnedBuilding != null)
        {
            Destroy(spawnedBuilding.gameObject);
            spawnedBuilding = null;
        }
    }

    private bool IsRayHittingSomething(LayerMask layerMask, out RaycastHit hitInfo)
    {
        var ray = new Ray(cam.transform.position, cam.transform.forward * rayDistance);

        return Physics.Raycast(ray, out hitInfo, rayDistance, layerMask);
    }

    private void DeleteModeLogic()
    {
        if (IsRayHittingSomething(deleteModeLayerMask, out RaycastHit hitInfo))
        {
            var detectedBuilding = hitInfo.collider.gameObject.GetComponentInParent<Building>();

            if (detectedBuilding == null)
            {
                return;
            }

            if (targetBuilding == null)
            {
                targetBuilding = detectedBuilding;
            }

            if (detectedBuilding != targetBuilding && targetBuilding.FlaggedForDelete)
            {
                targetBuilding.RemoveDeleteFlag();
                targetBuilding = detectedBuilding;
            }

            if (detectedBuilding == targetBuilding && !targetBuilding.FlaggedForDelete)
            {
                targetBuilding.FlagForDelete(buildingMatNegative);
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Destroy(targetBuilding.gameObject);
                targetBuilding = null;
            }
        }
        else
        {
            if (targetBuilding != null && targetBuilding.FlaggedForDelete)
            {
                targetBuilding.RemoveDeleteFlag();
                targetBuilding = null;
            }
        }
    }

    private void BuildModeLegic()
    {

        if (targetBuilding != null && targetBuilding.FlaggedForDelete)
        {
            targetBuilding.RemoveDeleteFlag();
            targetBuilding = null;
        }

        if (spawnedBuilding == null)
        {
            return;
        }

        PositionBuildingPreview();

    }

    private void PositionBuildingPreview()
    {
        spawnedBuilding.UpdateMaterial(spawnedBuilding.IsOverlapping ? buildingMatNegative : buildingMatPositive);

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            spawnedBuilding.transform.Rotate(0, rotateSnapAngle, 0);
            lastRotation = spawnedBuilding.transform.rotation;
        }


        if (IsRayHittingSomething(buildModeLayerMask, out RaycastHit hitInfo))
        {
            var gridPosition = WorldGrid.GridPositionFromWorldPosition(hitInfo.point, 3f);
            spawnedBuilding.transform.position = gridPosition;

            if (Mouse.current.leftButton.wasPressedThisFrame && !spawnedBuilding.IsOverlapping)
            {
                var playerLook = GameObject.FindWithTag("Player").GetComponent<PlayerLook>();
                foreach (GameObject panel in playerLook.UiPanels)
                {
                    if (panel.activeInHierarchy)
                    {
                        Debug.Log(panel + " is active");
                        return;
                    }
                }
                spawnedBuilding.PlaceBuilding();
                var dataCopy = spawnedBuilding.AssignedData;
                spawnedBuilding = null;
                ChoosePart(dataCopy);
            }
        }
    }
}
