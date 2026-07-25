using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Config")]
    [SerializeField] private float lifeSpan;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletDamage;
    [SerializeField] private LayerMask attackableLayerMask;

    [SerializeField] private Rigidbody2D _rd2d;
    private float _currLifeSpan;
    private bool _isReleased;

    public IObjectPool<Projectile> ObjectPool { get; set; }

    public void Init(Vector2 spawnPosition, Vector2 direction)
    {
        _isReleased = false; // NEW — reset when reused from the pool
        _rd2d.position = spawnPosition;
        _currLifeSpan = 0;
        _rd2d.linearVelocity = direction.normalized * bulletSpeed;
    }

    private void Update()
    {
        if (_isReleased) return; // NEW — stop ticking once released

        if (_currLifeSpan >= lifeSpan)
        {
            ReleaseSelf();
            return;
        }
        _currLifeSpan += Time.deltaTime;
    }

    private static bool IsInLayerMask(int layer, LayerMask mask)
    {
        return (mask.value & (1 << layer)) != 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isReleased) return; // NEW — ignore any late/duplicate trigger calls

        if (!IsInLayerMask(collision.gameObject.layer, attackableLayerMask))
            return;

        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.ITakeDamage(bulletDamage);
            ReleaseSelf();
        }
    }

    private void ReleaseSelf() // NEW — single guarded release point
    {
        if (_isReleased) return;
        _isReleased = true;
        ObjectPool.Release(this);
    }
}
