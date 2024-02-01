using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BindTool : MonoBehaviour
{
    [SerializeField] private float distance = 6f;
    [SerializeField] private Material selectedMat;
    [SerializeField] private Transform startPoint;
    private GameObject sourceOBJ;
    private GameObject targetOBJ;

    private Ray ray;

    private Material defaultMat;

    void Update()
    {
        ray = new Ray(startPoint.transform.position, startPoint.transform.forward);


        SelectTarget();
        SelectSource();
    }

    private void SelectTarget()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, distance))
            {
                Debug.Log(hitInfo.collider.tag);
                if (hitInfo.collider.CompareTag("CanRemove"))
                {
                    GameObject go = hitInfo.collider.gameObject;
                    if (defaultMat == null)
                    {
                        defaultMat = go.GetComponent<MeshRenderer>().material;
                    }
                    go.GetComponent<MeshRenderer>().material = selectedMat;

                    targetOBJ = go;

                }
            }
        }
    }

    private void SelectSource()
    {
        if (Input.GetMouseButton(1))
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, distance))
            {
                if (hitInfo.collider.CompareTag("CanRemove"))
                {
                    GameObject go = hitInfo.collider.gameObject;

                    if (defaultMat != null)
                    {
                        targetOBJ.GetComponent<MeshRenderer>().material = defaultMat;
                        defaultMat = null;
                    }

                    sourceOBJ = go;
                    if (targetOBJ != null && sourceOBJ != null)
                    {
                        BindSourceToTarget();
                    }
                }
            }
        }
    }


    private void BindSourceToTarget()
    {
        if (sourceOBJ.GetComponent<Miner>() != null)
        {
            sourceOBJ.GetComponent<Miner>().targetOBJ = targetOBJ;
        }
        else if (sourceOBJ.GetComponent<Furnace>() != null)
        {
            sourceOBJ.GetComponent<Furnace>().targetOBJ = targetOBJ;
        }
    }
}
