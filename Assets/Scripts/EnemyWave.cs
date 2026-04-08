using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave")]
public class EnemyWave : ScriptableObject
{
    public int meleeEnemies;
    public int rangedEnemies;
    public int tankEnemies;
    public float delayBetweenSpawns;
    public float delayBeforeNextWave;    
}
