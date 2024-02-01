using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI FPS;
    [SerializeField] private int FpsLimit = 144;



    private float updateInterval = 0.25f; // 0.5 saniyede bir güncelle
    private float accum = 0.0f; // Birikmiþ zaman
    private int frames = 0; // Toplam kare sayýsý
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

        // Belirli bir süre geçtikten sonra FPS deðerini güncelle
        if (timeLeft <= 0.0)
        {
            float fps = accum / frames;
            FPS.text = Mathf.RoundToInt(fps).ToString(); // FPS'yi yuvarla ve stringe çevir
            timeLeft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }
}
