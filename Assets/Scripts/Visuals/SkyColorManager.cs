using UnityEngine;

public class SkyColorManager : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;

    [Header("Day-Night Cycle")]
    [Tooltip("Total waktu yang dibutuhkan dari pagi (awal) hingga malam (akhir) dalam hitungan detik")]
    public float fullCycleDuration = 60f;
    
    [Tooltip("Atur warna dari Pagi (ujung kiri) sampai Malam (ujung kanan)")]
    public Gradient dayNightGradient;

    private float elapsedTime = 0f;

    void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera != null)
        {
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
        }
    }

    void Update()
    {
        if (mainCamera == null) return;

        elapsedTime += Time.deltaTime;

        float timeProgress = Mathf.Clamp01(elapsedTime / fullCycleDuration);

        mainCamera.backgroundColor = dayNightGradient.Evaluate(timeProgress);
    }
}