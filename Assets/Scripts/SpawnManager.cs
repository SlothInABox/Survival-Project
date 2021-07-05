using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Wave[] waves;
    public Enemy enemyPrefab;

    private Wave currentWave;
    private int currentWaveNumber;

    private int enemiesRemainingToSpawn;
    private int enemiesAlive;
    private float nextSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        NextWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenWave;

            Enemy spawnedEnemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity) as Enemy;
            spawnedEnemy.OnDeath += OnEnemyDeath; // Subscribe to event
        }
    }

    private void OnEnemyDeath()
    {
        enemiesAlive--;

        if (enemiesAlive == 0)
        {
            NextWave();
        }
    }

    private void NextWave()
    {
        currentWaveNumber++;
        if (currentWaveNumber - 1 < waves.Length)
            Debug.Log("Wave: " + currentWaveNumber);
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesAlive = enemiesRemainingToSpawn;
        }
    }

    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenWave;
    }
}
