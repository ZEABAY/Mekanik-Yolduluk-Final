using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePrefab : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    private PlayerLook pLook;
    private Hotbar hotbar;
    private void Awake()
    {
        hotbar = GameObject.FindWithTag("Player").gameObject.GetComponent<Hotbar>();
        pLook = GameObject.FindWithTag("Player").gameObject.GetComponent<PlayerLook>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && pLook.canLook) // Left mouse button clicked
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Instantiate prefab at the clicked position
                Instantiate(prefab, hit.point, Quaternion.identity);
                hotbar.RemoveSelected();
                Destroy(this.gameObject);
            }
        }
    }
}
