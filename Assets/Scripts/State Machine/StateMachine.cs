using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DataStateMachine
{
    public string nameStateCondition;
    public StateSO state;
    public ConditionSO condition;
}


public class StateMachine : MonoBehaviour
{
    [Header("State Machine Config")]
    [SerializeField] private EnemyCharacter ownerCharacter;
    [SerializeField] private List<DataStateMachine> dataStates = new();
    [SerializeField] private StateSO activeState;

    private void Update()
    {
        foreach (var data in dataStates)
        {
            if (data.condition.CheckCondition(ownerCharacter))
            {
                StateSO nextState = data.state;

                if (nextState != activeState)
                {
                    activeState?.ExitState(ownerCharacter);
                    activeState = nextState;
                    activeState.EnterState(ownerCharacter);
                }

                break;
            }
        }

        if (activeState != null)
            activeState.ExcuteState(ownerCharacter);
    }

    public void ResetCondition()
    {
        activeState?.ExitState(ownerCharacter);
        activeState = null;
    }
}
