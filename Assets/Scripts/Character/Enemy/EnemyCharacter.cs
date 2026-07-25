using UnityEngine;
using UnityEngine.Pool;

public class EnemyCharacter : Character
{
    [SerializeField] private Transform endWayPosition;

    [Header("Path Following")]
    [SerializeField] private EnemyPath currentPath;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int lutSampleCount = 200;
    private Vector2 _previousPosition;
    private float[] _cumulativeDistances;
    private float _distanceTravelled;

    [Header("Enemy Combat System")]
    [SerializeField] private EnemyCombatCharacter enemyCombatCharacter;

    public IObjectPool<EnemyCharacter> ObjectPool { get; set; }
    public Transform EndPoint => endWayPosition;
    public EnemyCombatCharacter EnemyCombatCharacter => enemyCombatCharacter;

    private Rigidbody2D rb;
    private float _pathLength;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }

    private void BuildDistanceLUT()
    {
        _cumulativeDistances = new float[lutSampleCount + 1];
        Vector2 prevPoint = currentPath.GetSplinePoint(0f);
        _cumulativeDistances[0] = 0f;

        for (int i = 1; i <= lutSampleCount; i++)
        {
            float t = i / (float)lutSampleCount;
            Vector2 point = currentPath.GetSplinePoint(t);
            _cumulativeDistances[i] = _cumulativeDistances[i - 1] + Vector2.Distance(prevPoint, point);
            prevPoint = point;
        }

        _pathLength = _cumulativeDistances[lutSampleCount];
    }

    // Converts "how far we've travelled" into the correct t for that distance
    private float DistanceToT(float distance)
    {
        if (distance <= 0f) return 0f;
        if (distance >= _pathLength) return 1f;

        for (int i = 1; i <= lutSampleCount; i++)
        {
            if (_cumulativeDistances[i] >= distance)
            {
                float segStart = _cumulativeDistances[i - 1];
                float segEnd = _cumulativeDistances[i];
                float segT = (distance - segStart) / (segEnd - segStart); // lerp within this segment
                float tStart = (i - 1) / (float)lutSampleCount;
                float tEnd = i / (float)lutSampleCount;
                return Mathf.Lerp(tStart, tEnd, segT);
            }
        }
        return 1f;
    }

    private void FixedUpdate()
    {
        if (currentPath == null || currentPath.PointCount < 2)
            return;

        if (_distanceTravelled >= _pathLength)
        {
            CharacterMovement.StopAtBorder();
            ReachEndPoint();
            return;
        }

        _distanceTravelled += moveSpeed * Time.fixedDeltaTime; // now this IS actually distance
        _distanceTravelled = Mathf.Clamp(_distanceTravelled, 0f, _pathLength);

        float t = DistanceToT(_distanceTravelled);
        Vector2 targetPosition = currentPath.GetSplinePoint(t);
        CharacterMovement.OnMoveCharacterByPath(targetPosition);
    }

    public void InitEnemy(EnemyPath path, float speed)
    {
        isDead = false;
        moveSpeed = speed;

        _distanceTravelled = 0f;
        currentPath = path;
        if (path != null && path.PointCount > 0)
        {
            BuildDistanceLUT(); // replaces your old GetPathLength() call
            transform.position = path.GetSplinePoint(0f);
        }

        if (path != null && path.PointCount > 0)
        {
            _pathLength = path.GetPathLength(); // sampled + cached once here
            transform.position = path.GetSplinePoint(0f);
        }
        else
        {
            _pathLength = 0f;
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
        enemyCombatCharacter.ResetCombat();
    }

    public void ReachEndPoint()
    {
        Debug.Log($"{gameObject.name} is reach end point");

        if (ObjectPool != null)
            ObjectPool.Release(this);
    }

    public override void OnDeathCharacter()
    {
        if (isDead)
            return;

        isDead = true;

        SoundEffectManager.Instance.PlaySound2D("enemy_die");
        GlobalEvent.OnKillEnemy.Invoke();

        EntityCounterManager.Instance.RemoveEntityFormCounterByID(EntityID);
        ObjectPool.Release(this);
    }
}
