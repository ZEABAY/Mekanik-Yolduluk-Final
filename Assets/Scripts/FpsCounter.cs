using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI FPS;
    [SerializeField] private int FpsLimit = 144;



    private float updateInterval = 0.25f; // 0.5 saniyede bir g�ncelle
    private float accum = 0.0f; // Birikmi� zaman
    private int frames = 0; // Toplam kare say�s�
    private float timeLeft;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;  // V-Sync'i kapat
        Application.targetFrameRate = FpsLimit; // Hedef FPS
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        frames++;

        // Belirli bir s�re ge�tikten sonra FPS de�erini g�ncelle
        if (timeLeft <= 0.0)
        {
            float fps = accum / frames;
            FPS.text = Mathf.RoundToInt(fps).ToString(); // FPS'yi yuvarla ve stringe �evir
            timeLeft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }
}
