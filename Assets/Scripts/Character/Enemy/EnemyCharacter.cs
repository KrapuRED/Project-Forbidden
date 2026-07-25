using UnityEngine;
using UnityEngine.Pool;

public class EnemyCharacter : Character
{
    [SerializeField] private Transform endWayPosition;

    [Header("Path Following")]
    [SerializeField] private EnemyPath currentPath;
    [SerializeField] private float pathSpeed = 0.15f;

    private Rigidbody2D rb;
    public IObjectPool<EnemyCharacter> ObjectPool { get; set; }
    public Transform EndPoint => endWayPosition;
    private float _t;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (currentPath == null) return;

        if (_t >= 1f)
        {
            ReachEndPoint();
            return;
        }

        // 1. Progres t bertambah secara mulus
        _t = Mathf.Clamp01(_t + pathSpeed * Time.fixedDeltaTime);

        // 2. Ambil titik target persis di jalur
        Vector2 targetPosition = currentPath.GetSplinePoint(_t);

        // 3. Pindahkan Rigidbody2D secara halus tanpa jitter
        if (rb != null)
        {
            rb.MovePosition(targetPosition);
        }
        else
        {
            transform.position = targetPosition;
        }

        // 4. (Opsional) Tetap kirimkan arah ke CharacterMovement jika butuh rotasi/animasi
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        CharacterMovement.OnMoveCharacter(direction);
    }

    public void InitEnemy(EnemyPath path)
    {
        _t = 0f;

        currentPath = path;

        if (path != null && path.PointCount > 0)
        {
            transform.position = path.GetSplinePoint(0f);
        }

        CharacterHealth.Init(CharacterData.characterHealthAmount);

        string entityID = string.Empty;
        if (string.IsNullOrEmpty(EntityID))
        {
            entityID = EntityCounterManager.Instance.GetEntityID(this);
        }
        else
            entityID = EntityID;

        SetCharacterID(entityID);
    }

    public void ReachEndPoint()
    {
        Debug.Log($"{gameObject.name} is reach end point");

        if (ObjectPool != null)
            ObjectPool.Release(this);
    }

    public override void OnDeathCharacter()
    {
        Debug.Log($"{gameObject.name} is Dead");
        EntityCounterManager.Instance.RemoveEntityFormCounterByID(EntityID);
        ObjectPool.Release(this);
    }
}
