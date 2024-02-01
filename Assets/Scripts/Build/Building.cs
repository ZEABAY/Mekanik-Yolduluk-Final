using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Building : MonoBehaviour
{
    private BuildingData assignedData;
    private BoxCollider boxCollider;
    private GameObject graphic;
    private Transform colliders;
    private bool isOverlapping;

    public BuildingData AssignedData => assignedData;
    public bool IsOverlapping => isOverlapping;

    private Renderer myRenderer;
    private Material defaultMaterial;

    private bool flaggedForDelete;
    public bool FlaggedForDelete => flaggedForDelete;

    public void Init(BuildingData data)
    {
        assignedData = data;
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = assignedData.BuildingSize;
        boxCollider.center = new Vector3(0, (assignedData.BuildingSize.y + 0.2f) * 0.5f, 0);
        boxCollider.isTrigger = true;

        var rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        graphic = Instantiate(data.Prefab, transform);
        myRenderer = graphic.GetComponentInChildren<Renderer>();
        defaultMaterial = myRenderer.material;

        colliders = graphic.transform.Find("Colliders");
        if (colliders != null)
        {
            colliders.gameObject.SetActive(false);
        }
    }

    public void PlaceBuilding()
    {
        boxCollider.enabled = false;
        if (colliders != null)
        {
            colliders.gameObject.SetActive(true);
        }
        UpdateMaterial(defaultMaterial);
        gameObject.layer = 10; //Building layer int
        gameObject.name = assignedData.DisplayName + " - " + transform.position;
    }

    public void UpdateMaterial(Material newMaterial)
    {
        if (myRenderer == null)
        {
            return;
        }
        if (myRenderer.material != newMaterial)
        {
            myRenderer.material = newMaterial;
        }
    }

    public void FlagForDelete(Material deleteMat)
    {
        UpdateMaterial(deleteMat);
        flaggedForDelete = true;
    }

    public void RemoveDeleteFlag()
    {
        UpdateMaterial(defaultMaterial);
        flaggedForDelete = false;
    }

    private void OnTriggerStay(Collider other)
    {
        isOverlapping = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isOverlapping = false;
    }

}
