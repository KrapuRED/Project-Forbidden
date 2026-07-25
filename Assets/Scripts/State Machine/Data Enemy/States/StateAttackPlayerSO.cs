using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "StateAttackPlayerSO", menuName = "State Machine/States/StateAttackPlayerSO")]
public class StateAttackPlayerSO : StateSO
{

    public override void EnterState(EnemyCharacter character)
    {
        character.EnemyCombatCharacter.SetPlayerPosition(
            EntityCounterManager.Instance.GetEntityByID("PL")?.entityPosition);
    }

    public override void ExcuteState(EnemyCharacter character)
    {
        if (character == null)
        {
            Debug.Log($"{character.name} is cannot find player Transform");
            return;
        }

        character.CharacterCombat.OnAttackByState();
    }

    public override void ExitState(EnemyCharacter character)
    {

    }
}
