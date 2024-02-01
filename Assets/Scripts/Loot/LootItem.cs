using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LootItem : MonoBehaviour
{
    private Camera cam;
    private Ray ray;

    [SerializeField] private float distance = 2f;
    [SerializeField] private LayerMask mask;
    [SerializeField] private Tools tool;
    private PlayerInventoryHolder inventoryHolder;
    private Animator animator;
    private bool ishitting;

    void Awake()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
        inventoryHolder = GameObject.FindWithTag("Player").GetComponent<PlayerInventoryHolder>();
    }
    private void Update()
    {
        ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);

        if (Input.GetMouseButtonDown(0) && !ishitting)
        {
            Loot();
            animator.SetBool("Hit", true);
            StartCoroutine(Anim());
        }
    }


    public void Loot()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            var lootable = hitInfo.collider.GetComponent<Lootable>();
            if (lootable != null)
            {
                lootable.Loot(tool, inventoryHolder);
            }
        }
    }
    private IEnumerator Anim()
    {
        ishitting = true;
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("Hit", false);
        ishitting = false;
    }

}



