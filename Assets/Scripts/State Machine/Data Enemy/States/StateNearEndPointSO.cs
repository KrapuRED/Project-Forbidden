using UnityEngine;


[CreateAssetMenu(fileName = "StateNearEndPointSO", menuName = "State Machine/States/StateNearEndPointSO")]
public class StateNearEndPointSO : StateSO
{
    public override void EnterState(EnemyCharacter character)
    {
        //character.ReachEndPoint();
    }

    public override void ExcuteState(EnemyCharacter character)
    {
        
    }

    public override void ExitState(EnemyCharacter character)
    {
        
    }
}
