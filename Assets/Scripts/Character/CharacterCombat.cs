using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class CharacterCombat : MonoBehaviour
{
    [SerializeField] private Character ownerCharacter;

    [Header("Combat Config")]
    [SerializeField] private Projectile bulletPrefab;

    [Header("Pooling Size")]
    [SerializeField] private int defaultCapacityPool;
    [SerializeField] private int maxCapacityPool;

    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Transform bulletContainer;
    [SerializeField] private Transform pointer;

    private IObjectPool<Projectile> _bulletPool;

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

    #region Event System
    private void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.actions["Combat"].performed += OnInputAttack;
            playerInput.actions["Combat"].canceled += OnInputAttack;

        }
    }

    private void OnDisable()
    {
        OnRemoveListener();
    }

    private void OnDestroy()
    {
        OnRemoveListener();
    }

    private void OnRemoveListener()
    {
        if (playerInput == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing playerInput!");
            return;
        }

        playerInput.actions["Combat"].performed -= OnInputAttack;
        playerInput.actions["Combat"].canceled -= OnInputAttack;
    }
    #endregion


    // 1. Triggered when the pool needs to create a brand new instance
    private Projectile CreateProjectile()
    {
        Projectile instance = Instantiate(bulletPrefab, bulletContainer);
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

    public void OnInputAttack(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnAttack();
    }


    public void OnAttack()
    {
        Vector2 direction = pointer.up;

        Projectile newBullet = _bulletPool.Get();
        newBullet.Init(pointer.position, direction);
    }
}
