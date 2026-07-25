using UnityEngine;

public class BulletBurstEmitter : MonoBehaviour
{
    private GameObject smallBulletPrefab;
    private int burstCount;
    private float spreadAngle;
    private float smallBulletSpeed;
    private float burstInterval;
    private int maxBurstCount;

    private Vector3 lastPosition;
    private Vector2 travelDirection = Vector2.right;
    private float burstTimer;
    private int burstsFired;

    public void Configure(
        GameObject smallBulletPrefab,
        int burstCount,
        float spreadAngle,
        float smallBulletSpeed,
        float burstInterval,
         int maxBurstCount)
    {
        this.smallBulletPrefab = smallBulletPrefab;
        this.burstCount = burstCount;
        this.spreadAngle = spreadAngle;
        this.smallBulletSpeed = smallBulletSpeed;
        this.burstInterval = burstInterval;
        this.maxBurstCount = maxBurstCount;

        ResetState();
    }

    private void OnEnable()
    {
        // Guards against stale state if this GameObject gets reused from the pool
        // before Configure() is called again.
        ResetState();
    }

    private void ResetState()
    {
        burstTimer = 0f;
        lastPosition = transform.position;
    }

    private void Update()
    {
        if (maxBurstCount > 0 && burstsFired >= maxBurstCount)
            return;

        Vector2 delta = transform.position - lastPosition;
        if (delta.sqrMagnitude > 0.0001f)
            travelDirection = delta.normalized;
        lastPosition = transform.position;

        burstTimer += Time.deltaTime;
        if (burstTimer >= burstInterval)
        {
            burstTimer = 0f;
            burstsFired++;
            FireBurst();
        }
    }

    private void FireBurst()
    {
        if (smallBulletPrefab == null || burstCount <= 0) return;

        float baseAngle = Mathf.Atan2(travelDirection.y, travelDirection.x) * Mathf.Rad2Deg;
        float startAngle = baseAngle - spreadAngle / 2f;
        float step = burstCount > 1 ? spreadAngle / (burstCount - 1) : 0f;

        for (int i = 0; i < burstCount; i++)
        {
            float angle = startAngle + step * i;
            Vector2 dir = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject go = Instantiate(smallBulletPrefab, transform.position, Quaternion.identity);
            if (go.TryGetComponent<SmallBullet>(out var smallBullet))
            {
                smallBullet.Launch(dir, smallBulletSpeed);
            }
        }
    }
}
