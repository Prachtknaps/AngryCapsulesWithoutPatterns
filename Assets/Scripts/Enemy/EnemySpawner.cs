using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Spawn Wave Settings")]
    [SerializeField] private int firstWaveMinEnemies = 10;
    [SerializeField] private int firstWaveMaxEnemies = 16;
    [SerializeField] private int waveMinEnemies = 2;
    [SerializeField] private int waveMaxEnemies = 8;
    [SerializeField] private float spawnInterval = 10.0f;

    [Header("Spawn Points")]
    [SerializeField] private List<GameObject> spawnPoints = new List<GameObject>();

    private bool isFirstWave = true;

    void Start()
    {
        if (spawnPoints.Count == 0)
        {
            return;
        }

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnWave();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnWave()
    {
        int minEnemies = isFirstWave ? firstWaveMinEnemies : waveMinEnemies;
        int maxEnemies = isFirstWave ? firstWaveMaxEnemies : waveMaxEnemies;
        isFirstWave = false;

        int numberOfEnemies = Random.Range(minEnemies, maxEnemies + 1);

        for (int i = 0; i < numberOfEnemies; i++)
        {
            GameObject randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            Vector3 spawnPosition = randomSpawnPoint.transform.position;

            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
