using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    private bool canSummon = false;

    public GameObject defaultEnemy;
    public GameObject rangedEnemy;
    public GameObject tankEnemy;
    public GameObject defaultEnemyBoss;
    public GameObject rangedEnemyBoss;
    public GameObject tankEnemyBoss;
    public GameObject spawnCircle;
    public GameObject spawnParticle;

    public Transform spawnPointParent;
    public List<Transform> spawnPoints = new List<Transform>();
    public float spawnInterval = 0.15f;
    public float waveDelay = 10f;

    public int currentWave = 0;
    private bool isSpawning = false;
    public int numWaves = 7;
    
    public int default_weight = 50;
    public int ranged_weight = 32;
    public int tank_weight = 18;
    
    public int rangedUnlockWave = 3;
    public int tankUnlockWave = 5;

    public int currLevel = 1;
    
    public float spawnDefaultInterval = 0.15f;
    public float spawnRangedInterval = 0.2f;
    public float spawnTankInterval = 0.25f;

    private GameObject player;
    public float minDistanceFromPlayer = 20f;
    public float spawnCircleDuration = 2f;
    
    public void Start() {
        foreach (Transform t in spawnPointParent) spawnPoints.Add(t);
        
        currentWave = 0;
        isSpawning = false;
        player = GameObject.FindWithTag("Player");
        
        
        StartCoroutine(StartSummon());
    }

    public void Reset() {
        currentWave = 0;
        isSpawning = false;
        canSummon = false;
        
        StopAllCoroutines();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            Destroy(enemy);
        }
    }
    
    public IEnumerator StartSummon() {
        yield return new WaitForSeconds(5);
        StartCoroutine(StartNextWave());
    }
    
    IEnumerator StartNextWave() {
        if (isSpawning) yield break;
        
        currentWave++;
            
        isSpawning = true;

        // pre boss waves
        if (currentWave < numWaves) {
            int enemiesToSpawn = 3;
            if (currLevel == 1) enemiesToSpawn = 2 + currentWave * 3; // 5, 8, 11, 14, 17
            else if (currLevel == 2) enemiesToSpawn = 7 + currentWave * 3; // 10, 13, 16, 19, 22
            else if (currLevel == 3) enemiesToSpawn = 6 + currentWave * 4; // 10, 14, 18, 22, 26
            //int enemiesToSpawn = 20;

            for (int i = 0; i < enemiesToSpawn; i++) {
                int enemyType = GetWeightedEnemyType();
                GameObject prefab;
                float interval;
                if (enemyType == 1) {
                    prefab = defaultEnemy;
                    interval = spawnDefaultInterval;
                }
                else if (enemyType == 2) {
                    prefab = rangedEnemy;
                    interval = spawnRangedInterval;
                }
                else {
                    //if(enemyType == 3) {
                    prefab = tankEnemy;
                    interval = spawnTankInterval;
                }

                Transform spawnPoint = GetValidSpawnPoint();
                if (spawnPoint != null) {
                    StartCoroutine(SpawnWithIndicator(prefab, spawnPoint));
                }

                yield return new WaitForSeconds(interval);


                /*
                int enemyType = SpawnWeightedEnemy();

                if (enemyType == 1) yield return new WaitForSeconds(spawnDefaultInterval);
                else if (enemyType == 2) yield return new WaitForSeconds(spawnRangedInterval);
                else if (enemyType == 3) yield return new WaitForSeconds(spawnTankInterval);
                //yield return new WaitForSeconds(spawnInterval);
                */
            }

            isSpawning = false;
            yield return new WaitForSeconds(waveDelay);
            StartCoroutine(StartNextWave());
        }
        else {
            int enemiesToSpawn = 3;
            if (currLevel == 1) enemiesToSpawn = 2 + (2) * 3; // 8
            else if (currLevel == 2) enemiesToSpawn = 7 + (2) * 3; // 13
            else if (currLevel == 3) enemiesToSpawn = 6 + (2) * 4; // 14
            //int enemiesToSpawn = 20;

            for (int i = 0; i < enemiesToSpawn; i++) {
                int enemyType = GetWeightedEnemyType();
                GameObject prefab;
                float interval;
                if (enemyType == 1) {
                    prefab = defaultEnemy;
                    interval = spawnDefaultInterval;
                }
                else if (enemyType == 2) {
                    prefab = rangedEnemy;
                    interval = spawnRangedInterval;
                }
                else {
                    //if(enemyType == 3) {
                    prefab = tankEnemy;
                    interval = spawnTankInterval;
                }

                Transform spawnPoint = GetValidSpawnPoint();
                if (spawnPoint != null) {
                    StartCoroutine(SpawnWithIndicator(prefab, spawnPoint));
                }
            }

            GameObject bossPrefab = defaultEnemyBoss;
            switch (currLevel) {
                case 1:
                    bossPrefab = defaultEnemyBoss;
                    break;
                case 2:
                    bossPrefab = rangedEnemyBoss;
                    break;
                case 3:
                    bossPrefab = tankEnemyBoss;
                    break;
            }
            Transform bossSpawnPoint = GetValidSpawnPoint();
            if (bossSpawnPoint != null) {
                StartCoroutine(SpawnWithIndicator(bossPrefab, bossSpawnPoint));
            }
        }
    }

    private IEnumerator SpawnWithIndicator(GameObject prefab, Transform spawnPoint) {
        
        GameObject circle = Instantiate (spawnCircle, spawnPoint.position, spawnPoint.rotation);
        Image image = circle.GetComponentInChildren<Image> ();
        Renderer[] renderers = circle.GetComponentsInChildren<Renderer>();

        float elapsed = 0f;
        while (elapsed < spawnCircleDuration) {
            elapsed += Time.deltaTime;
            float progress = elapsed / spawnCircleDuration;
            
            image.fillAmount = progress;
            foreach (Renderer renderer in renderers) {
                renderer.material.SetFloat("_Fllipbook_Emissive", Mathf.Lerp(1f, 10f, progress));
            }

            yield return null;
        }
        
        Destroy(circle);
        
        yield return new WaitForSeconds(0.1f);
        
        Instantiate(spawnParticle, spawnPoint.position + new Vector3(0,0.5f,0),  Quaternion.Euler(0,0,0));
        Instantiate (prefab, spawnPoint.position, spawnPoint.rotation);
        
    }

    private Transform GetValidSpawnPoint() {
        int maxAttempts = 20;
        for (int i = 0; i < maxAttempts; i++) {
            int random_number = Random.Range(0, spawnPoints.Count);
            Transform spawnPoint = spawnPoints[random_number];
            float distToPlayer =  Vector3.Distance(player.transform.position, spawnPoint.position);

            if (distToPlayer < minDistanceFromPlayer) continue;
            LayerMask mask = LayerMask.GetMask("Player and Enemies");
            Collider[] hits = Physics.OverlapSphere (spawnPoint.position, 1.5f, mask);
            if (hits.Length > 0) continue;

            return spawnPoint;

        }

        return null;
    }

    private int GetWeightedEnemyType() {
        // gets the total unlocked weight through the current wave
        int totalWeight = default_weight;

        if (currentWave >= rangedUnlockWave) {
            totalWeight += ranged_weight;
        }
        
        if (currentWave >= tankUnlockWave) {
            totalWeight += tank_weight;
        }
        
        // get a random number to determine which enemy to spawn, then return it
        int roll = Random.Range(0, totalWeight);
        
        if (roll < default_weight) {
            return 1;
        }
        else if (roll < default_weight + ranged_weight && currentWave >= tankUnlockWave) {
            return 2;
        }
        else if (currentWave >= tank_weight) {
            return 3;
        }
        else {
            return 1;
        }
    }
    

    public void setCanSummon(bool canSummon) {
        this.canSummon = canSummon;
    }
}