using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public enum SpawnModes
{
    Fixed,
    Random
}

public class Spawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private SpawnModes spawnMode = SpawnModes.Fixed;
    [SerializeField] private int enemyCount = 10;
    [SerializeField] private float delayBtwWaves = 1f;
    // [SerializeField] private GameObject testGO;

    [Header("Fixed Delay")]
    [SerializeField] private float delayBtwSpawns;

    [Header("Random Delay")]
    [SerializeField] private float minRandomDelay;
    [SerializeField] private float maxRandomDelay;

    private float _spawnTimer;
    private int _enemiesSpawned;
    private int _enemiesRamaining;

    private ObjectPooler _pooler;
    private Waypoint _waypoint;

    // Start is called before the first frame update
    void Start()
    {
        _pooler = GetComponent<ObjectPooler>();
        _waypoint = GetComponent<Waypoint>();
        _enemiesRamaining = enemyCount;
    }
 
    // Update is called once per frame
    void Update()
    {
        _spawnTimer -=Time.deltaTime;
        if(_spawnTimer < 0)
        {
            _spawnTimer = GetSpawnDelay();
            if(_enemiesSpawned < enemyCount)
            {
                _enemiesSpawned++;
                SpawnEnemy();
            }
        }
    }
    void SpawnEnemy()
    {
        GameObject newInstance = _pooler.GetInstanceFromPool();
        // Instantiate(newInstance ,transform.position , Quaternion.identity);
        Enemy enemy = newInstance.GetComponent<Enemy>();
        enemy.Waypoint = _waypoint;
        enemy.ResetEnemy();

        enemy.transform.localPosition = transform.position;
        newInstance.SetActive(true);

    }

    private float GetSpawnDelay()
    {
        float delay = 0f;
        if(spawnMode == SpawnModes.Fixed)
        {
            delay = delayBtwSpawns;
        }
        else	
        {	
            delay = GetRandomDelay();	
        }
        return delay;
    }
    
    private float GetRandomDelay()
    {
        float randomTimer = Random.Range(minRandomDelay , maxRandomDelay);
        return randomTimer;
    }

    private IEnumerator NextWave()
    {
        yield return new WaitForSeconds(delayBtwWaves);
        _enemiesRamaining = enemyCount;
        _spawnTimer = 0f;
        _enemiesSpawned = 0;
     
    }

    private void RecordEnemy(Enemy enemy)
    {
         _enemiesRamaining--;
         if(_enemiesRamaining <= 0)
         {
             StartCoroutine(NextWave());
         }
    }
    /// <summary>
    /// OnEnemyReached if enemy reached final position in a waypoint
    /// OnEnemyKilled we going record that
    /// </summary>

    private void OnEnable() 
    {
        Enemy.OnEndReached += RecordEnemy;

        EnemyHealth.OnEnemyKilled += RecordEnemy;
    }

    private void OnDisable() 
    {
        Enemy.OnEndReached -= RecordEnemy;
        EnemyHealth.OnEnemyKilled -= RecordEnemy;
    }
}
