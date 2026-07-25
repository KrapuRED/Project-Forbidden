using UnityEngine;
using UnityEngine.Pool;

public class SmallBullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private int damage = 1;

    private Vector2 direction;
    private float speed;

    public void Launch(Vector2 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.ITakeDamage(damage);
        }
    }
}
