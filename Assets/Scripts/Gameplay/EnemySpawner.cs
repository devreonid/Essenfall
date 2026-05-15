using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageConfig
{
    public string stageName = "Demo";
    public int maxEnemies = 5;
    
    [Tooltip("Waktu tunggu musuh pengganti setelah ada yang hancur (detik)")]
    public float minSpawnDelay = 10f;
    public float maxSpawnDelay = 20f;
    public GameObject[] enemyPrefabs; 
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Stage Settings")]
    public List<StageConfig> stages;
    public int currentStageIndex = 0; 

    [Header("Delay Settings")]
    [Tooltip("Jeda waktu sebelum musuh pertama kali muncul saat scene dimulai (dalam detik)")]
    public float initialDelay = 30f;

    [Header("Spawn Settings")]
    [Tooltip("Jika dicentang, layar akan langsung dipenuhi musuh sesuai maxEnemies di awal stage")]
    public bool spawnFullAtStart = true;
    public Transform[] spawnPoints; 

    private float spawnTimer;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool isInitialDelayFinished = false;

    void Start()
    {
        if (stages.Count > 0)
        {
            StartCoroutine(InitialDelayRoutine());
        }
    }

    private IEnumerator InitialDelayRoutine()
    {
        Debug.Log($"Masa persiapan... Musuh akan datang dalam {initialDelay} detik.");
        
        yield return new WaitForSeconds(initialDelay);
        
        isInitialDelayFinished = true;
        Debug.Log("Waktu habis! Musuh mulai menyerang.");

        if (spawnFullAtStart)
        {
            FillEnemiesToMax();
        }
        else
        {
            SetNextSpawnTimer();
        }
    }

    void Update()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);

        if (!isInitialDelayFinished) return;

        if (stages == null || stages.Count == 0 || currentStageIndex >= stages.Count) return;
        StageConfig currentStage = stages[currentStageIndex];

        if (activeEnemies.Count < currentStage.maxEnemies)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0)
            {
                SpawnEnemy(currentStage);
                SetNextSpawnTimer();
            }
        }
    }

    private void FillEnemiesToMax()
    {
        StageConfig currentStage = stages[currentStageIndex];
        
        int enemiesNeeded = currentStage.maxEnemies - activeEnemies.Count;
        for (int i = 0; i < enemiesNeeded; i++)
        {
            SpawnEnemy(currentStage);
        }
        
        SetNextSpawnTimer();
    }

    private void SpawnEnemy(StageConfig config)
    {
        if (spawnPoints.Length == 0 || config.enemyPrefabs.Length == 0) return;

        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject randomEnemyPrefab = config.enemyPrefabs[Random.Range(0, config.enemyPrefabs.Length)];

        GameObject spawnedEnemy = Instantiate(randomEnemyPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);
        activeEnemies.Add(spawnedEnemy);
    }

    private void SetNextSpawnTimer()
    {
        if (stages.Count > 0 && currentStageIndex < stages.Count)
        {
            StageConfig config = stages[currentStageIndex];
            spawnTimer = Random.Range(config.minSpawnDelay, config.maxSpawnDelay);
        }
    }
}