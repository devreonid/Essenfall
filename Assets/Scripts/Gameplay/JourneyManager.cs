using UnityEngine;
using System;

public class JourneyManager : MonoBehaviour
{
    [Header("Journey Settings")]
    [Tooltip("Total jarak dari awal sampai tujuan (dalam meter)")]
    public float totalDistance = 10000f; 
    [Tooltip("Kecepatan maksimal terbang zeppelin (meter per detik) saat 2 mesin menyala")]
    public float baseFlightSpeed = 15f; 

    [Header("Engine Dependencies")]
    [Tooltip("Masukkan objek Engine 1 yang memiliki script PartIntegrity")]
    public PartIntegrity engine1;
    [Tooltip("Masukkan objek Engine 2 yang memiliki script PartIntegrity")]
    public PartIntegrity engine2;

    public float currentDistance { get; private set; }
    private float currentFlightSpeed;

    public event Action<int, int> OnDistanceUpdated;

    private bool isJourneyComplete = false;

    void OnEnable()
    {
        if (engine1 != null) engine1.OnHealthChanged += CheckEngineStatus;
        if (engine2 != null) engine2.OnHealthChanged += CheckEngineStatus;
    }

    void OnDisable()
    {
        if (engine1 != null) engine1.OnHealthChanged -= CheckEngineStatus;
        if (engine2 != null) engine2.OnHealthChanged -= CheckEngineStatus;
    }

    void Start()
    {
        currentDistance = 0f;
        currentFlightSpeed = baseFlightSpeed;
        
        CheckEngineStatus(0, 0); 
    }

    void Update()
    {
        if (isJourneyComplete) return;

        currentDistance += currentFlightSpeed * Time.deltaTime;

        if (currentDistance >= totalDistance)
        {
            currentDistance = totalDistance;
            isJourneyComplete = true;
            JourneyCompleted();
        }

        OnDistanceUpdated?.Invoke(Mathf.FloorToInt(currentDistance), Mathf.FloorToInt(totalDistance));
    }

    private void CheckEngineStatus(int changedHP, int changedMaxHP)
    {
        int aliveEngines = 0;

        if (engine1 != null && engine1.currentHP > 0) aliveEngines++;
        if (engine2 != null && engine2.currentHP > 0) aliveEngines++;

        if (aliveEngines == 2)
        {
            currentFlightSpeed = baseFlightSpeed;
        }
        else if (aliveEngines == 1)
        {
            currentFlightSpeed = baseFlightSpeed / 2f;
        }
        else
        {
            currentFlightSpeed = 0f;
            Debug.LogWarning("KEDUA MESIN MATI! Zeppelin berhenti bergerak!");
        }
    }

    private void JourneyCompleted()
    {
        Debug.Log("Zeppelin telah tiba di tujuan! Stage Selesai.");
        
        if (GlobalTransition.Instance != null)
        {
            GlobalTransition.Instance.FadeOutAndLoadScene("DemoCompleted");
        }
    }
}