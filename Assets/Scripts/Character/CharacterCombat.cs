using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public abstract class CharacterCombat : MonoBehaviour
{
    [SerializeField] protected Character ownerCharacter;

    [Header("Combat Config")]
    [SerializeField] protected Projectile bulletPrefab;

    [Header("Pooling Size")]
    [SerializeField] protected int defaultCapacityPool;
    [SerializeField] protected int maxCapacityPool;

    [Header("General References")]
    [SerializeField] protected Transform bulletContainer;
    [SerializeField] protected Transform pointer;

    protected IObjectPool<Projectile> _bulletPool;

    private void Awake()
    {
        _bulletPool = new ObjectPool<Projectile>(
            createFunc      : CreateProjectile,
            actionOnGet     : OnGetFromPool,
            actionOnRelease : OnReleaseToPool,
            actionOnDestroy : OnDestroyPooledObject,
            collectionCheck : true,
            defaultCapacity : defaultCapacityPool,
            maxSize         : maxCapacityPool
            );
    }

    // 1. Triggered when the pool needs to create a brand new instance
    private Projectile CreateProjectile()
    {
        Projectile instance = Instantiate(bulletPrefab);
        instance.ObjectPool = _bulletPool; // Link the pool reference
        return instance;
    }

    // 2. Triggered when you fetch an object from the pool
    private void OnGetFromPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }

    // 3. Triggered when an object returns to the pool
    private void OnReleaseToPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    // 4. Triggered if the pool exceeds its max size limit
    private void OnDestroyPooledObject(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }

    public abstract void OnAttackByState();
    public abstract void OnAttack(Transform dire);
}
