using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Ticket : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLook>().canLook = false;
            GameObject.FindWithTag("Player").GetComponent<PlayerLook>().EndGame();

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            Time.timeScale = 0;
        }
    }
}
