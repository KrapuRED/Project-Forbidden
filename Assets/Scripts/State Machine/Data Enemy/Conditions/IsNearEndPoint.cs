using UnityEngine;

[CreateAssetMenu(fileName = "IsNearEndPointSO", menuName = "State Machine/Condition/IsNearEndPointSO")]
public class IsNearEndPoint : ConditionSO
{
    [SerializeField] private float stopThreshold = 0.15f;

    public override bool CheckCondition(EnemyCharacter ownerCharacter)
    {
        if (ownerCharacter == null || ownerCharacter.EndPoint == null)
            return false;

        float distance = Vector2.Distance(ownerCharacter.transform.position, ownerCharacter.EndPoint.position);

        return distance <= stopThreshold;
    }
}
