using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.Rendering.DebugUI;

public class PlayerLook : MonoBehaviour
{
    private Camera cam;
    private float xRotation = 0f;


    [SerializeField] private float xSensitivity = 30f;
    [SerializeField] private float ySensitivity = 30f;
    [SerializeField] private List<GameObject> UIPanels;
    public List<GameObject> UiPanels => UIPanels;
    public bool canLook = true;

    [Header("EndGame")]
    [SerializeField] GameObject go;
    [SerializeField] TextMeshProUGUI scoreText;
    private float score;

    private void Awake()
    {
        cam = Camera.main;
    }
    public void EndGame()
    {
        score = (float)System.Math.Round(score, 3);
        scoreText.text = $"Score : {score} Seconds";
        go.SetActive(true);
    }

    private void Update()
    {
        score += Time.deltaTime;
    }
    public void ProcessLook(Vector2 input)
    {
        foreach (GameObject panel in UIPanels)
        {
            if (panel.activeInHierarchy)
            {
                canLook = false;
                return;
            }
        }
        canLook = true;

        float mouseX = input.x;
        float mouseY = input.y;

        //Calculate camera rotation

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        //Apply
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //Rotate player
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);

    }
}
