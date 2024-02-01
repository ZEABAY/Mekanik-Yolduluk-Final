using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReseourceSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private List<ResourceData> resourceDataList;


    [Header("Raycast Settings")]
    [SerializeField] private float distanceBetweenCheck;
    [SerializeField] private float heightOfCheck = 10f;
    [SerializeField] private float rangeOfCheck = 30f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Vector2 positivePosition;
    [SerializeField] private Vector2 negativePosition;


    // Update is called once per frame
    void Start()
    {
        SpawnReseources();
    }
    /* Debug mod
     private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ClearAll();
            SpawnReseources();
        }
    }
    */
    private void ClearAll()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    private void SpawnReseources()
    {
        for (float x = negativePosition.x; x < positivePosition.x; x += distanceBetweenCheck)
        {
            for (float z = negativePosition.y; z < positivePosition.y; z += distanceBetweenCheck)
            {
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(x, heightOfCheck, z), Vector3.down, out hit, rangeOfCheck, layerMask))
                {

                    int i = UnityEngine.Random.Range(0, resourceDataList.Count);

                    if (resourceDataList[i].spawnChance > Random.Range(0, 101))
                    {
                        Quaternion randomRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
                        Instantiate(resourceDataList[i].prefab, hit.point, randomRotation, transform);
                    }
                }
            }
        }
    }
}


[System.Serializable]
public class ResourceData
{
    public GameObject prefab;
    public float spawnChance;
}