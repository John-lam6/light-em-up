using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<Object> enemyTypes;
    // Enemy spawners and waves will eventually be provided by ArenaManager on scene load
    [SerializeField] private List<Transform> enemySpawners;
    [SerializeField] private List<EnemyWave> enemyWaves;
    private int currentSpawner;
    private int currentWave;
    
    void Start()
    {
        currentSpawner = 0;
        currentWave = 0;
        StartCoroutine(SpawnWave(enemyWaves[0]));
    }
    IEnumerator SpawnWave(EnemyWave wave)
    {  
        Debug.Log("Spawning wave " + currentWave + " at spawner " + currentSpawner);

        for(int i = 0; i < enemyTypes.Count; i++)
        {
            Object currentEnemyType = enemyTypes[i];
            int currentEnemyCount = i == 0 ? wave.meleeEnemies : i == 1 ? wave.rangedEnemies : wave.tankEnemies;
            for(int j = 0; j < currentEnemyCount; j++)
            {
                Object newEnemy = Instantiate(currentEnemyType);
                newEnemy.GameObject().transform.position = enemySpawners[currentSpawner].position;
                yield return new WaitForSeconds(wave.delayBetweenSpawns);
            }
        }

        currentSpawner = currentSpawner + 1 == enemySpawners.Count ? 0 : currentSpawner + 1;
        yield return new WaitForSeconds(wave.delayBeforeNextWave);

        currentWave++;
        if(currentWave < enemyWaves.Count)
        {
            StartCoroutine(SpawnWave(enemyWaves[currentWave]));    
        }
    }
}
