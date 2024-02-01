using System;
using UnityEditor.Rendering;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    private Camera cam;
    private Ray ray;

    [SerializeField] private float distance = 6f;
    [SerializeField] private LayerMask mask;

    void Awake()
    {
        cam = Camera.main;

    }
    private void Update()
    {
        ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);
        RemovePrefabFromGroun();
    }

    private void RemovePrefabFromGroun()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, distance))
            {
                var interactable = hitInfo.collider.GetComponent<IInteractable>();
                if (hitInfo.collider.CompareTag("CanRemove"))
                {
                    PlayerInventoryHolder invHolder = GameObject.FindWithTag("Player").gameObject.GetComponent<PlayerInventoryHolder>();
                    CanRemove canR = hitInfo.collider.gameObject.GetComponent<CanRemove>();
                    invHolder.AddToInventory(canR.ItemData, 1);
                    canR.DestroyGO();

                }
            }
        }
    }

    public void StartInteraction()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            var interactable = hitInfo.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(this, out bool interactSuccessfull);
            }
        }
    }
}