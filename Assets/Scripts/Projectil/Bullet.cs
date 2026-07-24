using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Config")]
    [SerializeField] private float lifeSpan;
    [SerializeField] private float bulletSpeed;

    [SerializeField] private Rigidbody2D _rd2d;
    private float _currLifeSpan;

    public void Init(Vector2 direction)
    {
        _rd2d.linearVelocity = direction.normalized * bulletSpeed;
    }

    private void Update()
    {
        if (_currLifeSpan >= lifeSpan)
        {
            Destroy(gameObject);
            return;
        }

        _currLifeSpan += Time.deltaTime;
    }
}
