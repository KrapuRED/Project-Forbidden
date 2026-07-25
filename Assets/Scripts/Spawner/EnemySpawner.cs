using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [Header("Spawning Point Config")]
    [SerializeField] private List<EnemyPath> enemyPathDatas = new();

    [Header("Spawning Pool Config")]
    [SerializeField] private int defaultPoolCapacity;
    [SerializeField] private int maxPoolCapacity;

    [Header("Spawning Enemy Config")]
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private EnemyCharacter enemyPrefab;

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
        EnemyCharacter enemyCharacter = Instantiate(enemyPrefab, enemyContainer);
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
        OnSpawningEnemy();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpawningEnemy();
        }
    }

    private EnemyPath GetRandomPath()
    {
        int randomIndex = Random.Range(0, enemyPathDatas.Count);

        return enemyPathDatas[randomIndex];
    }

    public void OnSpawningEnemy()
    {
        EnemyCharacter newEnemy = _enemyPools.Get();

        bool spawnFromLeft = Random.value > 0.5f;

        EnemyPath selectdPath = GetRandomPath();

        newEnemy.InitEnemy(selectdPath);
    }
}
