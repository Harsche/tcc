using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour{
    [SerializeField] private Enemy_03 enemyPrefab;
    [SerializeField] private Transform projectilePrefab;
    [SerializeField] private float spawnDelay = 5f;
    [SerializeField] private int maxEnemies = 1;
    [SerializeField] private bool limitedSpawns;
    [SerializeField] private int spawnsCount = 5;
    [SerializeField] private bool spawnOnStart = true;
    private readonly List<Enemy_03> currentEnemies = new();

    private Coroutine spawnCoroutine;

    private void Awake(){
        if (spawnOnStart){ SpawnNewEnemy(); }
        spawnCoroutine = StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine(){
        WaitForSeconds waitForSeconds = new(spawnDelay);
        WaitUntil waitUntil = new(() => currentEnemies.Count < maxEnemies);
        while (true){
            yield return waitUntil;
            yield return waitForSeconds;
            SpawnNewEnemy();
        }
        // ReSharper disable once IteratorNeverReturns
    }

    public void SpawnNewEnemy(){
        if (limitedSpawns && spawnsCount > 0){
            spawnsCount--;
            if (spawnsCount == 0 && spawnCoroutine != null){ StopCoroutine(spawnCoroutine); }
        }

        Enemy_03 spawnedEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        if (projectilePrefab){ enemyPrefab.projectilePrefab = projectilePrefab;}
        currentEnemies.Add(spawnedEnemy);
        spawnedEnemy.OnDeath += enemy => currentEnemies.Remove(enemy);
    }
}