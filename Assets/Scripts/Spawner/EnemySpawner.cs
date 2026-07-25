using UnityEngine;
using UnityEngine.Pool;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [Header("Spawning Point Config")]
    [SerializeField] private float interval;
    [SerializeField] private List<EnemyPath> enemyPathDatas = new();

    [Header("Spawning Pool Config")]
    [SerializeField] private int defaultPoolCapacity;
    [SerializeField] private int maxPoolCapacity;

    [Header("Spawning Enemy Config")]
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private List<EnemyCharacter> enemyPrefabs = new();
    [SerializeField] private EnemyCharacter enemyPrefab;

    private Coroutine _spawnCoroutine;
    private int _maxSpawned;
    private int _currentSpawned;

    private IObjectPool<EnemyCharacter> _enemyPools ;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _enemyPools = new ObjectPool<EnemyCharacter>(
            createFunc      : CreateEnemyCharacter,
            actionOnGet     : OnGetFromPool,
            actionOnRelease : OnReleaseToPool,
            actionOnDestroy : OnDestroyPooledObject,
            collectionCheck : true,
            defaultCapacity : defaultPoolCapacity,
            maxSize         : maxPoolCapacity
            );
    }

    #region Pooling Optimize Config
    private EnemyCharacter CreateEnemyCharacter()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Count); 

        EnemyCharacter enemyCharacter = Instantiate(enemyPrefabs[randomIndex], enemyContainer);
        enemyCharacter.ObjectPool = _enemyPools;
        return enemyCharacter;
    }

    private void OnGetFromPool(EnemyCharacter enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(EnemyCharacter enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(EnemyCharacter enemy)
    {
        Destroy(enemy.gameObject);
    }
    #endregion

    private void Start()
    {
        StartSpawning(2.5f, 1);
    }

    private EnemyPath GetRandomPath()
    {
        int randomIndex = Random.Range(0, enemyPathDatas.Count);

        return enemyPathDatas[randomIndex];
    }

    private void OnSpawningEnemy(float speed, int spawned)
    {
        bool spawnFromLeft = Random.value > 0.5f;

        EnemyPath selectdPath = GetRandomPath();

        EnemyCharacter newEnemy = _enemyPools.Get();
        newEnemy.InitEnemy(selectdPath, speed);
    }

    public void StartSpawning(float speed, int countPerBatch)
    {
        StopSpawning();

        _maxSpawned = countPerBatch;
        _currentSpawned = 0;

        _spawnCoroutine = StartCoroutine(SpawnRoutine(speed, countPerBatch));
    }

    public void StopSpawning()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnRoutine(float speed, int countPerBatch)
    {
        WaitForSeconds wait = new WaitForSeconds(interval);

        while (_currentSpawned <= _maxSpawned)
        {
            OnSpawningEnemy(speed, countPerBatch);
            _currentSpawned++;
            yield return wait;
        }
    }
}
