using UnityEngine;
using UnityEngine.Pool;

public class EnemyCharacter : Character
{
    [SerializeField] private Transform endWayPosition;

    [Header("Path Following")]
    [SerializeField] private EnemyPath currentPath;
    [SerializeField] private float moveSpeed = 3f;
    private Vector2 _previousPosition;

    [Header("Enemy Combat System")]
    [SerializeField] private EnemyCombatCharacter enemyCombatCharacter;

    public IObjectPool<EnemyCharacter> ObjectPool { get; set; }
    public Transform EndPoint => endWayPosition;
    public EnemyCombatCharacter EnemyCombatCharacter => enemyCombatCharacter;

    private Rigidbody2D rb;
    private float _t; 
    private float _pathLength;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (currentPath == null || currentPath.PointCount < 2)
            return;

        if (_t >= 1f)
        {
            CharacterMovement.StopAtBorder();
            ReachEndPoint();
            return;
        }

        if (_pathLength > 0f)
        {
            float deltaT = (moveSpeed * Time.fixedDeltaTime) / _pathLength;
            _t = Mathf.Clamp01(_t + deltaT);
        }

        Vector2 targetPosition = currentPath.GetSplinePoint(_t);
        CharacterMovement.OnMoveCharacterByPath(targetPosition);
    }

    public void InitEnemy(EnemyPath path)
    {
        _t = 0f;
        isDead = false;
        currentPath = path;

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
        Debug.Log($"{gameObject.name} is Dead");
        EntityCounterManager.Instance.RemoveEntityFormCounterByID(EntityID);
        //ObjectPool.Release(this);
    }
}
