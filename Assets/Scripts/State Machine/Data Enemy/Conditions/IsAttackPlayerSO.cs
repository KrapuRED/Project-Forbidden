using UnityEngine;

[CreateAssetMenu(fileName = "IsAttackPlayerSO", menuName = "State Machine/Condition/IsAttackPlayerSO")]
public class IsAttackPlayerSO : ConditionSO
{
    public override bool CheckCondition(EnemyCharacter ownerChar)
    {
        return true;
    }
}
